using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RayController : MonoBehaviour
{
    public Transform targetPos; // �������� ������ "Player" ������Ʈ�� Transform
    public GameObject canvasNotice; // �˸� UI ������Ʈ
    public float noticeDistance = 3f; // �˸� Ȱ��ȭ �Ÿ�

    public Transform rayPos;

    void Start()
    {
        // ���� ���� Meta_ScrapBook_Scene�� ��� Canvas�� ã��
        if (SceneManager.GetActiveScene().name == "Meta_ScrapBook_Scene")
        {
            canvasNotice = GameObject.Find("Canvas_Interactive");
        }
        else
        {
            canvasNotice = null;
        }

        // Canvas�� �ʱ⿡�� ��Ȱ��ȭ
        if (canvasNotice != null)
        {
            canvasNotice.SetActive(false);
        }

        // "Player" �±׸� ���� ������Ʈ�� ã�� targetPos�� �Ҵ�
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            targetPos = player.transform;
        }
    }

    void Update()
    {
        // Player�� �������� ������ ��츦 ����� �� ������ Ȯ��
        if (targetPos == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                targetPos = player.transform;
            }
        }

        // �Ÿ� ������� Canvas Ȱ��ȭ/��Ȱ��ȭ
        CheckDistance();
    }

    public void CheckDistance()
    {
        if (targetPos != null && canvasNotice != null)
        {
            // Player�� RayController ���� �Ÿ� ���
            float distanceToPlayer = Vector3.Distance(transform.position, targetPos.position);

            // �Ÿ��� ������ noticeDistance �̳���� Canvas Ȱ��ȭ
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