using Photon.Pun;
using UnityEngine;

namespace Comibast
{
    /// <summary>
    /// �j�U�޲z��
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// �s�u�ܥD����k
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            print("<color=yellow>�w�g�s�u�ܥD��</color>");
        }

    }

}

