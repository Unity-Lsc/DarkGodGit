/****************************************************
	文件：CacheService.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/26 15:27   	
	功能：缓存层
*****************************************************/

using PEProtocol;
using System.Collections.Generic;

public class CacheService {
    private static CacheService instance = null;
    public static CacheService Instance {
        get {
            if (instance == null) {
                instance = new CacheService();
            }
            return instance;
        }
    }

    private Dictionary<string, ServerSession> onlineSessionDict = new Dictionary<string, ServerSession>();//在线玩家缓存列表
    private Dictionary<ServerSession, PlayerData> onlinePlayerDataDict = new Dictionary<ServerSession, PlayerData>();//在线玩家数据缓存列表
    private DBMgr dbMgr = null;

    public void Init() {
        dbMgr = DBMgr.Instance;
        PECommon.Log("CacheService init done...");
    }

    /// <summary>
    /// 判断玩家是否在线
    /// </summary>
    /// <param name="account">玩家账号</param>
    public bool IsAccountOnline(string account) {
        return onlineSessionDict.ContainsKey(account);
    }

    /// <summary>
    /// 从数据库获取玩家数据
    /// </summary>
    /// <param name="account">账号</param>
    /// <param name="pwd">密码</param>
    /// <returns>玩家账号数据</returns>
    public PlayerData GetPlayerData(string account,string pwd) {
        return dbMgr.QueryPlayerData(account, pwd);
    }

    /// <summary>
    /// 账号上线 缓存数据
    /// </summary>
    /// <param name="account">玩家账号</param>
    /// <param name="session">网络会话</param>
    /// <param name="playerData">玩家数据</param>
    public void AccountOnline(string account,ServerSession session,PlayerData playerData) {
        onlineSessionDict.Add(account, session);
        onlinePlayerDataDict.Add(session, playerData);
    }

    /// <summary>
    /// 查询是否已经存在该名字
    /// </summary>
    /// <param name="name">要校验的名字</param>
    public bool IsNameExist(string name) {
        return dbMgr.QueryNameData(name);
    }

    /// <summary>
    /// 通过网络会话 获取该玩家的数据
    /// </summary>
    /// <param name="session"></param>
    public PlayerData GetPlayerDataBySession(ServerSession session) {
        if(onlinePlayerDataDict.TryGetValue(session,out PlayerData playerData)) {
            return playerData;
        }
        return null;
    }

    /// <summary>
    /// 更新数据库中的玩家数据
    /// </summary>
    /// <param name="id">用户id</param>
    /// <param name="playerData">需要更新的用户数据</param>
    /// <returns></returns>
    public bool UpdatePlayerData(int id,PlayerData playerData) {
        return dbMgr.UpdatePlayerData(id, playerData);
    }

    /// <summary>
    /// 玩家下线 清理缓存数据
    /// </summary>
    /// <param name="session"></param>
    public void AccountOffline(ServerSession session) {
        foreach (var item in onlineSessionDict) {
            if(item.Value == session) {
                onlineSessionDict.Remove(item.Key);
                break;
            }
        }
        bool succ = onlinePlayerDataDict.Remove(session);
        PECommon.Log("Offline Session ID:" + session.SessionID + " result:" + succ);
    }

}
