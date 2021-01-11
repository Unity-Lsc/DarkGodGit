/****************************************************
	文件：PECommon.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/26 0:10   	
	功能：客户端服务端公用工具类
*****************************************************/

using PENet;
using PEProtocol;

public enum LogType {
    Log = 0,
    Warn = 1,
    Error = 2,
    Info = 3
}

public class PECommon {

    public static void Log(string msg,LogType type = LogType.Log) {
        LogLevel lv = (LogLevel)type;
        PETool.LogMsg(msg, lv);
    }

    //获取玩家的战斗力
    public static int GetFightByProps(PlayerData pd) {
        return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    //获取玩家的体力上限
    public static int GetPowerLimitByLevel(int lv) {
        return (lv - 1) * 15 + 150;
    }

    //获取玩家升级所需的经验值
    public static int GetExpLimitByLevel(int lv) {
        return lv * lv * 100;
    }

}