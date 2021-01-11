/****************************************************
	文件：ServerRoot.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/25 21:28   	
	功能：服务器初始化
*****************************************************/

public class ServerRoot {

    private static ServerRoot instance = null;
    public static ServerRoot Instance {
        get {
            if(instance == null) {
                instance = new ServerRoot();
            }
            return instance;
        }
    }

    public void Init() {
        //数据层初始化
        DBMgr.Instance.Init();

        //服务层初始化
        CacheService.Instance.Init();
        NetService.Instance.Init();

        //业务系统层初始化
        LoginSystem.Instance.Init();
    }

    public void Update() {
        NetService.Instance.Update();
    }

    private int SessionID = 0;
    public int GetSessionID() {
        if(SessionID >= int.MaxValue) {
            SessionID = 0;
        }
        return SessionID += 1;
    }

}
