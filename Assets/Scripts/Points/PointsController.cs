using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    public List<GameObject> points;
    public List<GameObject> activePoints;

    public NextLevelInfor nextLv;
    public ParticleSystem effectCombo;

    public GameObject objTemp;


    [Header("Upgrade Buttons")]
    public GameObject groupButtons;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ResetCheckPoint();
    }

    /// <summary>
    /// check toàn bộ point đã mở
    /// </summary>
    /// <returns></returns>
    private bool CheckAllPoints()
    {
        bool check = activePoints.TrueForAll(p => p.GetComponent<Point>().pChecked == true
        && p.GetComponent<Point>().startSpread == true);
        return check;
    }

    /// <summary>
    /// reset lại biến pChecked của point
    /// </summary>
    private void ResetCheckPoint()
    {
        if (CheckAllPoints() == true)
        {
            foreach (var p in activePoints)
            {
                p.GetComponent<Point>().pChecked = false;
                p.GetComponent<Point>().startSpread = false;
            }
        }
    }

    /// <summary>
    /// Đặt giá trị chấm tròn lan của point được bấm = 1
    /// </summary>
    /// <param name="p"></param>
    private void SetCircleValue(Point p)
    {
        foreach (var cValue in p.GetComponentsInChildren<CircleProperties>())
        {
            cValue.circleValue = 1;
        }
    }

    /// <summary>
    /// click lần đầu mở nút upgrade
    /// </summary>
    private void FirstClick(Point p)
    {
        groupButtons.SetActive(true);
        nextLv.ShowInfoNextLevel(p.gameObject);
        p.clickCount = 1;
        Debug.Log("<i><color=orange>1st click</color></i>");
    }

    /// <summary>
    /// click lần 2 thì thu tiền về
    /// </summary>
    private void SecondClick(Point p)
    {
        if (p.canCollect)
        {
            Debug.Log("<b>Point can collect </b>");
            StartCoroutine(CoolDownToNextClick(p.gameObject));
            ClickGetMoney(p.gameObject);
            p.startSpread = true;
            p.spreadIndex = 1;
            SetCircleValue(p);
        }
        p.clickCount = 0;
        Debug.Log("<i><color=orange>2nd click</color></i>");
    }

    /// <summary>
    /// Sự kiện click point
    /// </summary>
    /// <param name="point">point được click</param>
    public void OnClickPoint(GameObject point)
    {
        if (point.GetComponent<Point>().clickCount == 0 && point.GetComponent<Point>().block == false)
        {
            FirstClick(point.GetComponent<Point>());
            return;
        }
        if (point.GetComponent<Point>().clickCount == 1 && point.GetComponent<Image>().color == Color.white)
        {
            SecondClick(point.GetComponent<Point>());
            return;
        }
    }

    /// <summary>
    /// Đếm ngược thời gian clickable 1 point
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private IEnumerator CoolDownToNextClick(GameObject point)
    {
        PointInfo pInf = point.GetComponent<Point>().pInfo;
        point.GetComponent<Point>().canCollect = false;
        point.GetComponent<Image>().color = Color.black;
        yield return new WaitForSeconds(pInf.tgClick);
        point.GetComponent<Point>().canCollect = true;
        point.GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// click point lấy tiền
    /// </summary>
    /// <param name="point"></param>
    private void ClickGetMoney(GameObject point)
    {
        PointInfo pInf = point.GetComponent<Point>().pInfo;
        GameManager.instance.money += pInf.proClick;
        GameManager.instance.moneyText.text = "$: " + GameManager.instance.money;
        point = EventSystem.current.currentSelectedGameObject;
        objTemp = point;
    }
}
