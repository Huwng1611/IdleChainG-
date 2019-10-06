using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public PointsController pControl;

    [Header("Upgrade")]
    /// <summary>
    /// cooldown giảm 1.1 lần sau mỗi upgrade
    /// </summary>
    public float coolDownUpgrade = 1.1f;
    /// <summary>
    /// giá upgrade tăng mỗi 1.2 lần sau upgrade
    /// </summary>
    public float priceUpgrade = 1.2f;
    /// <summary>
    /// số tiền thu đc sau click tăng 1.05 lần sau upgrade
    /// </summary>
    public float moneyEarnedUpgrade = 1.05f;

    /// <summary>
    /// upgrade click
    /// </summary>
    public void AfterUpgradeClick()
    {
        GameObject point;
        point = pControl.objTemp;
        point.GetComponent<Point>();
        PointInfo pInf = point.GetComponent<Point>().pInfo;
        if (GameManager.instance.money >= pInf.prcClick)
        {
            GameManager.instance.money -= pInf.prcClick;
            pInf.tgClick /= coolDownUpgrade;
            pInf.prcClick *= priceUpgrade;
            pInf.proClick *= moneyEarnedUpgrade;
        }
    }

    /// <summary>
    /// upgrade auto click
    /// </summary>
    public void AfterUpgradeProduction()
    {
        GameObject point;
        point = pControl.objTemp;
        PointInfo pInf = point.GetComponent<Point>().pInfo;
        if (GameManager.instance.money > pInf.prcPro)
        {
            GameManager.instance.money -= pInf.prcPro;
            pInf.tgPro /= coolDownUpgrade;
            pInf.prcPro *= priceUpgrade;
            pInf.proPro *= moneyEarnedUpgrade;

        }
    }
}
