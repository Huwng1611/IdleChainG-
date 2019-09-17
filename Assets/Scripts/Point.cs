using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Point : MonoBehaviour
{
    /// <summary>
    /// vị trí của point cần lấy ra trong list các points
    /// </summary>
    public int index;
    public PointInfo pInfo;
    private void Start()
    {
        pInfo.thisPoint = this.gameObject;
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
