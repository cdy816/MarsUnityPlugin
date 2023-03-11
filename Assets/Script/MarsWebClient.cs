using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Mars 数据库连接客户端
/// </summary>
public class MarsWebClient
{

    #region ... Variables  ...

    private string mToken=string.Empty;

    #endregion ...Variables...

    #region ... Events     ...

    #endregion ...Events...

    #region ... Constructor...

    #endregion ...Constructor...

    #region ... Properties ...
    /// <summary>
    /// 
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsLogin { get; set; }


    #endregion ...Properties...

    #region ... Methods    ...


    private UnityWebRequest GetPost(string fun,string sval)
    {
        //UnityWebRequest req = UnityWebRequest.Post(Server + "/" + fun, sval, "application/json;charset=utf-8");
        UnityWebRequest req = new UnityWebRequest(Server + "/" + fun, UnityWebRequest.kHttpVerbPOST);
        req.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(sval)) { contentType = "application/json;charset=utf-8" };
        req.downloadHandler = new DownloadHandlerBuffer();
        req.useHttpContinue = false;
        return req;
    }

    private UnityWebRequest GetGet(string fun, string sval)
    {
        UnityWebRequest req = new UnityWebRequest(Server + "/" + fun, UnityWebRequest.kHttpVerbGET);
        req.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(sval)) { contentType = "application/json;charset=utf-8" };
        req.downloadHandler = new DownloadHandlerBuffer();
        //UnityWebRequest req = UnityWebRequest.Get(Server + "/" + fun, sval, "application/json;charset=utf-8");
        req.useHttpContinue = false;
        return req;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public IEnumerator Login(string username,string password)
    {
        LoginMessage mm = ScriptableObject.CreateInstance<LoginMessage>();
        mm.UserName= username;
        mm.Password= password;
        using (var req = GetPost("Login/TryLogin",JsonUtility.ToJson(mm)))
        {
           yield return req.SendWebRequest();

            if(req.isDone && req.downloadHandler!=null)
            {
                if (req.downloadHandler.data != null)
                {

                    var fs = JsonUtility.FromJson(req.downloadHandler.text, typeof(LoginResponse)) as LoginResponse;
                    if (fs != null && fs.result)
                    {
                        mToken = fs.token;
                        IsLogin = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator Logout()
    {
        Requestbase re = new Requestbase() { Token = mToken };
        using (var req = GetPost("Login/Logout", JsonUtility.ToJson(re)))
        {
            yield return req.SendWebRequest();

            if (req.isDone && req.downloadHandler != null)
            {
                if (!string.IsNullOrEmpty(req.downloadHandler.text))
                {
                    var fs = JsonUtility.FromJson(req.downloadHandler.text, typeof(ResponseBase)) as ResponseBase;
                    if (fs != null && fs.result)
                    {
                        mToken = "";
                        IsLogin = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tags"></param>
    /// <returns></returns>
    public IEnumerator GetTagRealValues(IEnumerable<Tag> tags)
    {
        if (IsLogin)
        {
            RealDataRequest rd = ScriptableObject.CreateInstance<RealDataRequest>();
            rd.TagNames = tags.Select(e => e.Name).ToArray();
            rd.Token = mToken;
            using (var req = GetGet("RealData", JsonUtility.ToJson(rd)))
            {
                yield return req.SendWebRequest();

                if (req.isDone && req.downloadHandler != null)
                {
                    if (!string.IsNullOrEmpty(req.downloadHandler.text))
                    {
                        var fs = JsonUtility.FromJson(req.downloadHandler.text, typeof(RealValueQueryResponse)) as RealValueQueryResponse;
                        if (fs != null && fs.result)
                        {
                            var vss = fs.datas.ToDictionary(e => e.name);
                            yield return null;

                            int i = 0;
                            foreach (var vv in tags)
                            {
                                if (vss.ContainsKey(vv.Name))
                                {
                                    var val = vss[vv.Name];
                                    vv.Quality = val.quality;
                                    vv.Time =  DateTime.Parse(val.time);
                                    vv.Value = val.value;
                                }
                                i++;
                                if(i%1000==0)
                                {
                                    yield return null;
                                }
                            }
                            IsLogin = true;
                        }
                    }
                }
            }
        }
    }

    #endregion ...Methods...

    #region ... Interfaces ...

    #endregion ...Interfaces...
}

/// <summary>
/// 实时值请求返回
/// 包括时间、值、质量
/// </summary>
[Serializable]
public class RealValueQueryResponse : ResponseBase
{

    #region ... Variables  ...

    #endregion ...Variables...

    #region ... Events     ...

    #endregion ...Events...

    #region ... Constructor...

    #endregion ...Constructor...

    #region ... Properties ...
    /// <summary>
    /// 实时值集合
    /// </summary>
    public RealValue[] datas;
    #endregion ...Properties...

    #region ... Methods    ...

    #endregion ...Methods...

    #region ... Interfaces ...

    #endregion ...Interfaces...
}

/// <summary>
/// 实时值(时间、质量戳、值)
/// </summary>
[Serializable]
public class RealValue
{

    #region ... Variables  ...

    #endregion ...Variables...

    #region ... Events     ...

    #endregion ...Events...

    #region ... Constructor...

    #endregion ...Constructor...

    #region ... Properties ...
    /// <summary>
    /// 时间
    /// </summary>
    public string time;

    /// <summary>
    /// 质量戳
    /// </summary>
    public byte quality;

    /// <summary>
    /// 值
    /// </summary>
    public string value;

    /// <summary>
    /// 
    /// </summary>
    public string name;
    #endregion ...Properties...

    #region ... Methods    ...

    #endregion ...Methods...

    #region ... Interfaces ...

    #endregion ...Interfaces...
}

[Serializable]
public class LoginMessage: ScriptableObject
{

    #region ... Variables  ...

    #endregion ...Variables...

    #region ... Events     ...

    #endregion ...Events...

    #region ... Constructor...

    #endregion ...Constructor...

    #region ... Properties ...
    /// <summary>
    /// 
    /// </summary>
    public string UserName;

    /// <summary>
    /// 
    /// </summary>
    public string Password;

    #endregion ...Properties...

    #region ... Methods    ...

    #endregion ...Methods...

    #region ... Interfaces ...

    #endregion ...Interfaces...
}

/// <summary>
/// 
/// </summary>
[Serializable]
public class LoginResponse : ResponseBase
{

    #region ... Variables  ...

    #endregion ...Variables...

    #region ... Events     ...

    #endregion ...Events...

    #region ... Constructor...

    #endregion ...Constructor...

    #region ... Properties ...

    /// <summary>
    /// 登录时间
    /// </summary>
    public string loginTime;

    /// <summary>
    /// 超时时间
    /// </summary>
    public long timeOut;

    /// <summary>
    /// 登录Token
    /// </summary>
    public string token;

    #endregion ...Properties...

    #region ... Methods    ...

    #endregion ...Methods...

    #region ... Interfaces ...

    #endregion ...Interfaces...
}

/// <summary>
/// 请求返回结果基类
/// </summary>
[Serializable]
public class ResponseBase
{

    #region ... Variables  ...

    #endregion ...Variables...

    #region ... Events     ...

    #endregion ...Events...

    #region ... Constructor...

    #endregion ...Constructor...

    #region ... Properties ...
    /// <summary>
    /// 操作结果
    /// True：请求成功
    /// </summary>
    public bool result;

    /// <summary>
    /// 错误消息
    /// </summary>
    public string erroMessage;

    #endregion ...Properties...

    #region ... Methods    ...

    #endregion ...Methods...

    #region ... Interfaces ...

    #endregion ...Interfaces...
}


/// <summary>
/// 
/// </summary>
[Serializable]
public class Requestbase:ScriptableObject
{

    #region ... Variables  ...

    #endregion ...Variables...

    #region ... Events     ...

    #endregion ...Events...

    #region ... Constructor...

    #endregion ...Constructor...

    #region ... Properties ...

    /// <summary>
    /// 登录Token
    /// </summary>
    public string Token;

    ///// <summary>
    ///// 
    ///// </summary>
    //public string Time { get; set; }

    #endregion ...Properties...

    #region ... Methods    ...

    #endregion ...Methods...

    #region ... Interfaces ...

    #endregion ...Interfaces...
}


/// <summary>
/// 实时数据请求
/// </summary>
[Serializable]
public class RealDataRequest : Requestbase
{

    #region ... Variables  ...

    #endregion ...Variables...

    #region ... Events     ...

    #endregion ...Events...

    #region ... Constructor...

    #endregion ...Constructor...

    #region ... Properties ...

    /// <summary>
    /// 变量组
    /// 不为空时，请求组+变量名组合而成的变量的全名的变量的值
    /// </summary>
    public string Group;

    /// <summary>
    /// 变量名集合
    /// </summary>
    public string[] TagNames;

    #endregion ...Properties...

    #region ... Methods    ...

    #endregion ...Methods...

    #region ... Interfaces ...

    #endregion ...Interfaces...
}

//public class MarsAPIWebClient : WebClient
//{

//    #region ... Variables  ...

//    private int mTimeout = 1000 * 10;

//    #endregion ...Variables...

//    #region ... Events     ...

//    #endregion ...Events...

//    #region ... Constructor...

//    #endregion ...Constructor...

//    #region ... Properties ...


//    /// <summary>
//    /// 
//    /// </summary>
//    public int Timeout
//    {
//        get
//        {
//            return mTimeout;
//        }
//        set
//        {
//            if (mTimeout != value)
//            {
//                mTimeout = value;
//            }
//        }
//    }

//    #endregion ...Properties...

//    #region ... Methods    ...
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="address"></param>
//    /// <returns></returns>
//    protected override WebRequest GetWebRequest(Uri address)
//    {
//        var result = base.GetWebRequest(address);
//        result.Timeout = this.mTimeout;
//        return result;
//    }
//    #endregion ...Methods...

//    #region ... Interfaces ...

//    #endregion ...Interfaces...
//}
