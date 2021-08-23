using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelManager : MonoBehaviour
{
    [SerializeField] private float SpinDuration = 1;
    [SerializeField] private float SpinsBeforeStopping = 5;
    [SerializeField] private int unitTestCount = 1000;
    [SerializeField] private GameObject wheelBackground;
    [SerializeField] private WinPanel winPanel;
    [SerializeField] private List<WeightedValue> WeightedPrizes = new List<WeightedValue>();
    private WeightedValue chosenPrize;

    
    private readonly List<string> weightedList = new List<string>();
    
    private bool spinning;
    private float anglePerItem;
    private float totalWeight = 0;

    void Start()
    {
        spinning = false;
        anglePerItem = 360f / WeightedPrizes.Count;

        foreach (var value in WeightedPrizes)
        {
            for (var i = 0; i < value.Weight; i++)
            {
                weightedList.Add(value.Value);
            }
            
            totalWeight += value.Weight;
        }

        if (totalWeight != 100f)
        {
            throw new Exception("Sector Weights do not equal 100! Make sure weights total to exactly 100 in the WheelManager script! Current weight total: " + totalWeight);
        }
    }
    
    private string GetPrize()
    {
        var randomIndex = Random.Range(0, weightedList.Count);
        return weightedList[randomIndex];
    }

    public void Spin()
    {
        if (spinning) return;

        spinning = true;

        var itemName = GetPrize();
        
        var itemIndex = WeightedPrizes.FindIndex(w => w.Value == itemName);
        chosenPrize = WeightedPrizes[itemIndex];
        var itemNumberAngle = itemIndex * anglePerItem;
        var currentAngle = wheelBackground.transform.eulerAngles.z;
        
        while (currentAngle >= 360)
        {
            currentAngle -= 360;
        }
        while (currentAngle < 0)
        {
            currentAngle += 360;
        }
        
        var targetAngle = (itemNumberAngle + 360f * SpinsBeforeStopping) + (anglePerItem / 2);

        StartCoroutine(SpinTheWheel(currentAngle, targetAngle, SpinsBeforeStopping * SpinDuration));
    }
    
    private IEnumerator SpinTheWheel(float fromAngle, float toAngle, float withinSeconds)
    {
        var passedTime = 0f;
        
        while (passedTime < withinSeconds)
        {
            var lerpFactor = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, passedTime / withinSeconds)));

            wheelBackground.transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(fromAngle, toAngle, lerpFactor));
            
            passedTime += Time.deltaTime;

            yield return null;
        }

        wheelBackground.transform.eulerAngles = new Vector3(0.0f, 0.0f, toAngle);
        spinning = false;
        ToggleWinPanel();
    }

    public void UnitTest()
    {
        for (int i = 0; i < unitTestCount; i++)
        {
            Debug.Log("Prize: " + GetPrize());
        }
    }

    public void ToggleWinPanel()
    {
        winPanel.winText.text = chosenPrize.WinText;
        winPanel.winImage.sprite = chosenPrize.PrizeSprite;
        winPanel.gameObject.SetActive(!winPanel.gameObject.activeSelf);

        if (winPanel.gameObject.activeSelf)
        {
            GameObject.Find("WinSFX").GetComponent<AudioSource>().Play();
        }
    }
}
