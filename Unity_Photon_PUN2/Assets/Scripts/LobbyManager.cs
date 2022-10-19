using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Comibast
{
    /// <summary>
    /// �j�U�޲z��
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region ��Ƥj�U
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
        /// �e���D�n
        /// </summary>
        private CanvasGroup groupMain;
        #endregion

        #region ��Ʃж�
        private TextMeshProUGUI textRoomName;
        private TextMeshProUGUI textRoomPlayer;
        private CanvasGroup groupRoom;
        private Button btnStartGame;
        private Button btnLeaveRoom;
        #endregion

        private void Awake()
        {
            GetLobbyObjectAndEvent();
            textRoomName = GameObject.Find("��r�ж��W��").GetComponent<TextMeshProUGUI>();
            textRoomPlayer = GameObject.Find("��r�ж��H��").GetComponent<TextMeshProUGUI>();
            groupRoom = GameObject.Find("�e���ж�").GetComponent<CanvasGroup>();
            btnStartGame = GameObject.Find("���s�}�l�C��").GetComponent<Button>();
            btnLeaveRoom = GameObject.Find("���s���}�ж�").GetComponent<Button>();

            btnLeaveRoom.onClick.AddListener(LeaveRoom);

            //PhotonView ���ݦP�B�Ȥ�("RPC ��k", �w�墨�Ǫ��a)
            btnStartGame.onClick.AddListener(() => photonView.RPC("RPCStartGame", RpcTarget.All));

            PhotonNetwork.ConnectUsingSettings();
        }

        //���ݦP�B�Ȥ�ݤ�k
        [PunRPC]
        private void RPCStartGame()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("�C������");
        }

        /// <summary>
        /// ���o�j�U����P�ƥ�
        /// </summary>
        private void GetLobbyObjectAndEvent()
        {
            inputFieldPlayerName = GameObject.Find("��J��쪱�a�W��").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("��J���ж��W��").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("��J�����w�ж��W��").GetComponent<TMP_InputField>();

            btnCreateRoom = GameObject.Find("���s�Ыةж�").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("���s�[�J���w�ж�").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("���s�[�J�H���ж�").GetComponent<Button>();

            groupMain = GameObject.Find("�e���D�n").GetComponent<CanvasGroup>();
            //�����s��G���U Enter �Ϊ̦b��L�a���I����
            //��J���.�����s��.�K�[��ť((��J��쪺��J�r��) => �x�s)
            inputFieldPlayerName.onEndEdit.AddListener((input) =>
            {
                namePlayer = input;
                PhotonNetwork.NickName = namePlayer;
            });

            inputFieldCreateRoomName.onEndEdit.AddListener((input) => nameCreateRoom = input);
            inputFieldJoinRoomName.onEndEdit.AddListener((input) => nameJoinRoom = input);

            btnCreateRoom.onClick.AddListener(CreateRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);
        }

        /// <summary>
        /// �s�u�ܥD��
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            groupMain.interactable = true;
            groupMain.blocksRaycasts = true;
            print("<color=yellow>�s�u�ܥD�����\�I</color>");
        }

        /// <summary>
        /// �Ыةж�
        /// </summary>
        private void CreateRoom()
        {
            //�ж��ﶵ�Ghttps://doc-api.photonengine.com/en/PUN/v2/class_photon_1_1_realtime_1_1_room_options.html
            RoomOptions ro = new RoomOptions();
            //�̤j�H�ƻP�i����
            ro.MaxPlayers = 20;
            ro.IsVisible = true;
            //Photon �s�u.�Ыةж�(�ж��W�١A�ж��ﶵ)
            PhotonNetwork.CreateRoom(nameCreateRoom, ro);
        }

        /// <summary>
        /// �[�J�ж�
        /// </summary>
        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(nameJoinRoom);
        }

        /// <summary>
        /// �[�J�H���ж�
        /// </summary>
        private void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        /// <summary>
        /// ���}�ж�
        /// </summary>
        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            groupRoom.alpha = 0;
            groupRoom.interactable = false;
            groupRoom.blocksRaycasts = false;
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print("<color=green>�Ыةж����\</color>");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("<color=green>�[�J�ж����\</color>");

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoomName.text = "�ж��W�١G" + PhotonNetwork.CurrentRoom.Name;
            textRoomPlayer.text = $"�ж��H�� { PhotonNetwork.CurrentRoom.PlayerCount } / { PhotonNetwork.CurrentRoom.MaxPlayers }";

        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            textRoomPlayer.text = $"�ж��H�� { PhotonNetwork.CurrentRoom.PlayerCount } / { PhotonNetwork.CurrentRoom.MaxPlayers }";
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            textRoomPlayer.text = $"�ж��H�� { PhotonNetwork.CurrentRoom.PlayerCount } / { PhotonNetwork.CurrentRoom.MaxPlayers }";
        }
    }

}

