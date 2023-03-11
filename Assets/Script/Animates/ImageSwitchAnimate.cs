using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitchAnimate : AnimateBase
{
    public ImageItem[] Images;

    private Image mImage;


    private ImageItem[] mSortedColors;

    ImageItem mOrignalColor;

    ImageItem mLastItem;

    // Start is called before the first frame update

    new void Start()
    {
        base.Start();
        mImage = this.GetComponentInChildren<Image>();

        if (Images != null && Images.Length > 0)
        {
            mSortedColors = Images.OrderByDescending(e => e.value).ToArray();
        }
        else
        {
            mSortedColors = new ImageItem[0];
        }
        

        if (mImage != null)
        {
            mOrignalColor = new ImageItem() { Sprite = mImage.sprite };
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
                if (mImage != null)
                {
                   var item  = GetValueItem(dval);
                    if(item!=mLastItem)
                    {
                        mLastItem = item;
                        mImage.sprite = item.Sprite;
                    }
                    mImage.SetAllDirty();
                 
                }
            }
        }
    }

    private ImageItem GetValueItem(double dval)
    {
        foreach (var vv in mSortedColors)
        {
            if (dval >= vv.value)
            {
                return vv;
            }
        }
        return mOrignalColor;
    }
}

[Serializable]
public class ImageItem
{
    public double value;
    public Sprite Sprite;
}
