using Photon.Pun;
using UnityEngine;

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

        private Rigidbody2D rig;

        private void OnDrawGizmos()
        {

            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
            Gizmos.DrawCube(transform.position + groundOffset, groundSize);
        }

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();

            //如果 不是自己的物件 關閉元件
            if (!photonView.IsMine) enabled = false;
        }

        private void Update()
        {
            Move();
            CheckGround();
        }

        private void Move()
        {
            //A　←：-1
            //D　→：+1
            //沒按：0
            float h = Input.GetAxis("Horizontal");
            rig.velocity = new Vector2(speed * h, rig.velocity.y);
        }

        private void CheckGround()
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position + groundOffset, groundSize, 0);
            print(hit.name);
        }

    }

}
