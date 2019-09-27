using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuyNewPoint : MonoBehaviour
{
    [Header("Number of actived point")]
    /// <summary>
    /// số các point đã mở, dùng để tính giá mở point tiếp theo
    /// </summary>
    public double numberActive;

    public GenerateMap genMap;
    public PointsController pControl;
    // Start is called before the first frame update
    void Start()
    {
        numberActive = 1;
    }

    // Update is called once per frame
    void Update()
    {
        OpenNewPoint(pControl.points);
    }

    /// <summary>
    /// mở point mới
    /// </summary>
    /// <param name="points"></param>
    public void OpenNewPoint(List<GameObject> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].GetComponent<Point>().block == false) //điều kiện để chỉ được mở point có nối với nhau
            {
                //điểm đầu 
                if (i == 0)
                {
                    if (points[i + 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + 1];
                        points[i + 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (points[i + genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + genMap.columnAmount];
                        points[i + genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                }
                //điểm cuối 
                else if (i == points.Count - 1)
                {
                    if (points[i - 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - 1];
                        points[i - 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (points[i - genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - genMap.columnAmount];
                        points[i - genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                }
                //point phải hàng đầu
                else if (i == genMap.columnAmount - 1)
                {
                    if (points[i - 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - 1];
                        points[i - 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (points[i + genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + genMap.columnAmount];
                        points[i + genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                }
                //point trái hàng cuối
                else if (i == points.Count - genMap.columnAmount)
                {
                    if (points[i + 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + 1];
                        points[i + 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (points[i - genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - genMap.columnAmount];
                        points[i - genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                }
                //hàng đầu tiên trừ 2 point đầu mút
                else if (Enumerable.Range(1, genMap.columnAmount - 1 - 1).Contains(i))
                {
                    if (points[i + genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + genMap.columnAmount];
                        points[i + genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                    if (points[i + 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + 1];
                        points[i + 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (i != 0 && points[i - 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - 1];
                        points[i - 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                }
                //hàng cuối cùng trừ 2 point đầu mút
                else if (Enumerable.Range(points.Count - genMap.columnAmount + 1, points.Count - 1 - 1).Contains(i))
                {
                    if (points[i - 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - 1];
                        points[i - 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (points[i - genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - genMap.columnAmount];
                        points[i - genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                    if (points[i + 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + 1];
                        points[i + 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                }
                //cột đầu trừ 2 point đầu mút
                else if (i != 0 && i != points.Count - genMap.columnAmount && i % genMap.columnAmount == 0)
                {
                    if (points[i + 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + 1];
                        points[i + 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (points[i + genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + genMap.columnAmount];
                        points[i + genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                    if (points[i - genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - genMap.columnAmount];
                        points[i - genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                }
                //cột cuối trừ 2 point đầu mút
                else if (i != genMap.columnAmount - 1 && i != points.Count - 1 && i % genMap.columnAmount == genMap.columnAmount - 1)
                {
                    if (points[i - 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - 1];
                        points[i - 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (points[i + genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + genMap.columnAmount];
                        points[i + genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                    if (points[i - genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - genMap.columnAmount];
                        points[i - genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                }
                //các điểm nằm giữa
                else
                {
                    if (points[i - 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - 1];
                        points[i - 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (points[i + 1].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + 1];
                        points[i + 1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedHorizontal();
                        }
                        );
                    }
                    if (points[i - genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - genMap.columnAmount];
                        points[i - genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                    if (points[i + genMap.columnAmount].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + genMap.columnAmount];
                        points[i + genMap.columnAmount].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                }
            }
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
            if (GameManager.instance.money > costOfNextPoint)
            {
                float tempMoney = GameManager.instance.money;
                GameManager.instance.money = tempMoney - costOfNextPoint;
                point.GetComponent<Point>().enabled = true;
                point.GetComponent<Point>().block = false;
                point.GetComponent<Point>().canCollect = true;
                point.GetComponent<Point>().startSpread = false;
                point.GetComponent<Image>().color = Color.white;
                genMap.AddAllAroundPoints();
                //genMap.AddAroundPointOfCurrentPoint();

                //Gán sự kiện click kiếm tiền vào point mới
                point.GetComponent<Button>().onClick.AddListener(() =>
                {
                    pControl.OnClickPoint(point);
                });
                return;
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// thêm point vừa mở vào 1 list chứa các point đã mở
    /// </summary>
    /// <param name="temp"></param>
    private void AddPointToListActived(GameObject temp)
    {
        if (!pControl.activePoints.Contains(temp))
        {
            pControl.activePoints.Add(temp);
            int index = pControl.activePoints.IndexOf(temp);
            PointInfo pInf = temp.GetComponent<Point>().pInfo;

            pInf.tgClick = (float)Math.Pow(2, index);
            pInf.prcClick = (float)Math.Pow(10, index);
            pInf.proClick = (float)Math.Pow(10, index);

            pInf.tgPro = (float)Math.Pow(2, index);
            pInf.prcPro = (float)Math.Pow(10, index);
            pInf.proPro = (float)Math.Pow(10, index);

            //CombopControl.money(temp, pControl.activePoints.IndexOf(temp));
        }
    }

    /// <summary>
    /// tính toán giá tiền của point
    /// </summary>
    /// <returns></returns>
    private double CalculateCost()
    {
        numberActive = (double)(pControl.activePoints.Count + 1);
        //var cost = 50 * Math.Pow(50, numberActive);
        var cost = 1 * Math.Pow(1, numberActive); //giá tạm để test
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
