/****************************************************
    文件：CreateWnd.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：创建角色界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class CreateWnd : WindowRoot 
{

    private InputField iptName;
    private Button btnRand;
    private Button btnEnter;

    private void Awake() {
        iptName = transform.Find("iptName").GetComponent<InputField>();
        btnRand = transform.Find("btnRand").GetComponent<Button>();
        btnEnter = transform.Find("btnEnter").GetComponent<Button>();
    }

    protected override void InitWnd() {
        base.InitWnd();
        btnRand.onClick.AddListener(OnClickRandBtn);
        btnEnter.onClick.AddListener(OnClickEnterBtn);
        iptName.text = resService.GetRandomName(false);
    }

    /// <summary>
    /// 点击随机名字按钮
    /// </summary>
    private void OnClickRandBtn() {
        audioService.PlayUIAudio(Constants.AudioUIClickBtn);
        iptName.text = resService.GetRandomName(false);
    }

    /// <summary>
    /// 点击进入游戏按钮
    /// </summary>
    private void OnClickEnterBtn() {
        audioService.PlayUIAudio(Constants.AudioUIClickBtn);
        if(iptName.text != "") {
            //TODO  发送名字数据到服务器 登录主城
            GameMsg msg = new GameMsg {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename {
                    name = iptName.text,
                }
            };
            netService.SendMsg(msg);
        }else {
            GameRoot.AddTips("请输入正确的名字...");
        }
    }

}