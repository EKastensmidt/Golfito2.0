using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Chat : MonoBehaviourPun
{
    public TextMeshProUGUI content;
    public TMP_InputField inputField;

    public void SendMessage()
    {
        var message = inputField.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        photonView.RPC("GetMessage", RpcTarget.All, PhotonNetwork.NickName, message);
    }

    [PunRPC]
    public void GetMessage(string nameClient,string message)
    {
        string color;
        if (PhotonNetwork.NickName == nameClient)
        {
            color = "<color=red>";

        }
        else
        {
            color = "<color=blue>";
        }

        content.text += color + nameClient + ": " + "</color>" + message + "\n";
    }
}
