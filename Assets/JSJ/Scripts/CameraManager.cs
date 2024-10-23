using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
   
    public float followSpeed = 5f;
    public bool isZoom = false;

    Vector3 orginCamPos;
    Vector3 targetPosition;
    Vector3 offset;
    

    void Start()
    {
        mainCamera = Camera.main;

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
                if (hit.collider.CompareTag("ContectObject"))
                {
                    isZoom = true;

                    targetPosition = hit.collider.transform.position + Vector3.back * 3f;
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
        targetPosition = transform.position + offset;
    }

}
