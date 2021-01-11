/****************************************************
    文件：LoopDragonAni.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：登录注册界面 飞龙动画
*****************************************************/

using UnityEngine;

public class LoopDragonAni : MonoBehaviour 
{

    private Animation ani;

    private void Awake() {
        ani = GetComponent<Animation>();
    }

    private void Start() {
        if(ani != null) {
            InvokeRepeating("PlayDragonAni", 0, 20);
        }
    }

    private void PlayDragonAni() {
        if(ani != null) {
            ani.Play();
        }
    }

}