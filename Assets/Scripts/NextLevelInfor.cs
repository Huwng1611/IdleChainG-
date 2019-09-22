using UnityEngine;
using UnityEngine.UI;

public class NextLevelInfor : MonoBehaviour
{
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

        nextCost = pInf.prcClick * GameManager.instance.priceUpgrade;
        nextMoney = pInf.proClick * GameManager.instance.moneyEarnedUpgrade;

        nextLevelCost.text = "+$: " + nextCost;
        nextLevelMoney.text = "Buy: " + nextMoney;

        nextCostProduct = pInf.prcPro * GameManager.instance.priceUpgrade;
        nextMoneyProduct = pInf.proPro * GameManager.instance.moneyEarnedUpgrade;

        nextLevelCostProduct.text = "+$: " + nextCostProduct;
        nextLevelMoneyProduct.text = "Buy: " + nextMoneyProduct;
    }
}
