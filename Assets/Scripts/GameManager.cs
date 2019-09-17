using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float money = 0;
    public Text moneyText;

    public GameObject objTemp;

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
        moneyText.text += money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$: " + money.ToString();
    }

    public void ClickToCollectMoney()
    {

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

    /// <summary>
    /// Sự kiện click point
    /// </summary>
    /// <param name="point">point được click</param>
    private void OnClickPoint(GameObject point)
    {
        groupButtons.SetActive(true);
        StartCoroutine(CoolDownToNextClick(point));
        ClickGetMoney(point);
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
        //int.TryParse(EventSystem.current.currentSelectedGameObject.name, out indexTemp);
        point = objTemp;
        point.GetComponent<Point>();
        PointInfo pInf = point.GetComponent<Point>().pInfo;
        if (money > pInf.prcClick)
        {
            money -= pInf.prcClick;
            pInf.tgClick /= coolDownUpgrade;
            pInf.prcClick *= priceUpgrade;
            pInf.proClick *= moneyEarnedUpgrade;
            groupButtons.SetActive(false);
        }
    }

    /// <summary>
    /// upgrade auto click
    /// </summary>
    public void AfterUpgradeProduction()
    {
        groupButtons.SetActive(false);
    }
}
