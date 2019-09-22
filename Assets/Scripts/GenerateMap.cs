using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GenerateMap : MonoBehaviour
{
    public Transform parentMap;
    public GameObject point;
    public GameObject line;

    //public List<GameObject> GameManager.instance.points;
    public int freeFirstPoint;
    public int pointAmount = 16;
    public int columnAmount;
    public int rowAmount;

    // Start is called before the first frame update
    private void Start()
    {
        parentMap.GetComponent<GridLayoutGroup>().constraintCount = columnAmount;
        GeneratePointOnMap();
    }

    private void Update()
    {

    }

    private void GetAFreePoint()
    {
        freeFirstPoint = Random.Range(GameManager.instance.points.Count - 1, GameManager.instance.points.IndexOf(GameManager.instance.points[0]));
        GameManager.instance.points[freeFirstPoint].GetComponent<Image>().color = Color.white;
        GameManager.instance.points[freeFirstPoint].GetComponent<Point>().enabled = true;
        GameManager.instance.points[freeFirstPoint].GetComponent<Point>().block = false;
        GameManager.instance.activePoints.Add(GameManager.instance.points[freeFirstPoint]);
    }

    /// <summary>
    /// sinh ra các point để tạo map 7x7
    /// </summary>
    private void GeneratePointOnMap()
    {
        int pointCount = 0;
        while (pointCount < pointAmount)
        {
            GameObject obj = Instantiate(point, parentMap);
            obj.name = pointCount.ToString();
            obj.GetComponent<Point>().block = true;
            obj.GetComponent<Point>().enabled = false;
            GameManager.instance.points.Add(obj);
            pointCount++;
        }
        GetAFreePoint();
        AddAllAroundPoints();
        //AddAroundPointOfCurrentPoint();

        GameManager.instance.AddEventForPoint(GameManager.instance.points);
        GameManager.instance.DefaultValueFirstPoint(GameManager.instance.points[freeFirstPoint]);
        StartCoroutine(DrawLineMap());
        Debug.Log("<color=green> Map Generated! </color>");
    }

    /// <summary>
    /// Tạo đường nối các GameManager.instance.points
    /// </summary>
    /// <returns></returns>
    private IEnumerator DrawLineMap()
    {
        for (int i = 0; i < GameManager.instance.points.Count; i++)
        {
            for (int j = 0; j < GameManager.instance.points.Count; j++)
            {
                if (GameManager.instance.points[i].GetComponent<RectTransform>().rect.x == GameManager.instance.points[j].GetComponent<RectTransform>().rect.x)
                {
                    if (GameManager.instance.points[i].transform.childCount < 2)
                    {
                        GameObject ln = Instantiate(line, GameManager.instance.points[i].gameObject.transform);
                        Vector3 temp;
                        temp = ln.transform.position;
                        temp.y -= 100;
                        ln.transform.position = temp;
                    }
                }
                if (GameManager.instance.points[i].GetComponent<RectTransform>().rect.y == GameManager.instance.points[j].GetComponent<RectTransform>().rect.y)
                {
                    if (GameManager.instance.points[i].transform.childCount < 2)
                    {
                        GameObject ln = Instantiate(line, GameManager.instance.points[i].gameObject.transform.position, Quaternion.Euler(0f, 0f, 90f), GameManager.instance.points[i].transform);
                        Vector3 temp;
                        temp = ln.transform.position;
                        temp.x += 100;
                        ln.transform.position = temp;
                        if (Enumerable.Range(GameManager.instance.points.Count - columnAmount, GameManager.instance.points.Count - 1).Contains(i))
                        {
                            Destroy(GameManager.instance.points[i].transform.GetChild(0).gameObject);
                        }
                        if ((i + 1) % columnAmount == 0)
                        {
                            Destroy(GameManager.instance.points[i].transform.GetChild(1).gameObject);
                        }
                    }
                }
            }
        }
        yield return 0;
    }

    public void AddAllAroundPoints()
    {
        for (int i = 0; i < GameManager.instance.points.Count; i++)
        {
            if (GameManager.instance.points[i].GetComponent<Point>().block == false)
            {
                //point góc trái hàng đầu
                if (i == 0)
                {
                    if (!GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + 1])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + columnAmount]))
                    {
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + columnAmount]);
                    }
                }
                //point góc phải hàng đầu
                else if (i == columnAmount - 1)
                {
                    if (!GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - 1])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + columnAmount]))
                    {
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + columnAmount]);
                    }
                }
                //point góc trái hàng cuối
                else if (i == GameManager.instance.points.Count - columnAmount)
                {
                    if (!GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + 1])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - columnAmount]))
                    {
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - columnAmount]);
                    }
                }
                //ponint góc phải hàng cuối
                else if (i == GameManager.instance.points.Count - 1)
                {
                    if (!GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - 1])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - columnAmount]))
                    {
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - columnAmount]);
                    }
                }
                //các points hàng đầu tiên (trừ 2 point đầu mút)
                else if (Enumerable.Range(1, columnAmount - 1 - 1).Contains(i))
                {
                    if (!GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - 1])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + 1])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + columnAmount]))
                    {
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + columnAmount]);
                    }
                }
                //các point hàng cuối cùng (trừ 2 point đầu mút)
                else if (Enumerable.Range(GameManager.instance.points.Count - columnAmount + 1, GameManager.instance.points.Count - 1 - 1).Contains(i))
                {
                    if (!GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - 1])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + 1])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - columnAmount]))
                    {
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - columnAmount]);
                    }
                }
                //các point cột đầu (trừ 2 point đầu mút)
                else if (i != 0 && i % columnAmount == 0)
                {
                    if (!GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + 1])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - columnAmount])
                        && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + columnAmount]))
                    {
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - columnAmount]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + columnAmount]);
                    }
                }
                //các point cột cuối (trừ 2 point đàu mút)
                else if (i != (columnAmount - 1) && i != GameManager.instance.points.Count - 1 && i % columnAmount == (columnAmount - 1))
                {
                    if (!GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - 1])
                       && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - columnAmount])
                       && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + columnAmount]))
                    {
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - columnAmount]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + columnAmount]);
                    }
                }
                //các point nằm giữa
                else
                {
                    if (!GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - 1])
                       && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + 1])
                       && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i - columnAmount])
                       && !GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Contains(GameManager.instance.points[i + columnAmount]))
                    {
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + 1]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i - columnAmount]);
                        GameManager.instance.points[i].GetComponent<Point>().aroundPoints.Add(GameManager.instance.points[i + columnAmount]);
                    }
                }
            }
        }
    }
}
