/****************************************************
    文件：GameRoot.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：游戏启动入口
*****************************************************/

using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance = null;
    public LoadingWnd loadingWnd;//登录注册界面
    public DynamicWnd dynamicWnd;//动态UI元素界面

    private PlayerData playerData = null;//玩家数据
    public PlayerData PlayerData {
        get {
            return playerData;
        }
    }

    private void Start() {
        Instance = this;
        DontDestroyOnLoad(this);
        PECommon.Log("Game Start...");
        Init();
    }

    private void Init() {
        //服务模块初始化
        NetService net = GetComponent<NetService>();
        net.InitService();
        ResService res = GetComponent<ResService>();
        res.InitService();
        AudioService audio = GetComponent<AudioService>();
        audio.InitService();

        //业务系统初始化
        LoginSystem login = GetComponent<LoginSystem>();
        login.InitSystem();
        MainCitySystem mainCity = GetComponent<MainCitySystem>();
        mainCity.InitSystem();
        

        //进入登录场景并加载相应UI
        login.EnterLogin();

        dynamicWnd.SetWndState();
    }

    /// <summary>
    /// 保存玩家数据
    /// </summary>
    public void SetPlayerData(RspLogin data) {
        playerData = data.playerData;
    }

    /// <summary>
    /// 设置玩家名字
    /// </summary>
    /// <param name="name"></param>
    public void SetPlayerName(string name) {
        playerData.name = name;
    }

    public static void AddTips(string tips) {
        Instance.dynamicWnd.AddTips(tips);
    }

}