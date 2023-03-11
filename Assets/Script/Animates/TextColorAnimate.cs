using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextColorAnimate : AnimateBase
{
    public ColorItem[] Colors;

    private TextMesh mTextMesh;
    private TextMeshProUGUI mTextMeshProUGUI;

    private Text mTextObj;

    private ColorItem[] mSortedColors;

    ColorItem mOrignalColor;

    // Start is called before the first frame update

    new void Start()
    {
        base.Start();
        mTextObj = this.GetComponentInChildren<Text>();

        if (mTextObj == null)
        {
            mTextMesh = GetComponentInChildren<TextMesh>();
           
        }

        if (mTextMesh == null)
        {
            mTextMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            mOrignalColor = new ColorItem() { color = mTextMeshProUGUI.color };
        }

        if (Colors != null && Colors.Length > 0)
        {
            mSortedColors = Colors.OrderByDescending(e => e.value).ToArray();
        }
        else
        {
            mSortedColors = new ColorItem[0];
        }


        if (mTextMeshProUGUI != null)
        {
            mOrignalColor = new ColorItem() { color = mTextMeshProUGUI.color };
        }
        else if (mTextObj != null)
        {
            mOrignalColor = new ColorItem() { color = mTextObj.color };
        }
        else if (mTextMesh != null)
        {
            mOrignalColor = new ColorItem() { color = mTextMesh.color };
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (double.TryParse(mValue, out double dval))
        {
            if (mSortedColors.Length > 0)
            {
                if (mTextObj != null)
                {
                    mTextObj.color = GetValueItem(dval).color;
                }
                else if (mTextMesh != null)
                {
                    mTextMesh.color = GetValueItem(dval).color;
                }
                else if (mTextMeshProUGUI != null)
                {
                    mTextMeshProUGUI.color = GetValueItem(dval).color;
                }
            }
        }
    }

    private ColorItem GetValueItem(double dval)
    {
        foreach(var vv in mSortedColors)
        {
            if(dval>=vv.value)
            {
                return vv;  
            }
        }
        return mOrignalColor;
    }

}

[Serializable]
public class ColorItem
{
    public double value;
    public Color color;
}
