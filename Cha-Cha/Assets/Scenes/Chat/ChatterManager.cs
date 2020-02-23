using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.UI;

public class ChatterManager : ChatManagerBehavior
{
    public Transform chatContent;
    public GameObject chatMessage;
    private string username;

    protected override void NetworkStart()
    {
        base.NetworkStart();
        {
            base.NetworkStart();
            if (networkObject.IsServer)
            {
                username = "Server";
            }
            else
                username = "Client";
        }
    }

    public void WriteMessage(InputField sender)
    {
        if (!string.IsNullOrEmpty(sender.text) && sender.text.Trim().Length > 0)
        {
            sender.text = sender.text.Replace("\r", string.Empty).Replace("\n", string.Empty);
            networkObject.SendRpc(RPC_SEND_MESSAGE, Receivers.All, "Scadian", sender.text.Trim());
            sender.text = string.Empty;
            sender.ActivateInputField();
        }
    }
    public override void SendMessage(RpcArgs args)
    {
        string username = args.GetNext<string>();
        string message = args.GetNext<string>();
        /*
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(message));
        {
            //Message was empty, no display
            return;
        }
        */
        GameObject newMessage = Instantiate(chatMessage, chatContent);
        Text content = newMessage.GetComponent<Text>();
        content.text = string.Format(content.text, username, message);

    }
}
