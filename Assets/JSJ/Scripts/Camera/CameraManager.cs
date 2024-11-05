using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    
    public float followSpeed = 5f;
    
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
        CameraMoving();
    }

    //카메라 이동
    public void CameraMoving()
    {
        Vector3 playerTargetPos = transform.position + offset;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, playerTargetPos, followSpeed * Time.deltaTime);
    }
}
