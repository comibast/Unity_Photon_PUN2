using UnityEngine;
using Photon.Pun;

namespace Comibast
{
    /// <summary>
    /// ��������G�޲z���a�i�J�᪫�󪺥ͦ�
    /// </summary>
    public class SceneControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("���a�w�s��")]
        private GameObject prefabPlayer;

        private void Awake()
        {
            InitializePlayer();
        }

        /// <summary>
        /// ��l�ƪ��a
        /// </summary>
        private void InitializePlayer()
        {
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(-5f, 5f);
            pos.y = 6f;
            PhotonNetwork.Instantiate(prefabPlayer.name, pos, Quaternion.identity);
        }
    }
}

