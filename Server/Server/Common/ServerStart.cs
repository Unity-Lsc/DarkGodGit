/****************************************************
	文件：ServerStart.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/25 21:27   	
	功能：服务器入口
*****************************************************/

class ServerStart {
    static void Main(string[] args) {

        ServerRoot.Instance.Init();

        while(true) {
            ServerRoot.Instance.Update();
        }

    }
}
