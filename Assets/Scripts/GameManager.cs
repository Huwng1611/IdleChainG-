using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float money = 0;
    public Text moneyText;

    public GameObject objTemp;
    public List<GameObject> points;
    public List<GameObject> activePoints;

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
    /// số các point đã mở, dùng để tính giá mở point tiếp theo
    /// </summary>
    public double numberActive;

    [Header("Upgrade Buttons")]
    public GameObject groupButtons;
    public Button click;
    public Button production;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        moneyText.text = "$: " + money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (money < 0)
        {
            money = 0;
        }
        else
        {
            moneyText.text = "$: " + Math.Round(money, 3).ToString();
        }
        //BuyNewPoint(points);
        OpenPointIfEnoughMoney();
    }

    private void OpenPointIfEnoughMoney()
    {
        BuyNewPoint(points);
        //money -= (50 * Mathf.Pow(50, numberActive));
    }

    public void DefaultValueFirstPoint(GameObject firstPoint)
    {
        PointInfo pInf = firstPoint.GetComponent<Point>().pInfo;

        pInf.tgClick = 2f;
        pInf.prcClick = 10f;
        pInf.proClick = 1f;

        pInf.tgPro = 2f;
        pInf.prcPro = 10f;
        pInf.proPro = 1f;
    }

    /// <summary>
    /// Gán sự kiện cho point đã mở
    /// </summary>
    /// <param name="points"></param>
    public void AddEventForPoint(List<GameObject> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].GetComponent<Image>().color == Color.white)
            {
                GameObject temp = points[i];
                points[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnClickPoint(temp);
                });
            }
        }
    }

    #region Click
    /// <summary>
    /// Sự kiện click point
    /// </summary>
    /// <param name="point">point được click</param>
    private void OnClickPoint(GameObject point)
    {
        groupButtons.SetActive(true);
        StartCoroutine(CoolDownToNextClick(point));
        ClickGetMoney(point);
        StartCoroutine(PointSuffuse());
        //StartCoroutine(PointSuffuse1111(points));

        if (activePoints.Count > 1)
        {
            ComboMoney(point, activePoints.IndexOf(point));
        }
        Debug.Log("<color=orange>Da bam dc roi</color>");
    }

    /// <summary>
    /// Đếm ngược thời gian clickable 1 point
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private IEnumerator CoolDownToNextClick(GameObject point)
    {
        PointInfo pInf = point.GetComponent<Point>().pInfo;
        point.GetComponent<Button>().enabled = false;
        point.GetComponent<Image>().color = Color.black;
        yield return new WaitForSeconds(pInf.tgClick);
        point.GetComponent<Button>().enabled = true;
        point.GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// click point lấy tiền
    /// </summary>
    /// <param name="point"></param>
    private void ClickGetMoney(GameObject point)
    {
        PointInfo pInf = point.GetComponent<Point>().pInfo;
        money += pInf.proClick;
        moneyText.text = "$: " + money;
        point = EventSystem.current.currentSelectedGameObject;
        objTemp = point;
    }

    /// <summary>
    /// upgrade click
    /// </summary>
    public void AfterUpgradeClick()
    {
        GameObject point;
        //int.TryParse(EventSystem.current.currentSelectedGameObject.name, out indexTemp);
        point = objTemp;
        point.GetComponent<Point>();
        PointInfo pInf = point.GetComponent<Point>().pInfo;
        if (money >= pInf.prcClick)
        {
            money -= pInf.prcClick;
            pInf.tgClick /= coolDownUpgrade;
            pInf.prcClick *= priceUpgrade;
            pInf.proClick *= moneyEarnedUpgrade;
            groupButtons.SetActive(false);
        }
    }
    #endregion

    #region Production
    /// <summary>
    /// upgrade auto click
    /// </summary>
    public void AfterUpgradeProduction()
    {
        GameObject point;
        point = objTemp;
        PointInfo pInf = point.GetComponent<Point>().pInfo;
        if (money > pInf.prcPro)
        {
            money -= pInf.prcPro;
            pInf.tgPro /= coolDownUpgrade;
            pInf.prcPro *= priceUpgrade;
            pInf.proPro *= moneyEarnedUpgrade;
            groupButtons.SetActive(false);
        }
    }
    #endregion

    public void BuyNewPoint(List<GameObject> points)
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
                    if (points[i + 7].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + 7];
                        points[i + 7].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                }
                //điểm cuối
                if (i == points.Count - 1)
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
                    if (points[i - 7].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - 7];
                        points[i - 7].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                }
                //hàng đầu tiên
                else if (Enumerable.Range(0, 7).Contains(i))
                {
                    if (points[i + 7].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + 7];
                        points[i + 7].GetComponent<Button>().onClick.AddListener(() =>
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
                //hàng cuối cùng
                else if (Enumerable.Range(42, 48).Contains(i))
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
                    if (points[i - 7].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - 7];
                        points[i - 7].GetComponent<Button>().onClick.AddListener(() =>
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
                    if (points[i - 7].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i - 7];
                        points[i - 7].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            UnlockNewPoint(temp);
                            AddPointToListActived(temp);
                            LinkedVertical();
                        }
                        );
                    }
                    if (points[i + 7].GetComponent<Point>().block == true)
                    {
                        GameObject temp = points[i + 7];
                        points[i + 7].GetComponent<Button>().onClick.AddListener(() =>
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
            if (money > costOfNextPoint)
            {
                float tempMoney = money;
                //point.GetComponent<Point>().cost = (float)CalculateCost();
                money = tempMoney - costOfNextPoint;
                point.GetComponent<Point>().enabled = true;
                point.GetComponent<Point>().block = false;
                point.GetComponent<Image>().color = Color.white;

                //Gán sự kiện click kiếm tiền vào point mới
                point.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnClickPoint(point);
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
    /// thêm point vừa mở vào 1 list chứa các ponint đã mở
    /// </summary>
    /// <param name="temp"></param>
    private void AddPointToListActived(GameObject temp)
    {
        if (!activePoints.Contains(temp))
        {
            activePoints.Add(temp);
            int index = activePoints.IndexOf(temp);
            PointInfo pInf = temp.GetComponent<Point>().pInfo;

            pInf.tgClick = (float)Math.Pow(2, index);
            pInf.prcClick = (float)Math.Pow(10, index);
            pInf.proClick = (float)Math.Pow(10, index);

            pInf.tgPro = (float)Math.Pow(2, index);
            pInf.prcPro = (float)Math.Pow(10, index);
            pInf.proPro = (float)Math.Pow(10, index);

            //ComboMoney(temp, activePoints.IndexOf(temp));
        }
    }

    /// <summary>
    /// tính toán giá tiền của point
    /// </summary>
    /// <returns></returns>
    private double CalculateCost()
    {
        numberActive = (double)activePoints.Count;
        //var cost = 50 * Math.Pow(50, numberActive);
        var cost = 1 * Math.Pow(1, numberActive); //giá tạm để test
        return cost;
    }

    /// <summary>
    /// thể hiện đường nối với point liền kề vừa mua
    /// </summary>
    private void LinkedHorizontal()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (!Enumerable.Range(42, 47).Contains(i)) //loại point 42 - 47
            {
                if (i % 7 != 6)
                {
                    if (points[i].GetComponent<RectTransform>().rect.x == points[i + 1].GetComponent<RectTransform>().rect.x)
                    {
                        if (points[i].GetComponent<Point>().block == false && points[i + 1].GetComponent<Point>().block == false)
                        {
                            points[i].transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.cyan;
                        }
                    }
                }
            }
            else if (Enumerable.Range(42, 47).Contains(i)) //tính point 42 - 47
            {
                if (points[i].GetComponent<Point>().block == false && points[i + 1].GetComponent<Point>().block == false)
                {
                    points[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                }
            }
        }
    }

    /// <summary>
    /// thể hiện đường nối với point liền kề vừa mua
    /// </summary>
    private void LinkedVertical()
    {
        for (int i = 0; i < points.Count - 7; i++) // ko tính hàng cuối cùng
        {
            if (points[i].GetComponent<Point>().block == false && points[i + 7].GetComponent<Point>().block == false)
            {
                points[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
            }
        }
    }

    /// <summary>
    /// click 1 point lan ra những ponint có kết nối
    /// </summary>
    private IEnumerator PointSuffuse()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].GetComponent<Point>().block == false && points[i + 1].GetComponent<Point>().block == false)
            {
                if (points[i].GetComponent<RectTransform>().rect.x == points[i + 1].GetComponent<RectTransform>().rect.x)
                {
                    yield return new WaitForSeconds(0.2f);
                    points[i].transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.red;
                    yield return new WaitForSeconds(points[i].GetComponent<Point>().pInfo.tgClick);
                    points[i].transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.cyan;
                }
            }

            if (!Enumerable.Range(42, 48).Contains(i))
            {
                if (points[i].GetComponent<Point>().block == false && points[i + 7].GetComponent<Point>().block == false)
                {
                    if (points[i].GetComponent<RectTransform>().rect.x == points[i + 7].GetComponent<RectTransform>().rect.x)
                    {
                        yield return new WaitForSeconds(0.2f);
                        points[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
                        yield return new WaitForSeconds(points[i].GetComponent<Point>().pInfo.tgClick);
                        points[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                    }
                }
            }
        }
        yield return 0;
    }


    /// <summary>
    /// nhân combo tiền khi có point lan ra dựa vào vị trí point
    /// </summary>
    /// <param name="point">point hiện tại</param>
    /// <param name="index">vị trí của point</param>
    private void ComboMoney(GameObject point, int index)
    {
        money += point.GetComponent<Point>().pInfo.proClick * (1 + (index + 1) * 0.3f);
    }
}
