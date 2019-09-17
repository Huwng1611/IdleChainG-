using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GenerateMap : MonoBehaviour
{
    public Transform parentMap;
    public GameObject point;
    public GameObject line;

    public List<GameObject> points;
    public int freeFirstPoint;
    private const int pointAmount = 49;

    // Start is called before the first frame update
    void Start()
    {
        GeneratePointOnMap();
        //EventClickTest();
    }

    private void Update()
    {
    }

    private void GetAFreePoint()
    {
        freeFirstPoint = Random.Range(points.Count - 1, points.IndexOf(points[0]));
        points[freeFirstPoint].GetComponent<Image>().color = Color.white;
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
            points.Add(obj);
            pointCount++;
        }
        GetAFreePoint();
        GameManager.instance.AddEventForPoint(points);
        GameManager.instance.DefaultValueFirstPoint(points[freeFirstPoint]);
        StartCoroutine(DrawLineMap());
        Debug.Log("<color=green> Map Generated! </color>");
    }

    /// <summary>
    /// Tạo đường nối các points
    /// </summary>
    /// <returns></returns>
    private IEnumerator DrawLineMap()
    {
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = 0; j < points.Count; j++)
            {
                if (points[i].GetComponent<RectTransform>().rect.x == points[j].GetComponent<RectTransform>().rect.x)
                {
                    if (points[i].transform.childCount < 2)
                    {
                        GameObject ln = Instantiate(line, points[i].gameObject.transform);
                        Vector3 temp;
                        temp = ln.transform.position;
                        temp.y -= 100;
                        ln.transform.position = temp;
                    }
                }
                if (points[i].GetComponent<RectTransform>().rect.y == points[j].GetComponent<RectTransform>().rect.y)
                {
                    if (points[i].transform.childCount < 2)
                    {
                        GameObject ln = Instantiate(line, points[i].gameObject.transform.position, Quaternion.Euler(0f, 0f, 90f), points[i].transform);
                        Vector3 temp;
                        temp = ln.transform.position;
                        temp.x += 100;
                        ln.transform.position = temp;
                        if (Enumerable.Range(points.Count - 7, points.Count - 1).Contains(i))
                        {
                            Destroy(points[i].transform.GetChild(0).gameObject);
                        }
                        if ((i + 1) % 7 == 0)
                        {
                            Destroy(points[i].transform.GetChild(1).gameObject);
                        }
                    }
                }
            }
        }
        yield return 0;
    }
}
