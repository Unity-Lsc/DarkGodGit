/****************************************************
	文件：PETools.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/24 21:51   	
	功能：工具类
*****************************************************/
using UnityEngine;
using System.Collections;

public class PETools {

    /// <summary>
    /// 获取一个随机数
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    public static int GetRandNum(int min,int max,System.Random rand = null) {
        if(rand == null) {
            rand = new System.Random();
        }
        return rand.Next(min, max + 1);
    }


}
