/****************************************************
    文件：Constants.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：#CreateTime#
	功能：常量配置
*****************************************************/

using UnityEngine;

public class Constants
{
    //场景名称,ID
    public const string SceneLogin = "SceneLogin";//登录注册场景
    public const int SceneMainCityID = 10000;//主城场景ID

    //音效名称
    public const string AudioBgLogin = "bgLogin";//登录注册页面背景音乐
    public const string AudioBgMainCity = "bgMainCity";//主城背景音乐
    public const string AudioUILoginBtn = "uiLoginBtn";//进入游戏按钮音效
    public const string AudioUIClickBtn = "uiClickBtn";//UI点击音效
    public const string AudioUIExtenBtn = "uiExtenBtn";//菜单按钮音效
    public const string AudioUIOpenPage = "uiOpenPage";//打开页面

    //屏幕标准宽高
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;

    //滑竿滑动最大距离
    public const int ScreenOperaDiatance = 90;

    //角色相关
    public const int PlayerMoveSpeed = 8;//玩家移动速度
    public const int MonsterMoveSpeed = 4;//怪物移动速度
    public const float AccelerSpeed = 5.0f;//运动平滑加速度

    //角色动画相关
    public const int BlendIdle = 0;//站立状态
    public const int BlendWalk = 1;//行走状态

    //本地存储
    public const string Account = "Account";//账号
    public const string Password = "Password";//密码

}