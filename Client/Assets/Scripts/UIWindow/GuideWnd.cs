/****************************************************
    文件：GuideWnd.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：2021/1/7 20:50:24
	功能：引导对话界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot 
{

    private Transform Bottom;
    private Text txtTalk;
    private Text txtName;
    private Image imgIcon;
    private Button btnNext;

    private PlayerData pd;
    private GuideCfg curTaskData;
    private string[] dialogArr;
    private int index;

    private void Awake() {
        Bottom = transform.Find("Bottom");
        txtTalk = Bottom.Find("imgTalkBg/txtTalk").GetComponent<Text>();
        txtName = Bottom.Find("imgTalkBg/txtName").GetComponent<Text>();
        imgIcon = Bottom.Find("imgIcon").GetComponent<Image>();
        btnNext = Bottom.Find("btnNext").GetComponent<Button>();
        btnNext.onClick.AddListener(OnClickNextBtn);
    }

    protected override void InitWnd() {
        base.InitWnd();
        curTaskData = MainCitySystem.Instance.GetCurTaskData();
        dialogArr = curTaskData.dilogArr.Split('#');
        index = 1;
        pd = GameRoot.Instance.PlayerData;
        SetTalk();
    }

    private void SetTalk() {
        string[] talkArr = dialogArr[index].Split('|');
        string iconPath = "ResImage/";
        string name;
        if (talkArr[0] == "0") {
            iconPath += "assassin";
            name = pd.name;
        }else {
            int id = curTaskData.npcID;
            if (id >= 0) {
                iconPath += ("npc" + id);
            } else {
                iconPath += "npcguide";
            }
            name = GetNPCName(id);
        }
        SetText(txtName, name);
        SetText(txtTalk, talkArr[1].Replace("$name",pd.name));
        SetSprite(imgIcon, iconPath);
        imgIcon.SetNativeSize();
    }

    private string GetNPCName(int npcID) {
        switch (npcID) {
            case 0: return "智者";
            case 1: return "将军";
            case 2: return "工匠";
            case 3: return "商人";
            default: return "萍萍";
        }
    }

    private void OnClickNextBtn() {
        audioService.PlayUIAudio(Constants.AudioUIClickBtn);
        index += 1;
        if(index == dialogArr.Length) {
            SetWndState(false);
            return;
        }
        SetTalk();
    }

}