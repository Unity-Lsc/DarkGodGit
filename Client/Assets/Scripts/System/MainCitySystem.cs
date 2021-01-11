/****************************************************
    文件：MainCitySystem.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：2020/12/29 15:27:13
	功能：主城业务系统
*****************************************************/

using UnityEngine;
using UnityEngine.AI;

public class MainCitySystem : SystemRoot 
{
    public static MainCitySystem Instance = null;
    public MainCityWnd mainCityWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;

    private PlayerController playerController;
    private NavMeshAgent playerNav;
    private Transform tranCharShowCamera;//角色展示相机
    private float startRotateY = 0;//角色的初识Y旋转
    private GuideCfg curTaskData;//当前任务数据
    private bool isNavGuide = false;//是否在任务导航中
    private Transform[] npcTranArr = new Transform[4];

    public override void InitSystem() {
        base.InitSystem();
        Instance = this;
        PECommon.Log("Init MainCity System...");
    }

    /// <summary>
    /// 进入主城
    /// </summary>
    public void EnterMainCity() {
        MapCfg mapData = resService.GetMapCfgData(Constants.SceneMainCityID);
        resService.AsyncLoadScene(mapData.sceneName, () => {
            PECommon.Log("Enter MainCity...");
            //加载游戏主角
            LoadPlayer(mapData);
            //打开主城场景UI
            mainCityWnd.SetWndState();
            //播放主城背景音乐
            audioService.PlayBgMusic(Constants.AudioBgMainCity);
            //设置人物展示相机
            if(tranCharShowCamera != null) {
                tranCharShowCamera.gameObject.SetActive(false);
            }
            //存储NPC的位置信息
            SetMainCityMapInfo();
        });
    }
    /// <summary>
    /// 加载角色
    /// </summary>
    /// <param name="mapData">地图数据</param>
    private void LoadPlayer(MapCfg mapData) {
        GameObject player = resService.LoadPrefab(PathDefine.AssissnPlayerPrefabPath,true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one * 1.5f;
        //初始化相机位置
        Transform mainCamTran = Camera.main.transform;
        mainCamTran.position = mapData.mainCamPos;
        mainCamTran.localEulerAngles = mapData.mainCamRote;

        playerController = player.GetComponent<PlayerController>();
        playerNav = player.GetComponent<NavMeshAgent>();
        playerController.Init();

    }

    private void SetMainCityMapInfo() {
        Transform npcRoot = GameObject.FindGameObjectWithTag("NPC").transform;
        for (int i = 0; i < npcRoot.childCount; i++) {
            npcTranArr[i] = npcRoot.GetChild(i);
        }
    }

    public void SetMoveDir(Vector2 dir) {
        StopNavTask();
        if (dir == Vector2.zero) {
            playerController.SetBlend(Constants.BlendIdle);
        } else {
            playerController.SetBlend(Constants.BlendWalk);
        }
        playerController.Dir = dir;
    }

    /// <summary>
    /// 打开角色信息界面
    /// </summary>
    public void OpenInfoWnd() {
        StopNavTask();
        if (tranCharShowCamera == null) {
            tranCharShowCamera = GameObject.FindGameObjectWithTag("PlayerCamera").transform;
        }
        //设置人物展示相机的相对位置
        tranCharShowCamera.localPosition = playerController.transform.localPosition + playerController.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        tranCharShowCamera.localEulerAngles = new Vector3(0, 180 + playerController.transform.localEulerAngles.y, 0);
        tranCharShowCamera.gameObject.SetActive(true);
        infoWnd.SetWndState();
    }
    /// <summary>
    /// 关闭角色信息界面
    /// </summary>
    public void CloseInfoWnd() {
        if(tranCharShowCamera != null) {
            tranCharShowCamera.gameObject.SetActive(false);
        }
        infoWnd.SetWndState(false);
    }

    public void SetStartRotate() {
        startRotateY = playerController.transform.localEulerAngles.y;
    }

    public void SetPlayerRotate(float rotate) {
        playerController.transform.localEulerAngles = new Vector3(0, startRotateY + rotate, 0);
    }
    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="guideCfg">任务数据</param>
    public void RunTask(GuideCfg guideCfg) {
        if(guideCfg != null) {
            curTaskData = guideCfg;
            playerNav.enabled = true;
            if(curTaskData.npcID != -1) {
                float dis = Vector3.Distance(playerController.transform.position, npcTranArr[curTaskData.npcID].position);
                if(dis < 0.5f) {
                    StopNavTask();
                    OpenGuideWnd();
                } else {
                    playerNav.enabled = true;
                    playerNav.speed = Constants.PlayerMoveSpeed;
                    playerNav.SetDestination(npcTranArr[curTaskData.npcID].position);
                    playerController.SetBlend(Constants.BlendWalk);
                    isNavGuide = true;
                }
            }else {
                OpenGuideWnd();
            }
        }
    }

    /// <summary>
    /// 打开任务引导界面
    /// </summary>
    private void OpenGuideWnd() {
        guideWnd.SetWndState();
    }

    private void StopNavTask() {
        if(isNavGuide) {
            isNavGuide = false;
            playerNav.isStopped = true;
            playerNav.enabled = false;
            playerController.SetBlend(Constants.BlendIdle);
        }
    }

    private void Update() {
        if(isNavGuide) {
            playerController.SetCameraFollow();
            SetNavState();
        }
    }

    private void SetNavState() {
        float dis = Vector3.Distance(playerController.transform.position, npcTranArr[curTaskData.npcID].position);
        if (dis < 0.5f) {
            StopNavTask();
            OpenGuideWnd();
        }
    }

    public GuideCfg GetCurTaskData() {
        return curTaskData;
    }

}