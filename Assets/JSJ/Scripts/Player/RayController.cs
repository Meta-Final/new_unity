using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RayController : MonoBehaviour
{
    public Transform targetPos; // 동적으로 생성된 "Player" 오브젝트의 Transform
    public GameObject canvasNotice; // 알림 UI 오브젝트
    public float noticeDistance = 3f; // 알림 활성화 거리

    public Transform rayPos;

    void Start()
    {
        // 현재 씬이 Meta_ScrapBook_Scene일 경우 Canvas를 찾음
        if (SceneManager.GetActiveScene().name == "Meta_ScrapBook_Scene")
        {
            canvasNotice = GameObject.Find("Canvas_Interactive");
        }
        else
        {
            canvasNotice = null;
        }

        // Canvas를 초기에는 비활성화
        if (canvasNotice != null)
        {
            canvasNotice.SetActive(false);
        }

        // "Player" 태그를 가진 오브젝트를 찾아 targetPos에 할당
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            targetPos = player.transform;
        }
    }

    void Update()
    {
        // Player가 동적으로 생성될 경우를 대비해 매 프레임 확인
        if (targetPos == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                targetPos = player.transform;
            }
        }

        // 거리 기반으로 Canvas 활성화/비활성화
        CheckDistance();
    }

    public void CheckDistance()
    {
        if (targetPos != null && canvasNotice != null)
        {
            // Player와 RayController 간의 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, targetPos.position);

            // 거리가 설정된 noticeDistance 이내라면 Canvas 활성화
            if (distanceToPlayer <= noticeDistance)
            {
                canvasNotice.SetActive(true);
            }
            else
            {
                canvasNotice.SetActive(false);
            }
        }
    }

    public void EnterMagazine()
    {
        Ray ray = new Ray(rayPos.position, rayPos.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.gameObject.layer == 21)
            {

            }
        }
    }
}