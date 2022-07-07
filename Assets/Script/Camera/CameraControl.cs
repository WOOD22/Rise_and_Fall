using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//메인카메라에 넣어서 사용
public class CameraControl : MonoBehaviour
{
    public float move_speed = 0.1f;
    public float zoom_speed = 10f;
    Camera camera;

    void Start()
    {
        camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        Camera_Zoom();
        Camera_Move();
    }
    //줌 기능
    void Camera_Zoom()
    {
        if(camera.orthographicSize >= 10)
            zoom_speed = camera.orthographicSize;

        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoom_speed;

        // scroll < 0 : scroll down하면 줌인
        if (camera.orthographicSize <= 2 && scroll > 0)
        {
            scroll = camera.orthographicSize;
            camera.orthographicSize = scroll; // maximize zoom in

            // 최대로 Zoom in 했을 때 특정 값을 지정했을 때

            // 최대 줌 인 범위를 벗어날 때 값에 맞추려고 한번 줌 아웃 되는 현상을 방지
        }
        // scroll > 0 : scroll up하면 줌아웃
        else if (camera.orthographicSize >= 50 && scroll < 0)
        {
            scroll = camera.orthographicSize;
            camera.orthographicSize = scroll; // maximize zoom out
        }
        else
            camera.orthographicSize -= scroll * 0.5f;
    }

    void Camera_Move()
    {
        move_speed = (camera.orthographicSize * 0.01f);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0.0f, move_speed, 0.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0.0f, move_speed, 0.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(move_speed, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(move_speed, 0.0f, 0.0f);
        }
    }
}
