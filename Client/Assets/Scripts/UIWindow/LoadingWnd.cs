/****************************************************
    文件：LoadingWnd.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：加载界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot 
{
    private Text txtTips;
    private Image imgBar;
    private Image imgPoint;
    private Text txtProcess;

    private float barWidth;

    private void Awake() {
        txtTips = transform.Find("txtTips").GetComponent<Text>();
        imgBar = transform.Find("imgBar").GetComponent<Image>();
        imgPoint = imgBar.transform.Find("imgPoint").GetComponent<Image>();
        txtProcess = imgBar.transform.Find("txtProcess").GetComponent<Text>();
    }

    protected override void InitWnd() {
        base.InitWnd();
        imgBar.fillAmount = 0;
        imgPoint.transform.localPosition = new Vector2(-545, 0);
        SetText(txtProcess, "0%");
        barWidth = imgBar.GetComponent<RectTransform>().sizeDelta.x;
    }

    /// <summary>
    /// 设置进度条
    /// </summary>
    /// <param name="percent">进度</param>
    public void SetProgress(float percent) {
        SetText(txtProcess, (int)(percent * 100) + "%");
        imgBar.fillAmount = percent;
        float posX = percent * barWidth - (barWidth / 2);
        imgPoint.transform.localPosition = new Vector2(posX, 0);
    }

}