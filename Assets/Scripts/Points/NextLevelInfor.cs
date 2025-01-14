﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelInfor : MonoBehaviour
{
    public Upgrade upgrade;

    [Header("Click tay")]
    public Text nextLevelCost;
    public Text nextLevelMoney;
    private float nextCost;
    private float nextMoney;

    [Header("Auto click")]
    public Text nextLevelCostProduct;
    public Text nextLevelMoneyProduct;
    private float nextCostProduct;
    private float nextMoneyProduct;

    public void ShowInfoNextLevel(GameObject point)
    {
        PointInfo pInf = point.GetComponent<Point>().pInfo;

        nextCost = pInf.prcClick * upgrade.priceUpgrade;
        nextMoney = pInf.proClick * upgrade.moneyEarnedUpgrade;

        nextLevelCost.text = "+$: " + Math.Round(nextCost, 3);
        nextLevelMoney.text = "Buy: " + Math.Round(nextMoney, 3);

        nextCostProduct = pInf.prcPro * upgrade.priceUpgrade;
        nextMoneyProduct = pInf.proPro * upgrade.moneyEarnedUpgrade;

        nextLevelCostProduct.text = "+$: " + Math.Round(nextCostProduct, 3);
        nextLevelMoneyProduct.text = "Buy: " + Math.Round(nextMoneyProduct, 3);
    }
}
