using System;
using UnityEngine;

[Serializable]
public class WeightedValue
{
    public Sprite PrizeSprite;
    public string Value;
    public int Weight;
    public string WinText;

    public WeightedValue(string value, int weight, Sprite image, string winText)
    {
        Value = value;
        Weight = weight;
        PrizeSprite = image;
        WinText = winText;
    }
}
