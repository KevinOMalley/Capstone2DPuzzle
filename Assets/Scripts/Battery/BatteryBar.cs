using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBar : MonoBehaviour
{
    [SerializeField] private Battery playerBattery;
    [SerializeField] private Image totalBatteryBar;
    [SerializeField] private Image currentBatteryBar;

    private void Start()
    {
        totalBatteryBar.fillAmount = 1;
    }

    private void Update()
    {
        currentBatteryBar.fillAmount = playerBattery.currentCharges / 10;
    }
}
