using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarsDatabase : MonoBehaviour
{
    // Start is called before the first frame update

    public string Server= "http://127.0.0.1:14331";
    public string UserName="Admin";
    public string Password="Admin";

    private MarsWebClient mWebClient;

    private Dictionary<string,Tag> tags = new Dictionary<string,Tag>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag"></param>
    public void AddTag(string tag)
    {
        lock(tags)
        {
            if (!tags.ContainsKey(tag))
            {
                tags.Add(tag, new Tag() { Name = tag });
            }
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag"></param>
    public void RemoveTag(string tag)
    {
        lock (tags)
        {
            if (tags.ContainsKey(tag))
            {
                tags.Remove(tag);
            }
        }
    }


    public Tag GetTag(string tag)
    {
        if(tags.ContainsKey(tag))
        return tags[tag];
        else
            return null;
    }

    private IEnumerator UpdateValues()
    {
        for(; ; )
        {
            if(!mWebClient.IsLogin)
            {
                yield return mWebClient.Login(UserName, Password);
            }
            else
            {
                if(tags.Count()>0)
                {
                   yield  return mWebClient.GetTagRealValues(tags.Values);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }


    void Start()
    {
        AddTag("tag1");
        mWebClient = new MarsWebClient() { Server = Server };
        StartCoroutine(UpdateValues());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
