using Photon.Pun;
using TMPro;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

namespace Comibast
{
    /// <summary>
    /// ���a���
    /// </summary>
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("���ʳt��"), Range(0, 10)]
        private float speed = 3.5f;
        [Header("�ˬd�a�O���")]
        [SerializeField] private Vector3 groundOffset;
        [SerializeField] private Vector3 groundSize;
        [SerializeField, Header("���D����"), Range(0, 1000)]
        private float jump = 30f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "�}������";
        private bool isGround;
        private Transform childCanvas;
        private TextMeshProUGUI textFruit;
        private int countFruit;
        private int countFruitMax = 3;            //�̦h�Y��T�ӴN�L��
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

            //�p�G ���O�ۤv������ ��������
            if (!photonView.IsMine) enabled = false;

            photonView.RPC("RPCUpdateName", RpcTarget.All);

            textFruit = transform.Find("�e�����a�W��/���G�ƶq").GetComponent<TextMeshProUGUI>();
            groupGame = GameObject.Find("�e���C������").GetComponent<CanvasGroup>();
            textWinner = GameObject.Find("�ӧQ��").GetComponent<TextMeshProUGUI>();

            btnBackToLobby = GameObject.Find("��^�C���j�U").GetComponent<Button>();
            btnBackToLobby.onClick.AddListener(() =>
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("�C���j�U");
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
            if (collision.name.Contains("���"))
            {
                Destroy(collision.gameObject);

                textFruit.text = (++countFruit).ToString();

                if (countFruit >= countFruitMax) Win();
            }
        }

        /// <summary>
        /// ���a������^������W��
        /// </summary>
        private void BackToTop()
        {
            if (transform.position.y < -20)
            {
                rig.velocity = Vector3.zero;                    //�����t��
                transform.position = new Vector3(0, 15, 0);     //�����᭫�ͮy��
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        private void Win()
        {
            groupGame.alpha = 1;
            groupGame.interactable = true;
            groupGame.blocksRaycasts = true;

            textWinner.text = "��Ӫ��a�G" + photonView.Owner.NickName;

            DestroyObject();
        }

        /// <summary>
        /// ��ӫ�R������
        /// </summary>
        private void DestroyObject()
        {
            GameObject[] fruits = GameObject.FindGameObjectsWithTag("���G");
            for (int i = 0; i < fruits.Length; i++) Destroy(fruits[i]);
            Destroy(FindObjectOfType<SpawnFruit>().gameObject);
        }

        [PunRPC]
        private void RPCUpdateName()
        {
            transform.Find("�e�����a�W��/�W�٤���").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Move()
        {
            //A�@���G-1
            //D�@���G+1
            //�S���G0
            float h = Input.GetAxis("Horizontal");
            rig.velocity = new Vector2(speed * h, rig.velocity.y);
            ani.SetBool(parWalk, h != 0);

            if (Mathf.Abs(h) < 0) return;
            transform.eulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
            childCanvas.localEulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
        }

        /// <summary>
        /// �ˬd�a�O
        /// </summary>
        private void CheckGround()
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position + groundOffset, groundSize, 0);
            //print(hit.name);
            isGround = hit;
        }

        /// <summary>
        /// ���D
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
