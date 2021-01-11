/****************************************************
    文件：LoginWnd.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：登录注册界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot 
{

    private InputField iptAccount;
    private InputField iptPwd;
    private Button btnEnter;
    private Button btnNotice;

    private void Awake() {
        iptAccount = transform.Find("imgIptAccountBg/iptAccount").GetComponent<InputField>();
        iptPwd = transform.Find("imgIptPwdBg/iptPwd").GetComponent<InputField>();
        btnEnter = transform.Find("btnEnter").GetComponent<Button>();
        btnNotice = transform.Find("btnNotice").GetComponent<Button>();
    }

    protected override void InitWnd() {
        base.InitWnd();
        btnEnter.onClick.AddListener(OnClickEnterBtn);
        btnNotice.onClick.AddListener(OnClickNoticeBtn);
        if (PlayerPrefs.HasKey(Constants.Account) && PlayerPrefs.HasKey(Constants.Password)) {
            iptAccount.text = PlayerPrefs.GetString(Constants.Account);
            iptPwd.text = PlayerPrefs.GetString(Constants.Password);
        } else {
            iptAccount.text = "";
            iptPwd.text = "";
        }
    }

    /// <summary>
    /// 点击进入游戏按钮
    /// </summary>
    private void OnClickEnterBtn() {
        audioService.PlayUIAudio(Constants.AudioUILoginBtn);
        string acct = iptAccount.text;
        string pass = iptPwd.text;
        if(acct != "" && pass != "") {
            //更新本地存储的账号密码
            PlayerPrefs.SetString(Constants.Account, acct);
            PlayerPrefs.SetString(Constants.Password, pass);

            //TODO  发送网络消息,请求登录
            netService.SendMsg(new GameMsg {
                cmd = (int)CMD.ReqLogin,
                reqLogin = new ReqLogin {
                    account = acct,
                    password = pass
                }
            });
        } else {
            GameRoot.AddTips("账号或者密码格式不正确,请重新输入...");
        }
    }
    /// <summary>
    /// 点击公告按钮
    /// </summary>
    private void OnClickNoticeBtn() {
        audioService.PlayUIAudio(Constants.AudioUIClickBtn);
        GameRoot.AddTips("功能正在开发中...");
    }

}