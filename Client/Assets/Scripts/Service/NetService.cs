/****************************************************
    文件：NetService.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：2020/12/25 23:35:4
	功能：网络服务
*****************************************************/

using PEProtocol;
using System.Collections.Generic;
using UnityEngine;

public class NetService : MonoBehaviour 
{
    public static NetService Instance = null;

    private PENet.PESocket<ClientSession, GameMsg> client = null;
    private Queue<GameMsg> msgQueue = new Queue<GameMsg>();//收到服务端消息队列
    private static readonly string obj = "lock";

    public void InitService() {
        Instance = this;
        client = new PENet.PESocket<ClientSession, GameMsg>();
        client.SetLog(true, (string msg, int lv) => {
            switch (lv) {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
        client.StartAsClient(ServerCfg.serverIP, ServerCfg.serverPort);

        PECommon.Log("Init Net Service...");

    }

    public void SendMsg(GameMsg msg) {
        if (client.session != null) {
            client.session.SendMsg(msg);
        }else {
            GameRoot.AddTips("服务器未连接...");
            InitService();
        }
    }

    public void AddMsgQueue(GameMsg msg) {
        lock(obj) {
            msgQueue.Enqueue(msg);
        }
    }

    /// <summary>
    /// 处理消息
    /// </summary>
    /// <param name="msg">需要处理的消息</param>
    private void ProcessMsg(GameMsg msg) {
        if(msg.err != (int)ErrorCode.None) {
            switch ((ErrorCode)msg.err) {
                case ErrorCode.AccountIsOnline:
                    GameRoot.AddTips("帐号已经在线...");
                    break;
                case ErrorCode.WrongPass:
                    GameRoot.AddTips("密码错误...");
                    break;
                case ErrorCode.UpdateDBError:
                    PECommon.Log("数据库更新异常", LogType.Error);
                    GameRoot.AddTips("网络不稳定,请检查网络连接...");
                    break;
            }
            return;
        }
        switch ((CMD)msg.cmd) {
            case CMD.RspLogin:
                LoginSystem.Instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSystem.Instance.RspRename(msg);
                break;
        }
    }

    private void Update() {
        if(msgQueue.Count > 0) {
            ProcessMsg(msgQueue.Dequeue());
        }
    }
}