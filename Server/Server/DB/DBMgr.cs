/****************************************************
	文件：DBMgr.cs
	作者：LSC
	邮箱: 314623971@qq.com
	日期：2020/12/26 22:24   	
	功能：数据库管理类
*****************************************************/

using MySql.Data.MySqlClient;
using PEProtocol;
using System;

public class DBMgr {

    private static DBMgr instance = null;
    public static DBMgr Instance {
        get {
            if(instance == null) {
                instance = new DBMgr();
            }
            return instance;
        }
    }

    private MySqlConnection conn = null;

    public void Init() {
        conn = new MySqlConnection("server = localhost;User Id = root;password =;Database = darkgod;Charset = utf8");
        conn.Open();
        PECommon.Log("DBMgr init done...");
    }

    /// <summary>
    /// 查询帐号数据
    /// </summary>
    /// <param name="account">帐号</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public PlayerData QueryPlayerData(string account,string password) {
        PlayerData playerData = null;
        MySqlDataReader reader = null;
        bool isNew = true;//是否是新用户
        try {
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
            cmd.Parameters.AddWithValue("acct", account);
            reader = cmd.ExecuteReader();
            if(reader.Read()) {
                isNew = false;
                string pwd = reader.GetString("password");
                if(pwd.Equals(password)) {
                    //密码正确
                    playerData = new PlayerData {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("level"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond"),
                        hp = reader.GetInt32("hp"),
                        ad = reader.GetInt32("ad"),
                        ap = reader.GetInt32("ap"),
                        addef = reader.GetInt32("addef"),
                        apdef = reader.GetInt32("apdef"),
                        dodge = reader.GetInt32("dodge"),
                        pierce = reader.GetInt32("pierce"),
                        critical = reader.GetInt32("critical"),
                        guideid = reader.GetInt32("guideid"),
                    };
                }else {
                    //密码不正确
                }
            }
        } catch (Exception e) {
            PECommon.Log("Query PlayerData By acct&password Error:" + e, LogType.Error);
        } finally {
            if(reader != null) {
                reader.Close();
            }
            if(isNew) {
                //新用户 数据库中没有用户数据 创建新的默认帐号数据 并返回
                playerData = new PlayerData {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 5000,
                    diamond = 500,

                    hp = 2000,
                    ad = 275,
                    ap = 265,
                    addef = 67,
                    apdef = 43,
                    dodge = 7,
                    pierce = 5,
                    critical = 2,
                    guideid = 1001,
                };
                playerData.id = InsertNewAccountData(account, password, playerData);
            }
        }
        return playerData;
    }

    /// <summary>
    /// 往数据库中插入新的用户数据
    /// </summary>
    /// <param name="acct">帐号</param>
    /// <param name="pass">密码</param>
    /// <param name="playerData">用户数据</param>
    /// <returns>返回用户的id</returns>
    private int InsertNewAccountData(string acct,string pass,PlayerData playerData) {
        int id = -1;

        try {
            MySqlCommand cmd = new MySqlCommand("insert into account set acct = @acct,password = @password,name = @name,level = @level,exp = @exp,power = @power,coin = @coin,diamond = @diamond,hp = @hp,ad = @ad,ap = @ap,addef = @addef,apdef = @apdef,dodge = @dodge,pierce = @pierce,critical = @critical,guideid = @guideid", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("password", pass);
            cmd.Parameters.AddWithValue("name", playerData.name);
            cmd.Parameters.AddWithValue("level", playerData.lv);
            cmd.Parameters.AddWithValue("exp", playerData.exp);
            cmd.Parameters.AddWithValue("power", playerData.power);
            cmd.Parameters.AddWithValue("coin", playerData.coin);
            cmd.Parameters.AddWithValue("diamond", playerData.diamond);

            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("ap", playerData.ap);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);

            cmd.Parameters.AddWithValue("guideid", playerData.guideid);

            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        } catch (Exception e) {
            PECommon.Log("Insert PlayerData Error:" + e, LogType.Error);
        }

        return id;
    }

    /// <summary>
    /// 查询玩家名字是否存在
    /// </summary>
    /// <param name="name">要查询的名字</param>
    /// <returns></returns>
    public bool QueryNameData(string name) {
        bool isExist = false;
        MySqlDataReader reader = null;
        try {
            MySqlCommand cmd = new MySqlCommand("select * from account where name = @name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if(reader.Read()) {
                isExist = true;
            }
        } catch (Exception e) {
            PECommon.Log("Query name error:" + e, LogType.Error);
        } finally {
            if (reader != null) reader.Close();
        }
        return isExist;
    }

    /// <summary>
    /// 更新玩家数据
    /// </summary>
    /// <param name="id">玩家id</param>
    /// <param name="playerData">玩家数据</param>
    /// <returns>是否更新成功</returns>
    public bool UpdatePlayerData(int id,PlayerData playerData) {
        try {
            MySqlCommand cmd = new MySqlCommand("update account set name = @name,level = @level,exp = @exp,power = @power,coin = @coin,diamond = @diamond,hp = @hp,ad = @ad,ap = @ap,addef = @addef,apdef = @apdef,dodge = @dodge,pierce = @pierce,critical = @critical,guideid = @guideid where id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", playerData.name);
            cmd.Parameters.AddWithValue("level", playerData.lv);
            cmd.Parameters.AddWithValue("exp", playerData.exp);
            cmd.Parameters.AddWithValue("power", playerData.power);
            cmd.Parameters.AddWithValue("coin", playerData.coin);
            cmd.Parameters.AddWithValue("diamond", playerData.diamond);

            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("ap", playerData.ap);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);

            cmd.Parameters.AddWithValue("guideid", playerData.guideid);

            cmd.ExecuteNonQuery();
        } catch (Exception e) {
            PECommon.Log("Update PlayerData Error:" + e, LogType.Error);
            return false;
        }
        return true;
    }

}
