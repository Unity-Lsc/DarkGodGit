/****************************************************
    文件：InfoWnd.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：2021/1/3 19:10:32
	功能：角色信息展示界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot 
{

    private Text txtTitle;
    private RawImage rawCharShow;
    private Transform MainContent;
    private Text txtExp;
    private Image imgExpBar;
    private Text txtPower;
    private Image imgPowerBar;
    private Text txtJob;
    private Text txtFight;
    private Text txtBlood;
    private Text txtDefence;
    private Text txtAttack;
    private Button btnDetail;
    private Button btnClose;

    private Transform tranDetail;
    private Transform tranGroup;
    private Text txtDetailBlood;
    private Text txtDetailAd;
    private Text txtDetailAp;
    private Text txtDetailAddef;
    private Text txtDetailApdef;
    private Text txtDetailDodge;
    private Text txtDetailPierce;
    private Text txtDetailCritical;
    private Button btnDetailClose;

    private Vector2 startPos;

    private void Awake() {
        txtTitle = transform.Find("imgCharBg/imgTitleBg/txtTitle").GetComponent<Text>();
        rawCharShow = transform.Find("imgCharBg/rawCharShow").GetComponent<RawImage>();
        MainContent = transform.Find("MainContent");
        txtExp = MainContent.Find("imgItem01/imgBarBg/txtExp").GetComponent<Text>();
        imgExpBar = MainContent.Find("imgItem01/imgBarBg/imgExpBar").GetComponent<Image>();
        txtPower = MainContent.Find("imgItem02/imgBarBg/txtPower").GetComponent<Text>();
        imgPowerBar = MainContent.Find("imgItem02/imgBarBg/imgPowerBar").GetComponent<Image>();
        txtJob = MainContent.Find("imgItem03/txtJob").GetComponent<Text>();
        txtFight = MainContent.Find("imgItem04/txtFight").GetComponent<Text>();
        txtBlood = MainContent.Find("imgItem05/txtBlood").GetComponent<Text>();
        txtDefence = MainContent.Find("imgItem06/txtDefence").GetComponent<Text>();
        txtAttack = MainContent.Find("imgItem07/txtAttack").GetComponent<Text>();
        btnDetail = MainContent.Find("btnDetail").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();

        tranDetail = transform.Find("imgDetail");
        tranGroup = tranDetail.Find("Group");
        txtDetailBlood = tranGroup.Find("imgPropItem1/txtBlood").GetComponent<Text>();
        txtDetailAd = tranGroup.Find("imgPropItem2/txtAd").GetComponent<Text>();
        txtDetailAp = tranGroup.Find("imgPropItem3/txtAp").GetComponent<Text>();
        txtDetailAddef = tranGroup.Find("imgPropItem4/txtAddef").GetComponent<Text>();
        txtDetailApdef = tranGroup.Find("imgPropItem5/txtApdef").GetComponent<Text>();
        txtDetailDodge = tranGroup.Find("imgPropItem6/txtDodge").GetComponent<Text>();
        txtDetailPierce = tranGroup.Find("imgPropItem7/txtPierce").GetComponent<Text>();
        txtDetailCritical = tranGroup.Find("imgPropItem8/txtCritical").GetComponent<Text>();
        btnDetailClose = tranDetail.Find("imgDetailBg/btnClose").GetComponent<Button>();
    }

    protected override void InitWnd() {
        base.InitWnd();
        RegisterEvent();
        RefreshUI();
    }

    private void RegisterEvent() {
        btnDetail.onClick.AddListener(OnClickDetailBtn);
        btnClose.onClick.AddListener(OnClickCloseBtn);
        btnDetailClose.onClick.AddListener(OnClickDetailCloseBtn);
        OnClickDown(rawCharShow.gameObject, (PointerEventData evt) => {
            startPos = evt.position;
            MainCitySystem.Instance.SetStartRotate();
        });
        OnDrag(rawCharShow.gameObject, (PointerEventData evt) => {
            float rotate = -(evt.position.x - startPos.x) * 0.4f;
            MainCitySystem.Instance.SetPlayerRotate(rotate);
        });
    }

    private void RefreshUI() {
        SetActive(tranDetail, false);
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtTitle, pd.name + " LV." + pd.lv);
        SetText(txtExp, pd.exp + "/" + PECommon.GetExpLimitByLevel(pd.lv));
        imgExpBar.fillAmount = pd.exp * 1.0f / PECommon.GetExpLimitByLevel(pd.lv);
        SetText(txtPower, pd.power + "/" + PECommon.GetPowerLimitByLevel(pd.lv));
        imgPowerBar.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimitByLevel(pd.lv);
        SetText(txtJob, "暗夜刺客");
        SetText(txtFight, PECommon.GetFightByProps(pd));
        SetText(txtBlood, pd.hp);
        SetText(txtDefence, pd.addef + pd.apdef);
        SetText(txtAttack, pd.ad + pd.ap);

        //详细属性
        SetText(txtDetailBlood, pd.hp);
        SetText(txtDetailAd, pd.ad);
        SetText(txtDetailAp, pd.ap);
        SetText(txtDetailAddef, pd.addef);
        SetText(txtDetailApdef, pd.apdef);
        SetText(txtDetailDodge, pd.dodge + "%");
        SetText(txtDetailPierce, pd.pierce + "%");
        SetText(txtDetailCritical, pd.critical + "%");
    }

    /// <summary>
    /// 点击详细属性按钮
    /// </summary>
    private void OnClickDetailBtn() {
        audioService.PlayUIAudio(Constants.AudioUIClickBtn);
        SetActive(tranDetail);
        
    }
    /// <summary>
    /// 点击信息页面关闭按钮
    /// </summary>
    private void OnClickCloseBtn() {
        audioService.PlayUIAudio(Constants.AudioUIClickBtn);
        MainCitySystem.Instance.CloseInfoWnd();
    }
    /// <summary>
    /// 点击详细属性页面关闭按钮
    /// </summary>
    private void OnClickDetailCloseBtn() {
        audioService.PlayUIAudio(Constants.AudioUIClickBtn);
        SetActive(tranDetail, false);
    }

}