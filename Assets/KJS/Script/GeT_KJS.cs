using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeT_KJS : MonoBehaviour
{
    public float detectionRange = 5.0f; // �˻��� �Ÿ� ����

    // �� ������ ȣ��
    void Update()
    {
        // G Ű�� ������ �� ����
        if (Input.GetKeyDown(KeyCode.G))
        {
            DestroyNearbyFood();
        }
    }

    // ����� food �±׸� ���� ������Ʈ�� ã�� �ı��ϴ� �޼���
    void DestroyNearbyFood()
    {
        // ��� food �±׸� ���� ������Ʈ�� �迭�� ��������
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");

        foreach (GameObject food in foods)
        {
            // �÷��̾�(���� ������Ʈ)�� food ������ �Ÿ� ���
            float distance = Vector3.Distance(transform.position, food.transform.position);

            // ������ ���� ���� ���� ��� ������Ʈ �ı�
            if (distance <= detectionRange)
            {
                Destroy(food);
                Debug.Log($"{food.name} ������Ʈ�� �ı��Ǿ����ϴ�.");
                break; // �� ���� �ϳ��� ������Ʈ�� �ı��ϰ� ����
            }
        }
    }
}
