using Photon.Pun;
using TMPro;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

namespace Comibast
{
    /// <summary>
    /// 玩家控制器
    /// </summary>
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("移動速度"), Range(0, 10)]
        private float speed = 3.5f;
        [Header("檢查地板資料")]
        [SerializeField] private Vector3 groundOffset;
        [SerializeField] private Vector3 groundSize;
        [SerializeField, Header("跳躍高度"), Range(0, 1000)]
        private float jump = 30f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "開關走路";
        private bool isGround;
        private Transform childCanvas;
        private TextMeshProUGUI textFruit;
        private int countFruit;
        private int countFruitMax = 3;            //最多吃到三個就過關
        private CanvasGroup groupGame;
        private TextMeshProUGUI textWinner;
        private Button btnBackToLobby;

        private void OnDrawGizmos()
        {

            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
            Gizmos.DrawCube(transform.position + groundOffset, groundSize);
        }

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
            childCanvas = transform.GetChild(0);

            //如果 不是自己的物件 關閉元件
            if (!photonView.IsMine) enabled = false;

            photonView.RPC("RPCUpdateName", RpcTarget.All);

            textFruit = transform.Find("畫布玩家名稱/水果數量").GetComponent<TextMeshProUGUI>();
            groupGame = GameObject.Find("畫布遊戲介面").GetComponent<CanvasGroup>();
            textWinner = GameObject.Find("勝利者").GetComponent<TextMeshProUGUI>();

            btnBackToLobby = GameObject.Find("返回遊戲大廳").GetComponent<Button>();
            btnBackToLobby.onClick.AddListener(() =>
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("遊戲大廳");
                }
            });
        }

        private void Start()
        {
            GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        }

        private void Update()
        {
            Move();
            CheckGround();
            Jump();
            BackToTop();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Contains("草莓"))
            {
                Destroy(collision.gameObject);

                textFruit.text = (++countFruit).ToString();

                if (countFruit >= countFruitMax) Win();
            }
        }

        /// <summary>
        /// 玩家掉落後回到場景上方
        /// </summary>
        private void BackToTop()
        {
            if (transform.position.y < -20)
            {
                rig.velocity = Vector3.zero;                    //掉落速度
                transform.position = new Vector3(0, 15, 0);     //掉落後重生座標
            }
        }

        /// <summary>
        /// 獲勝
        /// </summary>
        private void Win()
        {
            groupGame.alpha = 1;
            groupGame.interactable = true;
            groupGame.blocksRaycasts = true;

            textWinner.text = "獲勝玩家：" + photonView.Owner.NickName;

            DestroyObject();
        }

        /// <summary>
        /// 獲勝後刪除物件
        /// </summary>
        private void DestroyObject()
        {
            GameObject[] fruits = GameObject.FindGameObjectsWithTag("水果");
            for (int i = 0; i < fruits.Length; i++) Destroy(fruits[i]);
            Destroy(FindObjectOfType<SpawnFruit>().gameObject);
        }

        [PunRPC]
        private void RPCUpdateName()
        {
            transform.Find("畫布玩家名稱/名稱介面").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void Move()
        {
            //A　←：-1
            //D　→：+1
            //沒按：0
            float h = Input.GetAxis("Horizontal");
            rig.velocity = new Vector2(speed * h, rig.velocity.y);
            ani.SetBool(parWalk, h != 0);

            if (Mathf.Abs(h) < 0) return;
            transform.eulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
            childCanvas.localEulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
        }

        /// <summary>
        /// 檢查地板
        /// </summary>
        private void CheckGround()
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position + groundOffset, groundSize, 0);
            //print(hit.name);
            isGround = hit;
        }

        /// <summary>
        /// 跳躍
        /// </summary>
        private void Jump()
        {
            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                rig.AddForce(new Vector2(0, jump));
            }
        }
    }

}
