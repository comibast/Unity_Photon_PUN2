using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Comibast
{
    /// <summary>
    /// 大廳管理器
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region 資料
        private TMP_InputField inputFieldPlayerName;
        private TMP_InputField inputFieldCreateRoomName;
        private TMP_InputField inputFieldJoinRoomName;

        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Button btnJoinRandomRoom;

        private string namePlayer;
        private string nameCreateRoom;
        private string nameJoinRoom;

        /// <summary>
        /// 畫布主要
        /// </summary>
        private CanvasGroup groupMain;
        #endregion

        private void Awake()
        {
            inputFieldPlayerName = GameObject.Find("輸入欄位玩家名稱").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("輸入欄位房間名稱").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("輸入欄位指定房間名稱").GetComponent<TMP_InputField>();

            btnCreateRoom = GameObject.Find("按鈕創建房間").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("按鈕加入指定房間").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("按鈕加入隨機房間").GetComponent<Button>();

            groupMain = GameObject.Find("畫布主要").GetComponent<CanvasGroup>();
            //結束編輯：按下 Enter 或者在其他地方點左鍵
            //輸入欄位.結束編輯.添加監聽((輸入欄位的輸入字串) => 儲存)
            inputFieldPlayerName.onEndEdit.AddListener((input) => namePlayer = input);
            inputFieldCreateRoomName.onEndEdit.AddListener((input) => nameCreateRoom = input);
            inputFieldJoinRoomName.onEndEdit.AddListener((input) => nameJoinRoom = input);

            btnCreateRoom.onClick.AddListener(CreateRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);

            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 連線至主機方法
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            groupMain.interactable = true;
            groupMain.blocksRaycasts = true;
            print("<color=yellow>連線至主機成功！</color>");
        }

        /// <summary>
        /// 創建房間
        /// </summary>
        private void CreateRoom()
        {
            //房間選項：https://doc-api.photonengine.com/en/PUN/v2/class_photon_1_1_realtime_1_1_room_options.html
            RoomOptions ro = new RoomOptions();
            //最大人數與可視性
            ro.MaxPlayers = 20;
            ro.IsVisible = true;
            //Photon 連線.創建房間(房間名稱，房間選項)
            PhotonNetwork.CreateRoom(nameCreateRoom, ro);
        }

        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(nameJoinRoom);
        }

        private void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print("<color=green>創建房間成功</color>");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("<color=green>加入房間成功</color>");
        }


    }

}

