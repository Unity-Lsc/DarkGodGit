/****************************************************
	文件：LoginSystem.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/25 21:37   	
	功能：登录业务系统
*****************************************************/

using PEProtocol;

public class LoginSystem {

    private static LoginSystem instance = null;
    public static LoginSystem Instance {
        get {
            if (instance == null) {
                instance = new LoginSystem();
            }
            return instance;
        }
    }

    private CacheService cacheService = null;

    public void Init() {
        cacheService = CacheService.Instance;
        PECommon.Log("LoginSystem init done...");
    }

    public void ReqLogin(MsgPack pack) {
        //当前账号是否上线
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.RspLogin,
        };
        ReqLogin data = pack.msg.reqLogin;
        //已上线:返回错误信息
        if(cacheService.IsAccountOnline(data.account)) {
            msg.err = (int)ErrorCode.AccountIsOnline;
        }
        //未上线:
        else {
            //账号是否存在
            PlayerData pData = cacheService.GetPlayerData(data.account, data.password);
            if(pData == null) {
                //密码错误
                msg.err = (int)ErrorCode.WrongPass;
            }else {
                msg.rspLogin = new RspLogin {
                    playerData = pData
                };
                //缓存账号数据
                cacheService.AccountOnline(data.account, pack.session, pData);
            }
            
        }
        //回应客户端
        pack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack) {
        ReqRename reqRename = pack.msg.reqRename;
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.RspRename,
        };
        //名字是否已经存在
        if(cacheService.IsNameExist(reqRename.name)) {
            //存在,返回错误码
            msg.err = (int)ErrorCode.NameIsExist;
        } else {
            //不存在,更新缓存以及数据库 再返回给客户端
            PlayerData playerData = cacheService.GetPlayerDataBySession(pack.session);
            if (playerData != null) playerData.name = reqRename.name;

            if(!cacheService.UpdatePlayerData(playerData.id,playerData)) {
                msg.err = (int)ErrorCode.UpdateDBError;
            } else {
                msg.rspRename = new RspRename {
                    name = reqRename.name,
                };
            }
        }
        pack.session.SendMsg(msg);
    }

    /// <summary>
    /// 清理下线玩家数据
    /// </summary>
    /// <param name="session"></param>
    public void ClearOfflineData(ServerSession session) {
        cacheService.AccountOffline(session);
    }

}
