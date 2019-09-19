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

    //public List<GameObject> GameManager.instance.points;
    public int freeFirstPoint;
    private const int pointAmount = 49;

    // Start is called before the first frame update
    private void Start()
    {
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
        GameManager.instance.points[freeFirstPoint].GetComponent<Point>().cost = 0;
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
                        if (Enumerable.Range(GameManager.instance.points.Count - 7, GameManager.instance.points.Count - 1).Contains(i))
                        {
                            Destroy(GameManager.instance.points[i].transform.GetChild(0).gameObject);
                        }
                        if ((i + 1) % 7 == 0)
                        {
                            Destroy(GameManager.instance.points[i].transform.GetChild(1).gameObject);
                        }
                    }
                }
            }
        }
        yield return 0;
    }
}
