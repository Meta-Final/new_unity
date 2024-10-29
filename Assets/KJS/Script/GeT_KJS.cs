using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeT_KJS : MonoBehaviour
{
    public float detectionRange = 5.0f; // ���� ����
    private GameObject targetFood = null; // ���� ��� �ִ� food ������Ʈ
    private bool isCarrying = false; // ������Ʈ�� ��� �ִ��� ����

    void Update()
    {
        // G Ű�� ������ ������ ������Ʈ�� ���/���� ó��
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isCarrying)
                DetectAndPickUpFood(); // ���� �� ������Ʈ ���� �� ����
            else
                DropFood(); // ��� �ִ� ������Ʈ ����
        }

        // ������Ʈ�� ��� ���� ��, �÷��̾��� �������� �̵���Ŵ
        if (isCarrying && targetFood != null)
        {
            CarryFoodWithPlayer();
        }
    }

    // ���� ���� ���� ����� food ������Ʈ�� ã�� ���� �޼���
    void DetectAndPickUpFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
        float closestDistance = detectionRange;
        targetFood = null;

        foreach (GameObject food in foods)
        {
            float distance = Vector3.Distance(transform.position, food.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                targetFood = food; // ���� ����� food ����
            }
        }

        if (targetFood != null)
        {
            isCarrying = true;
            Debug.Log($"{targetFood.name}��(��) �������ϴ�.");
        }
    }

    // ��� �ִ� ������Ʈ�� �÷��̾��� �������� �̵���Ű�� �޼���
    void CarryFoodWithPlayer()
    {
        // �÷��̾ �ٶ󺸴� �������� �ణ �տ� ��ġ�ϵ��� ����
        Vector3 carryPosition = transform.position + transform.forward * 0.5f + new Vector3(0, 1, 0);
        targetFood.transform.position = carryPosition;
    }

    // ��� �ִ� ������Ʈ�� ���� ��ġ�� Y�� 0�� ������ �������� �޼���
    void DropFood()
    {
        if (targetFood != null)
        {
            // ���� ��ġ�� y���� 0���� �����Ͽ� ������Ʈ�� ����
            Vector3 dropPosition = targetFood.transform.position;
            dropPosition.y = 0;
            targetFood.transform.position = dropPosition;

            Debug.Log($"{targetFood.name}��(��) �������ҽ��ϴ�.");
            targetFood = null; // ���� ����
        }
        isCarrying = false;
    }
}