using Photon.Pun;
using UnityEngine;

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

        private Rigidbody2D rig;

        private void OnDrawGizmos()
        {

            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
            Gizmos.DrawCube(transform.position + groundOffset, groundSize);
        }

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();

            //�p�G ���O�ۤv������ ��������
            if (!photonView.IsMine) enabled = false;
        }

        private void Update()
        {
            Move();
            CheckGround();
        }

        private void Move()
        {
            //A�@���G-1
            //D�@���G+1
            //�S���G0
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
