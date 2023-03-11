using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillColorAnimate : AnimateBase
{
    public ColorItem[] Colors;

    private Image mImage;


    private ColorItem[] mSortedColors;

    ColorItem mOrignalColor;

    // Start is called before the first frame update

    new void Start()
    {
        base.Start();
        mImage = this.GetComponentInChildren<Image>();

        if (Colors != null && Colors.Length > 0)
        {
            mSortedColors = Colors.OrderByDescending(e => e.value).ToArray();
        }
        else
        {
            mSortedColors = new ColorItem[0];
        }
        

        if (mImage != null)
        {
            mOrignalColor = new ColorItem() { color = mImage.color };
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
                    mImage.color = GetValueItem(dval).color;
                }
            }
        }
    }

    private ColorItem GetValueItem(double dval)
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
