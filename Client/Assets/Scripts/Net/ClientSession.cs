/****************************************************
    文件：ClientSession.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：2020/12/25 23:42:38
	功能：客户端网络会话
*****************************************************/

using PEProtocol;

public class ClientSession : PENet.PESession<GameMsg> {

    protected override void OnConnected() {
        GameRoot.AddTips("服务器连接成功...");
        PECommon.Log("Connect to server...");
    }

    protected override void OnReciveMsg(GameMsg msg) {
        PECommon.Log("Receive server msg cmd:" + (CMD)msg.cmd);
        NetService.Instance.AddMsgQueue(msg);
    }

    protected override void OnDisConnected() {
        GameRoot.AddTips("服务器断开连接...");
        PECommon.Log("Disconnect to server...");
    }

}