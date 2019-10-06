using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyNewPoint : MonoBehaviour
{
    [Header("Number of actived point")]
    /// <summary>
    /// số các point đã mở, dùng để tính giá mở point tiếp theo
    /// </summary>
    public float numberActive;

    public GenerateMap genMap;
    public PointsController pControl;

    /// <summary>
    /// tham số thứ nhất để tính giá point tiếp theo
    /// </summary>
    public float inc1;
    /// <summary>
    /// tham số thứ hai để tính giá point tiếp theo
    /// </summary>
    public float inc2;
    //công thức tính giá point tiếp theo: inc1*inc2^n

    /// <summary>
    /// tính số tiền sau click của point mới
    /// </summary>
    public float proInc;
    /// <summary>
    /// thời gian cooldown của point mới
    /// </summary>
    public float tgInc;

    [Header("Buy Popup")]
    public GameObject buyPopup;
    public Text costText;
    public Button yesBtn;
    public Button noBtn;

    [Header("OK Popup")]
    public GameObject okPopup;
    public Text costTextOkPopup;
    public Button okBtn;
    // Start is called before the first frame update
    void Start()
    {
        numberActive = 1;
    }

    /// <summary>
    /// mở thêm point mới
    /// </summary>
    public void OpenNewPoint()
    {
        GameObject point = EventSystem.current.currentSelectedGameObject;
        //EventSystem.current.SetSelectedGameObject(point);
        if (point.GetComponent<Point>().aroundPoints.Count(p => p.GetComponent<Point>().block == false) > 0)
        {
            UnlockNewPoint(point);
        }
    }

    /// <summary>
    /// mở point mới
    /// </summary>
    /// <param name="point"></param>
    private void UnlockNewPoint(GameObject point)
    {
        if (point.GetComponent<Image>().color != Color.white)
        {
            float costOfNextPoint = (float)CalculateCost();
            if (GameManager.instance.money >= costOfNextPoint)
            {
                if (point.GetComponent<Point>().block == true)
                {
                    buyPopup.SetActive(true);
                    costText.text = "Giá: " + costOfNextPoint + "$";

                    yesBtn.onClick.AddListener(() =>
                    {
                        if (point.GetComponent<Point>().block)
                        {
                            buyPopup.SetActive(false);
                            GameManager.instance.money -= costOfNextPoint;
                            point.GetComponent<Point>().enabled = true;
                            point.GetComponent<Image>().color = Color.white;
                            point.GetComponent<Point>().block = false;
                            point.GetComponent<Point>().canCollect = true;
                            point.GetComponent<Point>().startSpread = false;
                            SetPropertiesForNewPoint(point);
                            point.GetComponent<Button>().onClick.RemoveAllListeners();
                            //Gán sự kiện click vào point mới
                            point.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                pControl.OnClickPoint(point);
                            });
                            //genMap.AddAllAroundPoints();
                            LinkedHorizontal();
                            LinkedVertical();
                            GameManager.instance.moneyText.text = "$: " + Math.Round(GameManager.instance.money, 3).ToString();
                        }
                    });

                    noBtn.onClick.AddListener(() =>
                    {
                        yesBtn.onClick.RemoveAllListeners();
                        buyPopup.SetActive(false);
                    });
                    return;
                }
                return;
            }
            else
            {
                okPopup.SetActive(true);
                costTextOkPopup.text = "Cần " + (float)CalculateCost() + "$";
                okBtn.onClick.AddListener(() =>
                {
                    okPopup.SetActive(false);
                });
                return;
            }
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// tạo các thông số của point mới
    /// </summary>
    /// <param name="temp"></param>
    private void SetPropertiesForNewPoint(GameObject temp)
    {
        if (temp.GetComponent<Point>().block == false)
        {
            numberActive = pControl.points.Count(p => p.GetComponent<Point>().block == false) - 1;
            PointInfo pInf = temp.GetComponent<Point>().pInfo;

            pInf.tgClick = (float)Math.Pow(tgInc, numberActive);
            pInf.prcClick = (float)Math.Pow(proInc, numberActive);
            pInf.proClick = (float)Math.Pow(proInc, numberActive);

            pInf.tgPro = (float)Math.Pow(tgInc, numberActive);
            pInf.prcPro = (float)Math.Pow(proInc, numberActive);
            pInf.proPro = (float)Math.Pow(proInc, numberActive);

            temp.GetComponent<Button>().onClick.RemoveListener(() =>
            {
                OpenNewPoint();
            });
        }
    }

    /// <summary>
    /// tính toán giá tiền của point
    /// </summary>
    /// <returns></returns>
    private float CalculateCost()
    {
        numberActive = pControl.points.Count(p => p.GetComponent<Point>().block == false) - 1;
        var cost = inc1 * (float)Math.Pow(inc2, numberActive);
        return cost;
    }

    /// <summary>
    /// thể hiện đường nối với point liền kề vừa mua
    /// </summary>
    private void LinkedHorizontal()
    {
        for (int i = 0; i < pControl.points.Count - 1; i++)
        {
            if (!Enumerable.Range(pControl.points.Count - genMap.columnAmount, pControl.points.Count - 2).Contains(i))
            {
                if (i % genMap.columnAmount != (genMap.columnAmount - 1))
                {
                    if (pControl.points[i].GetComponent<RectTransform>().rect.x == pControl.points[i + 1].GetComponent<RectTransform>().rect.x)
                    {
                        if (pControl.points[i].GetComponent<Point>().block == false && pControl.points[i + 1].GetComponent<Point>().block == false)
                        {
                            pControl.points[i].transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.cyan;
                        }
                    }
                }
            }
            else if (Enumerable.Range(pControl.points.Count - genMap.columnAmount, pControl.points.Count - 2).Contains(i))
            {
                if (pControl.points[i].GetComponent<Point>().block == false && pControl.points[i + 1].GetComponent<Point>().block == false)
                {
                    pControl.points[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                }
            }
        }
    }

    /// <summary>
    /// thể hiện đường nối với point liền kề vừa mua
    /// </summary>
    private void LinkedVertical()
    {
        for (int i = 0; i < pControl.points.Count - genMap.columnAmount; i++) // ko tính hàng cuối cùng
        {
            if (pControl.points[i].GetComponent<Point>().block == false && pControl.points[i + genMap.columnAmount].GetComponent<Point>().block == false)
            {
                pControl.points[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
            }
        }
    }
}
