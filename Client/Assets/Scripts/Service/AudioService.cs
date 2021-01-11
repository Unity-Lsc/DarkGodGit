/****************************************************
    文件：AudioService.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：声音播放服务
*****************************************************/

using UnityEngine;

public class AudioService : MonoBehaviour 
{

    public static AudioService Instance = null;

    private AudioSource bgAudio;
    private AudioSource uiAudio;

    private void Awake() {
        bgAudio = transform.Find("BGAudio").GetComponent<AudioSource>();
        uiAudio = transform.Find("UIAudio").GetComponent<AudioSource>();
    }

    public void InitService() {
        Instance = this;
        PECommon.Log("Init Audio Service...");
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name">声音名字</param>
    /// <param name="isLoop">是否循环播放</param>
    public void PlayBgMusic(string name,bool isLoop = true) {
        AudioClip clip = ResService.Instance.LoadAudio(name, true);
        if(bgAudio.clip == null || bgAudio.clip.name != clip.name) {
            bgAudio.clip = clip;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }

    /// <summary>
    /// 播放UI音效
    /// </summary>
    /// <param name="name">声音名字</param>
    public void PlayUIAudio(string name) {
        AudioClip clip = ResService.Instance.LoadAudio(name, true);
        uiAudio.clip = clip;
        uiAudio.Play();
    }

}