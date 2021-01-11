/****************************************************
	文件：ServerSession.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/25 23:14   	
	功能：网络会话连接
*****************************************************/

using PEProtocol;
using PENet;

public class ServerSession : PESession<GameMsg> {

    public int SessionID = 0;

    protected override void OnConnected() {
        SessionID = ServerRoot.Instance.GetSessionID();
        PECommon.Log("Session ID:" + SessionID + "  Client connect...");
    }

    protected override void OnReciveMsg(GameMsg msg) {
        PECommon.Log("Receive SessionID:" + SessionID + " client msg cmd:" + (CMD)msg.cmd);
        NetService.Instance.AddMsgQueue(this,msg);
    }

    protected override void OnDisConnected() {
        LoginSystem.Instance.ClearOfflineData(this);
        PECommon.Log("Session ID:" + SessionID + "  Client offline...");
    }

}
