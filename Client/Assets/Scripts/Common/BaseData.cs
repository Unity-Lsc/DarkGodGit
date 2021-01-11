/****************************************************
    文件：BaseData.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：2020/12/31 13:49:56
	功能：配置数据类
*****************************************************/

using UnityEngine;

public class BaseData<T> {
    public int ID;
}
/// <summary>
/// 副本配置数据
/// </summary>
public class MapCfg : BaseData<MapCfg> {
    public string mapName;
    public string sceneName;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
}
/// <summary>
/// 任务引导配置数据
/// </summary>
public class GuideCfg : BaseData<GuideCfg> {
    public int npcID;//触发任务的目标NPC索引
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}