using Photon.Pun;
using UnityEngine;

namespace Comibast
{
    /// <summary>
    /// 大廳管理器
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 連線至主機方法
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            print("<color=yellow>已經連線至主機</color>");
        }

    }

}

