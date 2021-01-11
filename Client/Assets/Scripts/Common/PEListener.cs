/****************************************************
    文件：PEListener.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：2020/12/30 17:15:39
	功能：UI事件监听
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PEListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

    public Action<PointerEventData> onClickDown;
    public Action<PointerEventData> onClickUp;
    public Action<PointerEventData> onDrag;

    public void OnDrag(PointerEventData eventData) {
        if(onDrag != null) {
            onDrag(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(onClickDown != null) {
            onClickDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if(onClickUp != null) {
            onClickUp(eventData);
        }
    }
}