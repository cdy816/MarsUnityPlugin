using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimateBase : MonoBehaviour
{
    // Start is called before the first frame update

    public MarsDatabase database;

    public string tagName;

    protected string mValue;

    protected byte mQuality;

    private Tag mTag;

    protected void Start()
    {
        if (database != null)
        {
            database.AddTag(tagName);
            mTag = database.GetTag(tagName);
        }
    }

    // Update is called once per frame
    protected void Update()
    {
       if(mTag != null)
        {
            mValue = mTag.Value!=null?mTag.Value.ToString():"";
            mQuality = mTag.Quality;
        }
    }
}
