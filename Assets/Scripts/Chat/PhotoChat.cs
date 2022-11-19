using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;

public class PhotoChat : MonoBehaviour, IChatClientListener
{
    public TextMeshProUGUI content;
    public TMP_InputField inputField;
    ChatClient chatClient;

    string channel;
    private void Start()
    {
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion,
            new AuthenticationValues(PhotonNetwork.NickName));
    }
    private void Update()
    {
        chatClient.Service();
    }
    void IChatClientListener.DebugReturn(DebugLevel level, string message)
    {

    }

    void IChatClientListener.OnChatStateChange(ChatState state)
    {
    }

    void IChatClientListener.OnConnected()
    {
        content.text += "SI SE CONECTO" + "\n";
        channel = PhotonNetwork.CurrentRoom.Name;
        chatClient.Subscribe(channel);

    }

    void IChatClientListener.OnDisconnected()
    {
        content.text += "NO SE CONECTO" + "\n";

    }

    void IChatClientListener.OnGetMessages(string channelName, string[] senders, object[] messages)
    {
    }

    void IChatClientListener.OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    void IChatClientListener.OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    void IChatClientListener.OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            content.text += "Subscribe to" + channels[i] + "\n";

        }
    }

    void IChatClientListener.OnUnsubscribed(string[] channels)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            content.text += "UnSubscribe to" + channels[i] + "\n";

        }
    }

    void IChatClientListener.OnUserSubscribed(string channel, string user)
    {
    }

    void IChatClientListener.OnUserUnsubscribed(string channel, string user)
    {
    }

    //xd


}
