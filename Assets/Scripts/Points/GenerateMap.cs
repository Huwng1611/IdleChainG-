using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GenerateMap : MonoBehaviour
{
    public Transform parentMap;
    public GameObject point;
    public GameObject line;
    public PointsController pControl;
    public BuyNewPoint buyP;

    public int freeFirstPoint;

    public int columnAmount;
    public int rowAmount;

    // Start is called before the first frame update
    private void Start()
    {
        //pControl.pArray = new GameObject[rowAmount, columnAmount];
        parentMap.GetComponent<GridLayoutGroup>().constraintCount = columnAmount;
        CreateMap();
    }

    private void Update()
    {

    }

    private void GetAFreePoint()
    {
        freeFirstPoint = Random.Range(pControl.points.Count - 1, pControl.points.IndexOf(pControl.points[0]));
        pControl.points[freeFirstPoint].GetComponent<Image>().color = Color.white;
        pControl.points[freeFirstPoint].GetComponent<Point>().enabled = true;
        pControl.points[freeFirstPoint].GetComponent<Point>().block = false;
        pControl.points[freeFirstPoint].GetComponent<Point>().canCollect = true;
        pControl.points[freeFirstPoint].GetComponent<Button>().onClick.AddListener(() =>
        {
            pControl.OnClickPoint(pControl.points[freeFirstPoint]);
        });
        pControl.activePoints.Add(pControl.points[freeFirstPoint]);
    }

    private void CreateMap()
    {
        for (int i = 0; i < rowAmount; i++)
        {
            for (int j = 0; j < columnAmount; j++)
            {
                GameObject obj = Instantiate(point, parentMap);
                obj.name = "[" + i + "," + j + "]";
                obj.GetComponent<Point>().block = true;
                obj.GetComponent<Point>().enabled = false;
                obj.GetComponent<Point>().canCollect = false;
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    buyP.OpenNewPoint(pControl.points);
                });
                pControl.points.Add(obj);
            }
        }
        GetAFreePoint();
        AddAllAroundPoints();
        DefaultValueFirstPoint(pControl.points[freeFirstPoint]);
        StartCoroutine(DrawLineMap());

        //foreach (var p in pControl.points)
        //{
        //    p.GetComponent<Button>().onClick.AddListener(() =>
        //    {
        //        buyP.OpenNewPoint(pControl.points);
        //    });
        //}
        Debug.Log("<color=green> New way to gen map! </color>");
    }

    /// <summary>
    /// các giá trị mặc định của point đầu tiên
    /// </summary>
    /// <param name="firstPoint"></param>
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
    /// Tạo đường nối các pControl.points
    /// </summary>
    /// <returns></returns>
    private IEnumerator DrawLineMap()
    {
        // cần sửa lại hàm này để phù hợp cho point lan
        for (int i = 0; i < pControl.points.Count; i++)
        {
            for (int j = 0; j < pControl.points.Count; j++)
            {
                if (pControl.points[i].GetComponent<RectTransform>().rect.x == pControl.points[j].GetComponent<RectTransform>().rect.x)
                {
                    if (pControl.points[i].transform.childCount < 2)
                    {
                        GameObject ln = Instantiate(line, pControl.points[i].gameObject.transform);
                        Vector3 temp;
                        temp = ln.transform.position;
                        temp.y -= 100;
                        ln.transform.position = temp;
                    }
                }
                if (pControl.points[i].GetComponent<RectTransform>().rect.y == pControl.points[j].GetComponent<RectTransform>().rect.y)
                {
                    if (pControl.points[i].transform.childCount < 2)
                    {
                        GameObject ln = Instantiate(line, pControl.points[i].gameObject.transform.position, Quaternion.Euler(0f, 0f, 90f), pControl.points[i].transform);
                        Vector3 temp;
                        temp = ln.transform.position;
                        temp.x += 100;
                        ln.transform.position = temp;
                        if (Enumerable.Range(pControl.points.Count - columnAmount, pControl.points.Count - 1).Contains(i))
                        {
                            Destroy(pControl.points[i].transform.GetChild(0).gameObject);
                        }
                        if ((i + 1) % columnAmount == 0)
                        {
                            Destroy(pControl.points[i].transform.GetChild(1).gameObject);
                        }
                    }
                }
            }
        }
        yield return 0;
    }

    /// <summary>
    /// add các điểm xung quanh của 1 point
    /// </summary>
    public void AddAllAroundPoints()
    {
        for (int i = 0; i < pControl.points.Count; i++)
        {
            if (pControl.points[i].GetComponent<Point>().block == false)
            {
                //point góc trái hàng đầu
                if (i == 0)
                {
                    if (!pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + 1])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + columnAmount]))
                    {
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + columnAmount]);
                    }
                    //pControl.points[i].GetComponent<Point>().aroundP1, 2] = pControl.points[i + 1];
                    //pControl.points[i].GetComponent<Point>().aroundP2, 1] = pControl.points[i + columnAmount];
                }
                //point góc phải hàng đầu
                else if (i == columnAmount - 1)
                {
                    if (!pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - 1])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + columnAmount]))
                    {
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + columnAmount]);
                    }
                    //pControl.points[i].GetComponent<Point>().aroundP1, 0] = pControl.points[i - 1];
                    //pControl.points[i].GetComponent<Point>().aroundP2, 1] = pControl.points[i + columnAmount];
                }
                //point góc trái hàng cuối
                else if (i == pControl.points.Count - columnAmount)
                {
                    if (!pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + 1])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - columnAmount]))
                    {
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - columnAmount]);
                    }
                    //pControl.points[i].GetComponent<Point>().aroundP1, 2] = pControl.points[i + 1];
                    //pControl.points[i].GetComponent<Point>().aroundP0, 1] = pControl.points[i - columnAmount];
                }
                //ponint góc phải hàng cuối
                else if (i == pControl.points.Count - 1)
                {
                    if (!pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - 1])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - columnAmount]))
                    {
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - columnAmount]);
                    }
                    //pControl.points[i].GetComponent<Point>().aroundP1, 0] = pControl.points[i - 1];
                    //pControl.points[i].GetComponent<Point>().aroundP0, 1] = pControl.points[i - columnAmount];
                }
                //các points hàng đầu tiên (trừ 2 point đầu mút)
                else if (Enumerable.Range(1, columnAmount - 1 - 1).Contains(i))
                {
                    if (!pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - 1])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + 1])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + columnAmount]))
                    {
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + columnAmount]);
                    }
                    //pControl.points[i].GetComponent<Point>().aroundP1, 0] = pControl.points[i - 1];
                    //pControl.points[i].GetComponent<Point>().aroundP1, 2] = pControl.points[i + 1];
                    //pControl.points[i].GetComponent<Point>().aroundP2, 1] = pControl.points[i + columnAmount];
                }
                //các point hàng cuối cùng (trừ 2 point đầu mút)
                else if (Enumerable.Range(pControl.points.Count - columnAmount + 1, pControl.points.Count - 1 - 1).Contains(i))
                {
                    if (!pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - 1])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + 1])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - columnAmount]))
                    {
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - columnAmount]);
                    }
                    //pControl.points[i].GetComponent<Point>().aroundP1, 0] = pControl.points[i - 1];
                    //pControl.points[i].GetComponent<Point>().aroundP1, 2] = pControl.points[i + 1];
                    //pControl.points[i].GetComponent<Point>().aroundP0, 1] = pControl.points[i - columnAmount];
                }
                //các point cột đầu (trừ 2 point đầu mút)
                else if (i != 0 && i % columnAmount == 0)
                {
                    if (!pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + 1])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - columnAmount])
                        && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + columnAmount]))
                    {
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - columnAmount]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + columnAmount]);
                    }
                    //pControl.points[i].GetComponent<Point>().aroundP1, 2] = pControl.points[i + 1];
                    //pControl.points[i].GetComponent<Point>().aroundP0, 1] = pControl.points[i - columnAmount];
                    //pControl.points[i].GetComponent<Point>().aroundP2, 1] = pControl.points[i + columnAmount];
                }
                //các point cột cuối (trừ 2 point đàu mút)
                else if (i != (columnAmount - 1) && i != pControl.points.Count - 1 && i % columnAmount == (columnAmount - 1))
                {
                    if (!pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - 1])
                       && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - columnAmount])
                       && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + columnAmount]))
                    {
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - columnAmount]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + columnAmount]);
                    }
                    //pControl.points[i].GetComponent<Point>().aroundP1, 0] = pControl.points[i - 1];
                    //pControl.points[i].GetComponent<Point>().aroundP0, 1] = pControl.points[i - columnAmount];
                    //pControl.points[i].GetComponent<Point>().aroundP2, 1] = pControl.points[i + columnAmount];
                }
                //các point nằm giữa
                else
                {
                    if (!pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - 1])
                       && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + 1])
                       && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i - columnAmount])
                       && !pControl.points[i].GetComponent<Point>().aroundPoints.Contains(pControl.points[i + columnAmount]))
                    {
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + 1]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i - columnAmount]);
                        pControl.points[i].GetComponent<Point>().aroundPoints.Add(pControl.points[i + columnAmount]);
                    }
                    //pControl.points[i].GetComponent<Point>().aroundP1, 2] = pControl.points[i + 1];
                    //pControl.points[i].GetComponent<Point>().aroundP1, 0] = pControl.points[i - 1];
                    //pControl.points[i].GetComponent<Point>().aroundP0, 1] = pControl.points[i - columnAmount];
                    //pControl.points[i].GetComponent<Point>().aroundP2, 1] = pControl.points[i + columnAmount];
                }
            }
        }
    }
}
