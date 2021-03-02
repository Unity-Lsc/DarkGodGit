/****************************************************
    文件：ResService.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：资源加载服务
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResService : MonoBehaviour 
{

    public static ResService Instance = null;

    private Action processCallback = null;

    public void InitService() {
        Instance = this;
        InitRandNameCfg(PathDefine.RandNameCfgPath);
        InitMapCfg(PathDefine.MapCfgPath);
        InitGuideCfg(PathDefine.GuideCfgPath);
        PECommon.Log("Init Res Service...");
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="loadedCallback">加载完成的回调</param>
    public void AsyncLoadScene(string sceneName, Action loadedCallback = null) {
        GameRoot.Instance.loadingWnd.SetWndState();
        AsyncOperation opera = SceneManager.LoadSceneAsync(sceneName);
        processCallback = () => {
            float value = opera.progress;
            GameRoot.Instance.loadingWnd.SetProgress(value);
            if (value >= 1) {
                if(loadedCallback != null) {
                    loadedCallback();
                }
                processCallback = null;
                opera = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };
    }

    private void Update() {
        if(processCallback != null) {
            processCallback();
        }
    }

    //声音资源缓存池
    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();
    /// <summary>
    /// 加载声音资源
    /// </summary>
    /// <param name="name">声音资源名字</param>
    /// <param name="isCache">是否需要缓存</param>
    public AudioClip LoadAudio(string name,bool isCache = false) {
        AudioClip clip = null;
        if(!audioDict.TryGetValue(name,out clip)) {
            clip = Resources.Load<AudioClip>("ResAudio/" + name);
            if(isCache) {
                audioDict.Add(name, clip);
            }
        }
        return clip;
    }
    //预制体缓存池
    private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();
    /// <summary>
    /// 加载预制体资源
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="isCache">是否需要缓存</param>
    public GameObject LoadPrefab(string path,bool isCache = false) {
        GameObject prefab = null;
        if(!prefabDict.TryGetValue(path,out prefab)) {
            prefab = Resources.Load<GameObject>(path);
            if(isCache) {
                prefabDict.Add(path, prefab);
            }
        }
        GameObject go = null;
        if(prefab != null) {
            go = Instantiate(prefab);
        }
        return go;
    }

    //精灵缓存池
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
    /// <summary>
    /// 加载精灵资源
    /// </summary>
    /// <param name="path">加载路径</param>
    /// <param name="isCache">是否需要缓存</param>
    public Sprite LoadSprite(string path,bool isCache = false) {
        Sprite sp = null;
        if(!spriteDict.TryGetValue(path,out sp)) {
            sp = Resources.Load<Sprite>(path);
            if(isCache) {
                spriteDict.Add(path, sp);
            }
        }
        return sp;
    }

    #region InitCfgs

    #region 随机名字

    private List<string> surnameLst = new List<string>();//姓氏
    private List<string> manLst = new List<string>();//男名字
    private List<String> womanLst = new List<string>();//女名字

    /// <summary>
    /// 初始化随机名字配置表
    /// </summary>
    private void InitRandNameCfg(string path) {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeLst.Count; i++) {
                XmlElement element = nodeLst[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                foreach (XmlElement item in nodeLst[i].ChildNodes) {
                    switch (item.Name) {
                        case "surname":
                            surnameLst.Add(item.InnerText);
                            break;
                        case "man":
                            manLst.Add(item.InnerText);
                            break;
                        case "woman":
                            womanLst.Add(item.InnerText);
                            break;
                    }
                }
            }
        } else {
            PECommon.Log("xml file:" + PathDefine.RandNameCfgPath + "  not exist...", LogType.Error);
        }
    }

    /// <summary>
    /// 获取一个随机名字
    /// </summary>
    /// <param name="isMan">是否是男性</param>
    public string GetRandomName(bool isMan = true) {
        System.Random rand = new System.Random();
        string randName = surnameLst[PETools.GetRandNum(0, surnameLst.Count - 1, rand)];
        if (isMan) {
            randName += manLst[PETools.GetRandNum(0, manLst.Count - 1, rand)];
        } else {
            randName += womanLst[PETools.GetRandNum(0, womanLst.Count - 1, rand)];
        }
        return randName;
    }

    #endregion

    #region 地图配置信息

    private Dictionary<int, MapCfg> mapCfgDataDict = new Dictionary<int, MapCfg>();//地图数据存储

    /// <summary>
    /// 初始化地图配置信息
    /// </summary>
    private void InitMapCfg(string path) {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeLst.Count; i++) {
                XmlElement element = nodeLst[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                MapCfg mapCfg = new MapCfg() {
                    ID = ID
                };
                foreach (XmlElement item in nodeLst[i].ChildNodes) {
                    switch (item.Name) {
                        case "mapName":
                            mapCfg.mapName = item.InnerText;
                            break;
                        case "sceneName":
                            mapCfg.sceneName = item.InnerText;
                            break;
                        case "mainCamPos":
                            string[] posArr = item.InnerText.Split(',');
                            mapCfg.mainCamPos = new Vector3(float.Parse(posArr[0]), float.Parse(posArr[1]), float.Parse(posArr[2]));
                            break;
                        case "mainCamRote":
                            string[] rotateArr = item.InnerText.Split(',');
                            mapCfg.mainCamRote = new Vector3(float.Parse(rotateArr[0]), float.Parse(rotateArr[1]), float.Parse(rotateArr[2]));
                            break;
                        case "playerBornPos":
                            string[] bornPosArr = item.InnerText.Split(',');
                            mapCfg.playerBornPos = new Vector3(float.Parse(bornPosArr[0]), float.Parse(bornPosArr[1]), float.Parse(bornPosArr[2]));
                            break;
                        case "playerBornRote":
                            string[] bornRotateArr = item.InnerText.Split(',');
                            mapCfg.playerBornRote = new Vector3(float.Parse(bornRotateArr[0]), float.Parse(bornRotateArr[1]), float.Parse(bornRotateArr[2]));
                            break;
                    }
                }
                mapCfgDataDict.Add(ID, mapCfg);
            }
        } else {
            PECommon.Log("xml file:" + PathDefine.MapCfgPath + "  not exist...", LogType.Error);
        }
    }
    /// <summary>
    /// 根据ID获取地图数据
    /// </summary>
    /// <param name="id">地图ID</param>
    /// <returns>地图配置数据</returns>
    public MapCfg GetMapCfgData(int id) {
        MapCfg mapCfg;
        if(mapCfgDataDict.TryGetValue(id,out mapCfg)) {
            return mapCfg;
        }
        return null;
    }

    #endregion

    #region 任务引导配置信息

    private Dictionary<int, GuideCfg> guideCfgDataDict = new Dictionary<int, GuideCfg>();

    private void InitGuideCfg(string path) {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (xml) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeLst.Count; i++) {
                XmlElement element = nodeLst[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                GuideCfg guideCfg = new GuideCfg() {
                    ID = ID
                };
                foreach (XmlElement item in nodeLst[i].ChildNodes) {
                    switch (item.Name) {
                        case "npcID":
                            guideCfg.npcID = int.Parse(item.InnerText);
                            break;
                        case "dilogArr":
                            guideCfg.dilogArr = item.InnerText;
                            break;
                        case "actID":
                            guideCfg.actID = int.Parse(item.InnerText);
                            break;
                    }
                }
                guideCfgDataDict.Add(ID, guideCfg);
            }
        } else {
            PECommon.Log("xml file:" + PathDefine.MapCfgPath + "  not exist...", LogType.Error);
        }
    }

    public GuideCfg GetGuideCfgData(int id) {
        GuideCfg guide;
        if(guideCfgDataDict.TryGetValue(id,out guide)) {
            return guide;
        }
        return null;
    }

    #endregion

    #endregion

}