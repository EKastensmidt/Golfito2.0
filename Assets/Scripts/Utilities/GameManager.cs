using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private List<TextMeshProUGUI> PlayerNames;
    [SerializeField] private List<TextMeshProUGUI> PlayerScores;
    public void SetManager(Ball ball)
    {
        ball.Manager = this;
    }

    private void Start()
    {
        
    }

    public void UpdatePlayerName()
    {
        pv.RPC("UpdateName", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void UpdateName()
    {
        switch (PhotonNetwork.PlayerList.Length)
        {
            case 2:
                PlayerNames[0].gameObject.SetActive(true);
                PlayerNames[0].text = PhotonNetwork.PlayerList[1].NickName + ":";
                break;
            case 3:
                PlayerNames[0].gameObject.SetActive(true);
                PlayerNames[1].gameObject.SetActive(true);

                PlayerNames[0].text = PhotonNetwork.PlayerList[1].NickName + ":";
                PlayerNames[1].text = PhotonNetwork.PlayerList[2].NickName + ":";
                break;
            case 4:
                PlayerNames[0].gameObject.SetActive(true);
                PlayerNames[1].gameObject.SetActive(true);
                PlayerNames[2].gameObject.SetActive(true);

                PlayerNames[0].text = PhotonNetwork.PlayerList[1].NickName + ":";
                PlayerNames[1].text = PhotonNetwork.PlayerList[2].NickName + ":";
                PlayerNames[2].text = PhotonNetwork.PlayerList[3].NickName + ":";
                break;
            case 5:
                PlayerNames[0].gameObject.SetActive(true);
                PlayerNames[1].gameObject.SetActive(true);
                PlayerNames[2].gameObject.SetActive(true);
                PlayerNames[3].gameObject.SetActive(true);

                PlayerNames[0].text = PhotonNetwork.PlayerList[1].NickName + ":";
                PlayerNames[1].text = PhotonNetwork.PlayerList[2].NickName + ":";
                PlayerNames[2].text = PhotonNetwork.PlayerList[3].NickName + ":";
                PlayerNames[3].text = PhotonNetwork.PlayerList[4].NickName + ":";
                break;
        }
    }


}
