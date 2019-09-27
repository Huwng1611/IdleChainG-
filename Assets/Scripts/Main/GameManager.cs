using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

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
    }

    /// <summary>
    /// nhân combo tiền khi có point lan ra dựa vào vị trí point
    /// </summary>
    /// <param name="point">point hiện tại</param>
    /// <param name="index">vị trí của point</param>
    public void ComboMoney(GameObject point, int index)
    {
        money += point.GetComponent<Point>().pInfo.proClick * (1 + (index + 1) * 0.3f);
    }

    /// <summary>
    /// khi tất cả point đã được mở
    /// </summary>
    /// <param name="point"></param>
    public void ComboAllPoint(GameObject point)
    {
        if (pControl.activePoints.Count == pControl.points.Count)
        {
            effectCombo.Play();
            money += tCombo * point.GetComponent<Point>().pInfo.proClick * (1 + ((pControl.activePoints.Count - 1) + 1) * 0.3f);
        }
    }
}
