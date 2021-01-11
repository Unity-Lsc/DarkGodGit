/****************************************************
    文件：DynamicWnd.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：动态UI元素界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot 
{

    private Animation aniTips;
    private Text txtTips;
    private Queue<string> tipsQueue = new Queue<string>();
    private bool isTipsShow = false;

    private void Awake() {
        aniTips = transform.Find("txtTip").GetComponent<Animation>();
        txtTips = transform.Find("txtTip").GetComponent<Text>();
    }

    protected override void InitWnd() {
        base.InitWnd();
        SetActive(txtTips, false);
    }

    public void AddTips(string tips) {
        lock(tipsQueue) {
            tipsQueue.Enqueue(tips);
        }
    }

    private void SetTips(string content) {
        SetActive(txtTips);
        SetText(txtTips, content);
        isTipsShow = true;

        AnimationClip clip = aniTips.GetClip("TipsShow");
        aniTips.Play();
        StartCoroutine(AniPlayDone(clip.length, () => {
            SetActive(txtTips, false);
            isTipsShow = false;
        }));
    }

    private IEnumerator AniPlayDone(float time,Action callback) {
        yield return new WaitForSeconds(time);
        if(callback != null) {
            callback();
        }
    }

    private void Update() {
        if(tipsQueue.Count > 0 && !isTipsShow) {
            lock(tipsQueue) {
                string tips = tipsQueue.Dequeue();
                SetTips(tips);
            }
        }
    }

}