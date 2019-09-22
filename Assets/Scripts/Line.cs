using System.Collections;
using UnityEngine;

public class Line : MonoBehaviour
{
    public GameObject circle;
    // Start is called before the first frame update
    void Start()
    {
        circle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator FillFullLine(GameObject startPos, GameObject endPos)
    {
        circle.SetActive(true);
        float time = 0f;
        if (startPos.GetComponent<Point>().block == false && endPos.GetComponent<Point>().block == false)
        {
            while (time < 2f)
            {
                time += Time.deltaTime * 2f;
                circle.transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, time);
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.2f);
        circle.SetActive(false);
    }
}
