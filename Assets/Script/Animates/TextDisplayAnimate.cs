using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayAnimate : AnimateBase
{
    private Text mTextObj;
    private TextMesh mTextMesh;
    private TextMeshProUGUI mTextMeshProUGUI;

    public AnimateValueType ValueType = AnimateValueType.Analog;

    public byte decimalCount=2;
    public string trueString="True";
    public string falseString="False";
    public string timeFormate = "yyyy-MM-dd HH:mm:ss";

    public enum AnimateValueType
    {
        String,
        Analog,
        Digital,
        DateTime
    }


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        mTextObj = this.GetComponentInChildren<Text>();
        
        if(mTextObj==null)
        {
            mTextMesh = GetComponentInChildren<TextMesh>();
        }

        if(mTextMesh==null)
        {
            mTextMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if(mTextObj!=null)
        {
            mTextObj.text = Formate(this.mValue);
        }
        else if(mTextMesh!=null)
        {
            mTextMesh.text = Formate(this.mValue);
        }
        else if(mTextMeshProUGUI!=null)
        {
            mTextMeshProUGUI.text = Formate(this.mValue);
        }
    }

    private string Formate(string value)
    {
        switch (ValueType)
        {
            case AnimateValueType.String:
                return value;
            case AnimateValueType.Analog:
                if (double.TryParse(value, out double dval))
                {
                    return dval.ToString("F"+decimalCount);
                }
                return "???";
            case AnimateValueType.DateTime:
                try
                {
                    var vtime = DateTime.Parse(value);
                    if(string.IsNullOrEmpty(value))
                    {
                        return vtime.ToString();
                    }
                    else
                    {
                        return vtime.ToString(timeFormate);
                    }
                }
                catch
                {

                }
                return "???";
            case AnimateValueType.Digital:
                if (double.TryParse(value, out double dgval))
                {
                    if(dgval>0)
                    {
                        return trueString;
                    }
                    else
                    {
                        return falseString;
                    }
                }
                return "???";
            default:
                return value;
        }
    }

    
}
