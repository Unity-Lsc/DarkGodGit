/****************************************************
	文件：Class1.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/25 21:41   	
	功能：网络通信协议(服务端客户端通用)
*****************************************************/

using System;
using PENet;

namespace PEProtocol {

    public enum CMD {
        None = 0,
        //登录相关 101
        ReqLogin = 101,//登录请求
        RspLogin = 102,//登录回应
        ReqRename = 103,//重命名请求
        RspRename = 104,//重命名回应
    }

    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCode {
        None = 0,//没有错误
        AccountIsOnline,//账号已经在线
        WrongPass,//密码错误
        NameIsExist,//名字已经存在
        UpdateDBError,//更新数据库出错
    }

    #region 登录相关

    [Serializable]
    public class GameMsg : PEMsg {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqRename reqRename;
        public RspRename rspRename;
    }

    //登录请求
    [Serializable]
    public class ReqLogin {
        public string account;
        public string password;
    }

    //登录回应
    [Serializable]
    public class RspLogin {
        public PlayerData playerData;
    }

    //重命名请求
    [Serializable]
    public class ReqRename {
        public string name;
    }

    //重命名回应
    [Serializable]
    public class RspRename {
        public string name;
    }

    //玩家数据
    [Serializable]
    public class PlayerData {
        public int id;//玩家ID
        public string name;//玩家名字
        public int lv;//玩家等级
        public int exp;//玩家经验值
        public int power;//玩家战力
        public int coin;//玩家金币数
        public int diamond;//玩家钻石数

        public int hp;//血量
        public int ad;//物理攻击
        public int ap;//魔法攻击
        public int addef;//物理防御
        public int apdef;//魔法防御
        public int dodge;//闪避概率
        public int pierce;//穿透比率
        public int critical;//暴击概率

        public int guideid;
    }

    #endregion



    public class ServerCfg {
        public const string serverIP = "127.0.0.1";
        public const int serverPort = 17666;
    }

}
