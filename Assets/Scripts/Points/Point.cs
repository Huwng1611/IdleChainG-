using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    /// <summary>
    /// click lần 1 => hiển thị các nút upgrade
    /// click lần 2 => lấy tiền về
    /// </summary>
    public int clickCount = 0;

    /// <summary>
    /// = true => point đang bị khóa; = false => đã mở khóa
    /// </summary>
    public bool block;

    /// <summary>
    /// check point lan ra được duyệt hay chưa
    /// </summary>
    public bool startSpread;

    /// <summary>
    /// check xem point đang ở trạng thái có thể click kiếm tiền hay ko
    /// </summary>
    public bool canCollect;

    /// <summary>
    /// duyệt qua rồi thì ko duyệt lại nữa
    /// </summary>
    public bool pChecked;

    /// <summary>
    /// điểm lan thứ n
    /// </summary>
    public int spreadIndex;

    /// <summary>
    /// những point kề với point đang xét
    /// </summary>
    public List<GameObject> aroundPoints;

    /// <summary>
    /// hình tròn chạy lan từ point này tới point kế tiêp
    /// </summary>
    public GameObject circle;

    public List<GameObject> cirlces;
    public PointsController pControl;

    public PointInfo pInfo;
    private void Start()
    {
        startSpread = false;

        //InvokeRepeating("AutoGetMoney", 2f, pInfo.tgPro);
        SpawnCircle();
        pControl = FindObjectOfType<PointsController>();
    }

    private void Update()
    {
        CheckSpreading();
    }

    /// <summary>
    /// tự động kiếm tiền ko cần click
    /// </summary>
    private void AutoGetMoney()
    {
        GameManager.instance.money += pInfo.proPro;
    }

    /// <summary>
    /// sinh ra số điểm lan tương ứng số point kề point hiện tại
    /// </summary>
    private void SpawnCircle()
    {
        for (int i = 0; i < aroundPoints.Count; i++)
        {
            GameObject temp = Instantiate(circle, this.transform);
            temp.name = "circle" + i;
            temp.GetComponent<Image>().color = new Color32(83, 127, 224, 0);
            cirlces.Add(temp);
        }
    }

    private void CheckSpreading()
    {
        if (startSpread && !pChecked)
        {
            Spreading();
            if (pControl.activePoints.Count > 1)
            {
                GameManager.instance.ComboMoney(this.gameObject);
                GameManager.instance.ComboAllPoint(this.gameObject);
            }
        }
    }

    /// <summary>
    /// lan ra các point xung quanh
    /// </summary>
    public void Spreading()
    {
        if (!block && !pChecked)
        {
            for (int i = 0; i < aroundPoints.Count; i++)
            {
                if (aroundPoints[i].GetComponent<Point>().block == false
                    && aroundPoints[i].GetComponent<Point>().pChecked == false)
                {
                    StartCoroutine(FillFullLine(this.gameObject, aroundPoints[i], cirlces[i]));
                }
            }
            pChecked = true;
        }
    }

    /// <summary>
    /// lan ra các point tiếp theo
    /// </summary>
    /// <param name="startPos">point khởi đầu</param>
    /// <param name="endPos">point đích</param>
    /// <returns></returns>
    public IEnumerator FillFullLine(GameObject startPos, GameObject endPos, GameObject circle)
    {
        float time = 0f;
        circle.GetComponent<Image>().color = new Color32(83, 127, 224, 255);
        circle.GetComponent<CircleProperties>().circleValue = spreadIndex;
        while (time < 2f)
        {
            time += Time.deltaTime * 2f;
            circle.transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, time);
            yield return null;
        }
        endPos.GetComponent<Point>().startSpread = true;
        endPos.GetComponent<Point>().spreadIndex = spreadIndex + 1;
        foreach (var c in endPos.GetComponentsInChildren<CircleProperties>())
        {
            c.circleValue = endPos.GetComponent<Point>().spreadIndex;
        }
        circle.GetComponent<Image>().color = new Color32(83, 127, 224, 0);
    }
}

/// <summary>
/// các thuộc tính của 1 point
/// </summary>
[Serializable]
public class PointInfo
{
    #region Click_Variables
    /// <summary>
    /// thời gian cho phép giữa 2 cú click liên tiếp
    /// </summary>
    public float tgClick;
    /// <summary>
    /// giá tiền để nâng cấp click => kiếm nhiều tiền hơn sau mỗi click
    /// </summary>
    public float prcClick;
    /// <summary>
    /// số tiền kiếm được sau 1 cú click
    /// </summary>
    public float proClick;
    #endregion

    #region Autoclick_Variables
    /// <summary>
    /// Thời gian cho phép giữa 2 cú auto click
    /// </summary>
    public float tgPro;
    /// <summary>
    /// giá tiền nâng cấp click => kiếm nhiều tiền hơn sau mỗi cú auto click
    /// </summary>
    public float prcPro;
    /// <summary>
    /// số tiền kiếm được sau 1 cú auto click
    /// </summary>
    public float proPro;
    #endregion
}
