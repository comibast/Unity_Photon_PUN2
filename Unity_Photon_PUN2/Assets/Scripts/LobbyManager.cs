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
        #region ���
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

        private void Awake()
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
            inputFieldPlayerName.onEndEdit.AddListener((input) => namePlayer = input);
            inputFieldCreateRoomName.onEndEdit.AddListener((input) => nameCreateRoom = input);
            inputFieldJoinRoomName.onEndEdit.AddListener((input) => nameJoinRoom = input);

            btnCreateRoom.onClick.AddListener(CreateRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);

            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// �s�u�ܥD����k
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
            print("<color=green>�Ыةж����\</color>");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("<color=green>�[�J�ж����\</color>");
        }


    }

}

