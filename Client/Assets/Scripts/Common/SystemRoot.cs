/****************************************************
    文件：SystemRoot.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：业务系统基类
*****************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour 
{

    protected ResService resService = null;
    protected AudioService audioService = null;
    protected NetService netService = null;

    public virtual void InitSystem() {
        resService = ResService.Instance;
        audioService = AudioService.Instance;
        netService = NetService.Instance;
    }

}