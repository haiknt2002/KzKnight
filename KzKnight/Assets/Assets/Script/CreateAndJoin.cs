using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField createField;
    [SerializeField] InputField joinField;
    [SerializeField] Text label;
    // Biến lưu danh sách room được cập nhật từ Photon
    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();
    // Start is called before the first frame update
    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;  // Giới hạn số người trong room là 2
            PhotonNetwork.CreateRoom(createField.text, roomOptions, TypedLobby.Default);
        }
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinField.text);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        label.text = ("Failed to join room: " + message);
        // Hiển thị thông báo cho người dùng hoặc xử lý logic khác khi không thể join room
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Đã vào room, số lượng người chơi: " + PhotonNetwork.PlayerList.Length);

        PhotonNetwork.AutomaticallySyncScene = true;

        // Chỉ MasterClient mới gọi LoadLevel
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("LobbyScene");
        }
    }
    // Callback được gọi khi danh sách room được cập nhật
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomList = roomList;
        check();
    }
    public void check()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
        Debug.Log("Cập nhật danh sách room: " + cachedRoomList.Count + " room hiện có.");
        foreach (RoomInfo room in cachedRoomList)
        {
            Debug.Log("Room Name: " + room.Name + ", Số người: " + room.PlayerCount + "/" + room.MaxPlayers);
        }
    }
}
