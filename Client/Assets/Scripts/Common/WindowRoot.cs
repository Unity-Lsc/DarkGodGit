/****************************************************
    文件：WindowRoot.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：UI界面基类
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : BasePanel 
{

    protected ResService resService = null;
    protected AudioService audioService = null;
    protected NetService netService = null;

    /// <summary>
    /// 设置界面显示隐藏状态
    /// </summary>
    /// <param name="isActive">状态</param>
    public void SetWndState(bool isActive = true) {
        if(gameObject.activeSelf != isActive) {
            SetActive(gameObject, isActive);
            if(isActive) {
                InitWnd();
            } else {
                ClearWnd();
            }
        }
    }

    /// <summary>
    /// 初始化界面
    /// </summary>
    protected virtual void InitWnd() {
        resService = ResService.Instance;
        audioService = AudioService.Instance;
        netService = NetService.Instance;
    }
    /// <summary>
    /// 释放界面
    /// </summary>
    protected virtual void ClearWnd() {
        resService = null;
        audioService = null;
        netService = null;
    }

    #region 工具方法

    //设置文本内容
    protected void SetText(Text txt, string content = "") {
        txt.text = content;
    }
    protected void SetText(Text txt, int num = 0) {
        txt.text = num + "";
    }
    protected void SetText(Transform trans, string content = "") {
        SetText(trans.GetComponent<Text>(), content);
    }
    protected void SetText(Transform trans, int num = 0) {
        SetText(trans.GetComponent<Text>(), num);
    }

    //设置物体显示
    protected void SetActive(GameObject go, bool state = true) {
        go.SetActive(state);
    }
    protected void SetActive(Transform trans,bool state = true) {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rect, bool state = true) {
        rect.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true) {
        img.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true) {
        txt.gameObject.SetActive(state);
    }

    //设置Image图片
    protected void SetSprite(Image img,string path) {
        Sprite sp = resService.LoadSprite(path, true);
        img.sprite = sp;
    }

    protected T GetOrAddComponent<T>(GameObject go) where T: Component {
        T t = go.GetComponent<T>();
        if(t == null) {
            t = go.AddComponent<T>();
        }
        return t;
    }

    #endregion

    #region 点击事件

    protected void OnClickDown(GameObject go, Action<PointerEventData> cb) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = cb;
    }

    protected void OnClickUp(GameObject go, Action<PointerEventData> cb) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = cb;
    }

    protected void OnDrag(GameObject go, Action<PointerEventData> cb) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = cb;
    }

    #endregion

}