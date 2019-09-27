using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;

public class Point : MonoBehaviour
{
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
    /// những point kề với point đang xét
    /// </summary>
    public List<GameObject> aroundPoints;

    /// <summary>
    /// số thứ tự hàng của point trong mảng
    /// </summary>
    public int row;

    /// <summary>
    /// số thứ tự cột của point trong mảng
    /// </summary>
    public int col;

    public GameObject[,] aroundP = new GameObject[3, 3];

    /// <summary>
    /// hình tròn chạy lan từ point này tới point kế tiêp
    /// </summary>
    public GameObject circle;

    public PointInfo pInfo;
    private void Start()
    {
        startSpread = false;
        aroundP[1, 1] = this.gameObject;

        InvokeRepeating("AutoGetMoney", 2f, pInfo.tgPro);
        circle = gameObject.transform.GetChild(transform.childCount - 1).gameObject;
        GetIndexOfPoint();
    }

    private void Update()
    {
        if (startSpread)
        {
            //StartCoroutine(Spreading());
            Spreading();
        }
    }

    /// <summary>
    /// lấy số hàng, cột của point trong mảng
    /// </summary>
    private void GetIndexOfPoint()
    {
        string pointName = this.gameObject.name;
        string[] pName = pointName.Split(new char[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries);
        row = Convert.ToInt32(pName[0]);
        col = Convert.ToInt32(pName[1]);
    }

    /// <summary>
    /// tự động kiếm tiền ko cần click
    /// </summary>
    private void AutoGetMoney()
    {
        GameManager.instance.money += pInfo.proPro;
    }

    /// <summary>
    /// bắt đầu lan
    /// </summary>
    private void Spreading()
    {
        for (int i = 0; i < aroundPoints.Count; i++)
        {
            if (aroundPoints[i].GetComponent<Point>().block == false
                && aroundPoints[i].GetComponent<Point>().canCollect == true
                && aroundPoints[i].GetComponent<Point>().pChecked == false)
            {
                StartCoroutine(FillFullLine(this.gameObject, aroundPoints[i]));
                //yield return new WaitForSeconds(2f);
                Debug.Log("<b><color=blue>Spreading....</color></b>");
            }
        }
        pChecked = true;
        startSpread = false;
    }

    /// <summary>
    /// lan ra các point tiếp theo
    /// </summary>
    /// <param name="startPos">point khỏi đầu</param>
    /// <param name="endPos">point đích</param>
    /// <returns></returns>
    public IEnumerator FillFullLine(GameObject startPos, GameObject endPos)
    {
        float time = 0f;
        circle.SetActive(true);
        while (time < 2f)
        {
            time += Time.deltaTime * 2f;
            circle.transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, time);
            yield return null;
        }
        endPos.GetComponent<Point>().startSpread = true;
        yield return new WaitForSeconds(0.2f);
        circle.SetActive(false);
        //yield break;
    }
}

/// <summary>
/// các thuộc tính của 1 point
/// </summary>
[Serializable]
public class PointInfo
{
    #region Upgrade_Click_Variables
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

    #region Upgrade_Autoclick_Variables
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
