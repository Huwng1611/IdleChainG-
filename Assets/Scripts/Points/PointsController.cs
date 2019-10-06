using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    public List<GameObject> points;

    public NextLevelInfor nextLv;
    public ParticleSystem effectCombo;

    public GameObject objTemp;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ResetCheckPoint", 0f, 2f);
    }

    /// <summary>
    /// reset lại biến pChecked của point
    /// </summary>
    private void ResetCheckPoint()
    {
        foreach (var p in points)
        {
            if (!p.GetComponent<Point>().block && p.GetComponent<Point>().pChecked && p.GetComponent<Point>().startSpread)
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
    /// Sự kiện click point
    /// </summary>
    /// <param name="point">point được click</param>
    public void OnClickPoint(GameObject point)
    {
        Debug.Log(point.name);
        nextLv.ShowInfoNextLevel(point);
        if (point.GetComponent<Point>().canCollect)
        {
            //Debug.Log("<b>Point can collect </b>");
            StartCoroutine(CoolDownToNextClick(point));
            ClickGetMoney(point);
            point.GetComponent<Point>().startSpread = true;
            point.GetComponent<Point>().spreadIndex = 1;
            SetCircleValue(point.GetComponent<Point>());
        }
        GameManager.instance.moneyText.text = "$: " + Math.Round(GameManager.instance.money, 3).ToString();
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
        //point = EventSystem.current.currentSelectedGameObject;
        objTemp = point;
    }
}
