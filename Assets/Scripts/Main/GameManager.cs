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
    public PointsController pControl;

    public ParticleSystem effectCombo;

    /// <summary>
    /// tCombo khi mở tất cả các point
    /// </summary>
    public float tCombo = 2f;

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

    /// <summary>
    /// nhân combo tiền khi có point lan ra dựa vào vị trí point
    /// </summary>
    /// <param name="point">point hiện tại</param>
    public void ComboMoney(GameObject point)
    {
        money += point.GetComponent<Point>().pInfo.proClick * (1 + point.GetComponent<Point>().spreadIndex * 0.3f);
    }

    /// <summary>
    /// khi tất cả point đã được mở
    /// </summary>
    /// <param name="point"></param>
    public void ComboAllPoint(GameObject point)
    {
        if (pControl.points.Count(p => p.GetComponent<Point>().block == false) == pControl.points.Count)
        {
            effectCombo.Play();
            money += tCombo * point.GetComponent<Point>().pInfo.proClick * (1 + point.GetComponent<Point>().spreadIndex * 0.3f);
        }
    }
}
