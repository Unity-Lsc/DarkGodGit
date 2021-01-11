/****************************************************
	文件：NetService.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/25 21:34   	
	功能：网络服务
*****************************************************/

using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack {
    public ServerSession session;
    public GameMsg msg;
    public MsgPack(ServerSession session,GameMsg msg) {
        this.session = session;
        this.msg = msg;
    }
}

public class NetService {
    private static NetService instance = null;
    public static NetService Instance {
        get {
            if (instance == null) {
                instance = new NetService();
            }
            return instance;
        }
    }

    private Queue<MsgPack> msgQueue = new Queue<MsgPack>();

    public static readonly string obj = "lock";

    /// <summary>
    /// 网络服务初始化
    /// </summary>
    public void Init() {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(ServerCfg.serverIP, ServerCfg.serverPort);

        PECommon.Log("NetServer init done...");
    }

    public void AddMsgQueue(ServerSession session,GameMsg msg) {
        lock(obj) {
            msgQueue.Enqueue(new MsgPack(session,msg));
        }
    }

    public void Update() {
        if(msgQueue.Count > 0) {
            lock(obj) {
                MsgPack pack = msgQueue.Dequeue();
                HandOutMsg(pack);
            }
        }
    }

    /// <summary>
    /// 处理消息数据
    /// </summary>
    /// <param name="msg">消息</param>
    private void HandOutMsg(MsgPack pack) {
        switch ((CMD)pack.msg.cmd) {
            case CMD.ReqLogin:
                LoginSystem.Instance.ReqLogin(pack);
                break;
            case CMD.ReqRename:
                LoginSystem.Instance.ReqRename(pack);
                break;
        }
    }

}