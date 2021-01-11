/****************************************************
    文件：MainCityWnd.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：2020/12/29 15:3:50
	功能：主城界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCityWnd : WindowRoot 
{
    private Transform LeftTop;
    private Text txtFight;
    private Button btnUpFight;
    private Image imgPowerBar;
    private Text txtPower;
    private Button btnBuyPower;
    private Button btnHead;
    private Text txtLevel;

    private Transform RightTop;
    private Button btnGuide;
    private Button btnCharge;
    private Button btnVIP;
    private Text txtVIP;

    private Transform RightBottom;
    private Button btnMenu;
    private Transform MenuRoot;
    private Button btnTask;
    private Button btnArena;
    private Button btnStrong;
    private Button btnMKCoin;

    private Transform Center;
    private Text txtName;

    private Transform CenterBottom;
    private Text txtExpPercent;
    private Transform tranExpBg;
    private Transform tranItemLst;

    private Transform LeftBottom;
    private Image imgTouchArea;
    private Image imgDirBg;
    private Image imgDirPoint;

    private Animation aniMenu;
    private bool isMenuOpen = true;//菜单是否处于打开状态
    private Vector2 startPos = Vector2.zero;//摇杆滑块的开始位置
    private float pointDistance;//摇杆滑块的最大移动距离
    private GuideCfg curGuideCfg;//当前任务的数据

    private void Awake() {
        LeftTop = transform.Find("LeftTop");
        txtFight = LeftTop.Find("imgFightBg/txtFight").GetComponent<Text>();
        btnUpFight = LeftTop.Find("imgFightBg/btnUpFight").GetComponent<Button>();
        imgPowerBar = LeftTop.Find("imgPowerBarBg/imgPowerBar").GetComponent<Image>();
        txtPower = LeftTop.Find("imgPowerBarBg/txtPower").GetComponent<Text>();
        btnBuyPower = LeftTop.Find("imgPowerBarBg/btnBuyPower").GetComponent<Button>();
        btnHead = LeftTop.Find("btnHead").GetComponent<Button>();
        txtLevel = LeftTop.Find("imgLevelBg/txtLevel").GetComponent<Text>();

        RightTop = transform.Find("RightTop");
        btnGuide = RightTop.Find("btnGuide").GetComponent<Button>();
        btnCharge = RightTop.Find("btnCharge").GetComponent<Button>();
        btnVIP = RightTop.Find("btnVIP").GetComponent<Button>();
        txtVIP = btnVIP.transform.Find("txtVIP").GetComponent<Text>();

        RightBottom = transform.Find("RightBottom");
        MenuRoot = RightBottom.Find("MenuRoot");
        btnMenu = MenuRoot.Find("btnMenu").GetComponent<Button>();
        btnTask = MenuRoot.Find("btnTask").GetComponent<Button>();
        btnArena = MenuRoot.Find("btnArena").GetComponent<Button>();
        btnStrong = MenuRoot.Find("btnStrong").GetComponent<Button>();
        btnMKCoin = MenuRoot.Find("btnMKCoin").GetComponent<Button>();

        Center = transform.Find("Center");
        txtName = Center.Find("txtName").GetComponent<Text>();

        CenterBottom = transform.Find("CenterBottom");
        txtExpPercent = CenterBottom.Find("txtExpPercent").GetComponent<Text>();
        tranExpBg = CenterBottom.Find("imgExpBg");
        tranItemLst = tranExpBg.Find("itemLst");

        LeftBottom = transform.Find("LeftBottom");
        imgTouchArea = LeftBottom.Find("imgTouchArea").GetComponent<Image>();
        imgDirBg = imgTouchArea.transform.Find("imgDirBg").GetComponent<Image>();
        imgDirPoint = imgDirBg.transform.Find("imgDirPoint").GetComponent<Image>();

        aniMenu = MenuRoot.GetComponent<Animation>();
    }


    protected override void InitWnd() {
        base.InitWnd();
        pointDistance = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOperaDiatance;
        RegisterClickEvents();
        RefreshUI();
        SetActive(imgDirBg, false);
    }

    private void RefreshUI() {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtFight, PECommon.GetFightByProps(pd));
        int powerLimit = PECommon.GetPowerLimitByLevel(pd.lv);
        SetText(txtPower, "体力:" + pd.power + "/" + powerLimit);
        SetText(txtLevel, pd.lv);
        imgPowerBar.fillAmount = pd.power * 1.0f / powerLimit;
        SetText(txtName, pd.name);
        //经验条自适应
        GridLayoutGroup gridGroup = tranItemLst.GetComponent<GridLayoutGroup>();
        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalRate;
        float cellWidth = (screenWidth - 180) / 10;
        gridGroup.cellSize = new Vector2(cellWidth, 15);
        //经验条显示
        int expPercentVal = (int)(pd.exp * 1.0f / PECommon.GetExpLimitByLevel(pd.lv) * 100);
        SetText(txtExpPercent, expPercentVal + "%");
        int index = expPercentVal / 10;
        for (int i = 0; i < tranItemLst.childCount; i++) {
            Image img = tranItemLst.GetChild(i).GetComponent<Image>();
            if(i < index) {
                img.fillAmount = 1;
            }else if(i == index) {
                img.fillAmount = expPercentVal % 10 * 1.0f / 10;
            }else {
                img.fillAmount = 0;
            }
        }
        //显示当前任务的图标
        curGuideCfg = resService.GetGuideCfgData(pd.guideid);
        int npcID = curGuideCfg == null ? -1 : curGuideCfg.npcID;
        SetGuideBtnIcon(npcID);
    }

    private void SetGuideBtnIcon(int npcID) {
        Image img = btnGuide.GetComponent<Image>();
        string path = npcID == -1 ? "ResImage/task" : "ResImage/guideIcon" + npcID;
        SetSprite(img, path);
    }

    /// <summary>
    /// 注册点击事件
    /// </summary>
    private void RegisterClickEvents() {
        btnUpFight.onClick.AddListener(OnClickUpFightBtn);
        btnBuyPower.onClick.AddListener(OnClickBuyPowerBtn);
        btnHead.onClick.AddListener(OnClickHeadBtn);
        btnGuide.onClick.AddListener(OnClickGuideBtn);
        btnCharge.onClick.AddListener(OnClickChargeBtn);
        btnVIP.onClick.AddListener(OnClickVIPBtn);
        btnMenu.onClick.AddListener(OnClickMenuBtn);
        btnTask.onClick.AddListener(OnClickTaskBtn);
        btnArena.onClick.AddListener(OnClickArenaBtn);
        btnStrong.onClick.AddListener(OnClickStrongBtn);
        btnMKCoin.onClick.AddListener(OnClickMKCoinBtn);
        //点击操作区域
        OnClickDown(imgTouchArea.gameObject, (PointerEventData evt) => {
            SetActive(imgDirBg);
            startPos = evt.position;
            imgDirBg.transform.position = evt.position;
        });
        //从操作区域抬起
        OnClickUp(imgTouchArea.gameObject, (PointerEventData evt) => {
            SetActive(imgDirBg, false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            MainCitySystem.Instance.SetMoveDir(Vector2.zero);
        });
        //拖拽操作区域
        OnDrag(imgTouchArea.gameObject, (PointerEventData evt) => {
            Vector2 dir = evt.position - startPos;
            if(dir.magnitude > pointDistance) {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDistance);
                imgDirPoint.transform.position = startPos + clampDir;
            }else {
                imgDirPoint.transform.position = evt.position;
            }
            MainCitySystem.Instance.SetMoveDir(dir.normalized);
        });
    }

    #region 点击函数
    
    //点击提升战力
    private void OnClickUpFightBtn() {

    }
    //点击购买体力
    private void OnClickBuyPowerBtn() {

    }
    //点击头像
    private void OnClickHeadBtn() {
        audioService.PlayUIAudio(Constants.AudioUIOpenPage);
        MainCitySystem.Instance.OpenInfoWnd();
    }
    //点击自动任务
    private void OnClickGuideBtn() {
        audioService.PlayUIAudio(Constants.AudioUIClickBtn);
        if(curGuideCfg != null) {
            MainCitySystem.Instance.RunTask(curGuideCfg);
        } else {
            GameRoot.AddTips("更多引导任务,正在开发中...");
        }
    }
    //点击充值
    private void OnClickChargeBtn() {

    }
    //点击VIP
    private void OnClickVIPBtn() {

    }
    //点击菜单
    private void OnClickMenuBtn() {
        audioService.PlayUIAudio(Constants.AudioUIExtenBtn);
        isMenuOpen = !isMenuOpen;
        AnimationClip clip = isMenuOpen ? aniMenu.GetClip("OpenMainCityMenu") : aniMenu.GetClip("CloseMainCityMenu");
        aniMenu.Play(clip.name);
    }
    //点击任务
    private void OnClickTaskBtn() {

    }
    //点击副本
    private void OnClickArenaBtn() {

    }
    //点击强化
    private void OnClickStrongBtn() {

    }
    //点击铸造
    private void OnClickMKCoinBtn() {

    }

    #endregion

}