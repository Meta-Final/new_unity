using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject frame;
    
    public float followSpeed = 5f;
    public bool isZoom = false;

    Vector3 orginCamPos;
    Vector3 targetPosition;
    Vector3 offset;
    

    void Start()
    {
        mainCamera = Camera.main;

        if (SceneManager.GetActiveScene().name == "PostitTest_Scene")
        {
            frame = GameObject.Find("Frame");
        }
        else
        {
            frame = null;
        }
       
        if (mainCamera != null)
        {
            offset = mainCamera.transform.position - transform.position;
        }
    }

    void Update()
    {
        if (!isZoom)
        {
            CameraMoving();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (frame != null && hit.transform == frame.transform)
                {
                    CameraGoToFrame();
                }
                else
                {
                    isZoom = false;
                }
            }
        }

        if (mainCamera.transform.position != targetPosition)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    // 카메라 이동 
    public void CameraMoving()
    {
        Vector3 playerTargetPos = transform.position + offset;

        targetPosition = playerTargetPos;
    }

    // 카메라 -> Frame
    public void CameraGoToFrame()
    {
        isZoom = true;

        Vector3 frameTargetPos = frame.transform.position + Vector3.back * 3f;

        targetPosition = frameTargetPos;
    }

}
