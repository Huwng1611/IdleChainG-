using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Point : MonoBehaviour
{
    /// <summary>
    /// check xem có phải point đầu tiên sinh ra ko
    /// </summary>
    public bool isFirstPoint;
    /// <summary>
    /// = true => point đang bị khóa; = false => đã mở khóa
    /// </summary>
    public bool block;
    /// <summary>
    /// giá tiền để mua point tiếp theo
    /// </summary>
    public float cost;

    public PointInfo pInfo;
    private void Start()
    {
        pInfo.thisPoint = this.gameObject;
        //Invoke("AutoGetMoney", 2f);
        InvokeRepeating("AutoGetMoney", 2f, pInfo.tgPro);
    }

    private void OnEnable()
    {
        //Debug.Log("<b>point enable.</b>");
        //if (this.isFirstPoint == false)
        //{
        //    //InvokeRepeating("AutoGetMoney", 4f, pInfo.tgPro);
        //}
    }

    private void Update()
    {
        //if (this.block == false)
        //{
        //    InvokeRepeating("AutoGetMoney", 0f, pInfo.tgPro);
        //}
    }

    /// <summary>
    /// tự động kiếm tiền ko cần click
    /// </summary>
    private void AutoGetMoney()
    {
        GameManager.instance.money += pInfo.proPro;
    }
}

[Serializable]
public class PointInfo
{
    public GameObject thisPoint;

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

    #region Upgrade_Autoclick_Variable
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
