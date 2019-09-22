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

    public GenerateMap genMap;
    public NextLevelInfor nextLv;
    public ParticleSystem effectCombo;

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
    /// tCombo khi mở tất cả các point
    /// </summary>
    public float tCombo = 2f;

    [Header("Number of actived point")]
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
        nextLv.ShowInfoNextLevel(point);
        StartCoroutine(CoolDownToNextClick(point));
        ClickGetMoney(point);
        StartCoroutine(CheckAllAroundPoints(point));
        //StartCoroutine(CheckAllAroundPoint1111(point));
        //StartCoroutine(PointSuffuse());

        if (activePoints.Count > 1)
        {
            ComboMoney(point, activePoints.IndexOf(point));
        }
        ComboAllPoint(point);
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
                //hàng đầu tiên
                else if (Enumerable.Range(0, genMap.columnAmount).Contains(i))
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
                //hàng cuối cùng
                else if (Enumerable.Range(points.Count - genMap.columnAmount, points.Count - 1).Contains(i))
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
            if (money > costOfNextPoint)
            {
                float tempMoney = money;
                //point.GetComponent<Point>().cost = (float)CalculateCost();
                money = tempMoney - costOfNextPoint;
                point.GetComponent<Point>().enabled = true;
                point.GetComponent<Point>().block = false;
                point.GetComponent<Image>().color = Color.white;
                genMap.AddAllAroundPoints();
                //genMap.AddAroundPointOfCurrentPoint();
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
        numberActive = (double)(activePoints.Count + 1);
        var cost = 50 * Math.Pow(50, numberActive);
        //var cost = 1 * Math.Pow(1, numberActive); //giá tạm để test
        return cost;
    }

    /// <summary>
    /// thể hiện đường nối với point liền kề vừa mua
    /// </summary>
    private void LinkedHorizontal()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (!Enumerable.Range(points.Count - genMap.columnAmount, points.Count - 2).Contains(i))
            {
                if (i % genMap.columnAmount != (genMap.columnAmount - 1))
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
            else if (Enumerable.Range(points.Count - genMap.columnAmount, points.Count - 2).Contains(i))
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
        for (int i = 0; i < points.Count - genMap.columnAmount; i++) // ko tính hàng cuối cùng
        {
            if (points[i].GetComponent<Point>().block == false && points[i + genMap.columnAmount].GetComponent<Point>().block == false)
            {
                points[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
            }
        }
    }

    //private IEnumerator CheckAllAroundPoint1111(GameObject point)
    //{
    //    //int pNumber = point.GetComponent<Point>().aroundPoints.Count;
    //    //List<GameObject> tempList = point.GetComponent<Point>().aroundPoints;
    //    for (int i = 0; i < points.Count; i++)
    //    {
    //        if (points[i].GetComponent<Point>().block == false)
    //        {
    //            //point đầu góc trái
    //            if (point == points[0])
    //            {
    //                StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[1]));
    //                StartCoroutine(point.transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, points[genMap.columnAmount]));
    //            }
    //            //point đầu góc phải
    //            else if (point == points[genMap.columnAmount - 1])
    //            {
    //                StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[genMap.columnAmount - 1 + genMap.columnAmount]));
    //                StartCoroutine(points[genMap.columnAmount - 1 - 1].transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, points[genMap.columnAmount - 1 - 1]));
    //            }
    //            //point cuối góc trái
    //            else if (point == points[points.Count - genMap.columnAmount])
    //            {
    //                StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[points.Count - genMap.columnAmount + 1]));
    //                StartCoroutine(points[points.Count - genMap.columnAmount - genMap.columnAmount].transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, points[points.Count - genMap.columnAmount - genMap.columnAmount]));
    //            }
    //            //point cuối góc phải
    //            else if (point == points[points.Count - 1])
    //            {
    //                StartCoroutine(points[points.Count - 1 - 1].transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[points.Count - 1 - 1]));
    //                StartCoroutine(points[points.Count - 1 - genMap.columnAmount].transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[points.Count - 1 - genMap.columnAmount]));
    //            }
    //            else
    //            {
    //                //các point giữa
    //                if (point.transform.childCount == genMap.columnAmount)
    //                {
    //                    StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i - 1]));
    //                    StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i + 1]));
    //                    StartCoroutine(point.transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, points[i - genMap.columnAmount]));
    //                    StartCoroutine(point.transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, points[i + genMap.columnAmount]));
    //                }

    //                //point cột trái
    //                if (i != 0 && i % genMap.columnAmount == 0)
    //                {
    //                    StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i + 1]));
    //                    StartCoroutine(points[i - genMap.columnAmount].transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i - genMap.columnAmount]));
    //                    StartCoroutine(point.transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, points[i + genMap.columnAmount]));
    //                }
    //                //point cột phải
    //                else if (i != genMap.columnAmount - 1 && i != points.Count - 1 && i % genMap.columnAmount == genMap.columnAmount - 1)
    //                {
    //                    StartCoroutine(points[i - 1].transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, points[i - 1]));
    //                    StartCoroutine(points[i - genMap.columnAmount].transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i - genMap.columnAmount]));
    //                    StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i + genMap.columnAmount]));
    //                }
    //                //point hàng đầu
    //                else if (Enumerable.Range(1, genMap.columnAmount - 1 - 1).Contains(i))
    //                {
    //                    StartCoroutine(points[i - 1].transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, points[i - 1]));
    //                    StartCoroutine(point.transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, points[i + 1]));
    //                    StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i + genMap.columnAmount]));
    //                }
    //                //point hàng cuối
    //                else if (Enumerable.Range(GameManager.instance.points.Count - genMap.columnAmount + 1, GameManager.instance.points.Count - 1 - 1).Contains(i))
    //                {
    //                    StartCoroutine(points[i - 1].transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i - 1]));
    //                    StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i + 1]));
    //                    StartCoroutine(points[i - genMap.columnAmount].transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, points[i - genMap.columnAmount]));
    //                }
    //            }
    //        }
    //    }
    //    yield return 0;
    //}

    /// <summary>
    /// duyệt các point xung quanh point đang xét
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckAllAroundPoints(GameObject point)
    {
        int pNumber = point.GetComponent<Point>().aroundPoints.Count;
        List<GameObject> tempList = point.GetComponent<Point>().aroundPoints;
        for (int i = 0; i < pNumber; i++)
        {
            //yield return new WaitForEndOfFrame();
            if (/*point.GetComponent<Point>().block == false && */tempList[i].GetComponent<Point>().block == false
               /* && point.GetComponent<Point>().isMarked == false*/ /*&& tempList[i].GetComponent<Point>().isMarked == false*/)
            {
                //lan hàng dọc
                if (point.GetComponent<RectTransform>().position.x == tempList[i].GetComponent<RectTransform>().position.x)
                {
                    yield return new WaitForSeconds(0.2f);
                    if (point.transform.childCount > 0)
                    {
                        //yield return new WaitForSeconds(0.2f);
                        StartCoroutine(point.transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(point, tempList[i]));
                        //point.GetComponent<Point>().isMarked = true;
                        //tempList[i].GetComponent<Point>().isMarked = true;
                    }
                }
                //lan hàng ngang
                //else if (point.GetComponent<RectTransform>().rect.y == tempList[i].GetComponent<RectTransform>().rect.y)
                //{
                //    if (point.GetComponent<RectTransform>().rect.x < tempList[i].GetComponent<RectTransform>().rect.x)
                //    {
                //        StartCoroutine(point.transform.GetChild(1).gameObject.GetComponent<Line>().FillFullLine(point, tempList[i]));
                //    }
                //}
            }
            //yield return new WaitForSeconds(1f);
        }
        yield return 0;
        //point.GetComponent<Point>().isMarked = false;
        //foreach (var tempObj in tempList)
        //{
        //    tempObj.GetComponent<Point>().isMarked = false;
        //}
    }

    /// <summary>
    /// click 1 point lan ra những ponint có kết nối
    /// </summary>
    private IEnumerator PointSuffuse()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].GetComponent<RectTransform>().rect.x == points[i + 1].GetComponent<RectTransform>().rect.x)
            {
                if (points[i].GetComponent<Point>().block == false && points[i + 1].GetComponent<Point>().block == false)
                {

                    yield return new WaitForSeconds(0.2f);
                    //points[i].transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.red;
                    StartCoroutine(points[i].transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(points[i], points[i + 1]));
                    //yield return new WaitForSeconds(points[i].GetComponent<Point>().pInfo.tgClick);
                    //points[i].transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.cyan;
                }
            }

            if (!Enumerable.Range(points.Count - genMap.columnAmount, points.Count - 1).Contains(i))
            {
                if (points[i].GetComponent<Point>().block == false && points[i + genMap.columnAmount].GetComponent<Point>().block == false)
                {
                    if (points[i].GetComponent<RectTransform>().rect.x == points[i + genMap.columnAmount].GetComponent<RectTransform>().rect.x)
                    {
                        yield return new WaitForSeconds(0.2f);
                        //points[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
                        StartCoroutine(points[i].transform.GetChild(0).gameObject.GetComponent<Line>().FillFullLine(points[i], points[i + genMap.columnAmount]));
                        //yield return new WaitForSeconds(points[i].GetComponent<Point>().pInfo.tgClick);
                        //points[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
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

    /// <summary>
    /// khi tất cả point đã được mở
    /// </summary>
    /// <param name="point"></param>
    private void ComboAllPoint(GameObject point)
    {
        if (activePoints.Count == points.Count)
        {
            effectCombo.Play();
            money += tCombo * point.GetComponent<Point>().pInfo.proClick * (1 + ((activePoints.Count - 1) + 1) * 0.3f);
        }
    }
}
