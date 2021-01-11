/****************************************************
    文件：LoginSystem.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：登录注册业务系统
*****************************************************/

using PEProtocol;
using UnityEngine;

public class LoginSystem : SystemRoot 
{
    public static LoginSystem Instance = null;
    public LoginWnd loginWnd;//登录注册界面
    public CreateWnd createWnd;//角色创建界面

    public override void InitSystem() {
        base.InitSystem();
        Instance = this;
        PECommon.Log("Init Login System...");
    }

    /// <summary>
    /// 进入登录场景
    /// </summary>
    public void EnterLogin() {
        //异步地加载登录场景 加载进度条
        resService.AsyncLoadScene(Constants.SceneLogin, () => {
            loginWnd.SetWndState();
            audioService.PlayBgMusic(Constants.AudioBgLogin);
            GameRoot.AddTips("登录注册场景加载完毕...");
        });
        //加载完成后再打开注册登录界面
        //TODO
    }

    /// <summary>
    /// 登录消息反馈
    /// </summary>
    /// <param name="msg">反馈的消息</param>
    public void RspLogin(GameMsg msg) {
        GameRoot.AddTips("登录成功...");
        GameRoot.Instance.SetPlayerData(msg.rspLogin);
        if (msg.rspLogin.playerData.name == "") {
            //打开角色创建界面
            createWnd.SetWndState();
        }else {
            //进入主城 TODO
            MainCitySystem.Instance.EnterMainCity();
        }
        //关闭登录界面
        loginWnd.SetWndState(false);
    }

    /// <summary>
    /// 重命名消息反馈
    /// </summary>
    /// <param name="msg">反馈的消息</param>
    public void RspRename(GameMsg msg) {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);
        //跳转场景进入主城
        //打开主城的界面
        PECommon.Log("创建角色成功,进入主城...");
        MainCitySystem.Instance.EnterMainCity();
        //关闭创建角色界面
        createWnd.SetWndState(false);
    }

}