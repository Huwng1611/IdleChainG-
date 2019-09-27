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
    public GameObject[,] pArray;

    [Header("Upgrade Buttons")]
    public GameObject groupButtons;

    // Start is called before the first frame update
    void Start()
    {
        ResetCheckPoint();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool CheckAllPoints()
    {
        bool check = activePoints.TrueForAll(p => p.GetComponent<Point>().pChecked == true);
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
            }
        }
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

    /// <summary>
    /// Sự kiện click point
    /// </summary>
    /// <param name="point">point được click</param>
    public void OnClickPoint(GameObject point)
    {
        groupButtons.SetActive(true);
        nextLv.ShowInfoNextLevel(point);
        if (point.GetComponent<Point>().canCollect)
        {
            point.GetComponent<Point>().startSpread = true;
            Debug.Log("<b>Point can collec </b>");
            StartCoroutine(CoolDownToNextClick(point));
            ClickGetMoney(point);
        }
        if (activePoints.Count > 1)
        {
            GameManager.instance.ComboMoney(point, activePoints.IndexOf(point));
        }
        GameManager.instance.ComboAllPoint(point);
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
        point.GetComponent<Point>().canCollect = false;
        point.GetComponent<Image>().color = Color.black;
        yield return new WaitForSeconds(pInf.tgClick);
        point.GetComponent<Point>().canCollect = true;
        //point.GetComponent<Point>().startSpread = false;
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
