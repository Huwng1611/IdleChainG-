using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 0.1f;
    public Camera cam;

    private Vector3 touchStart;
    public float groundZ = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1 && Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(groundZ);
        }
        if (Input.touchCount == 1 && Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - GetWorldPosition(groundZ);
            cam.transform.position += direction;
        }

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            cam.fieldOfView += deltaMagnitudeDiff * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 80f, 140f);
        }
    }
    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        ground.Raycast(mousePos, out float distance);
        return mousePos.GetPoint(distance);
    }
}

