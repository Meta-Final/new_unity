using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarController : MonoBehaviour
{
    public RectTransform[] icons;
    public Vector2 iconStartPos; // �������� �ʱ� ��ġ
    public Canvas canvas; // �������� ���� Canvas

    void Start()
    {
        // �������� �ʱ� ��ġ ���� (UI ��ǥ�� ����)
        foreach (var icon in icons)
        {
            icon.position = canvas.transform.TransformPoint(iconStartPos);
            icon.gameObject.SetActive(false); // ������ �ʱ⿡�� ��Ȱ��ȭ
        }
    }

    // �޴� �� �� �������� ȭ�鿡 ��������
    public void ToggleMenu(bool isMenuOpen)
    {
        foreach (var icon in icons)
        {
            icon.gameObject.SetActive(isMenuOpen); // �޴��� ������ �������� Ȱ��ȭ
        }

        if (isMenuOpen)
        {
            StartCoroutine(IconMoveOn());
        }
        else
        {
            StartCoroutine(IconMoveOff());
        }
    }

    // �������� ȭ�鿡 �ű��
    IEnumerator IconMoveOn()
    {
        float moveDuration = 0.5f;
        float delayBetweenIcons = 0.2f;

        for (int i = 0; i < icons.Length; i++)
        {
            Vector3 targetPosition = icons[i].position + new Vector3(100f + (i * 50f), 0, 0);
            float currentTime = 0;

            while (currentTime < moveDuration)
            {
                icons[i].position = Vector3.Lerp(icons[i].position, targetPosition, (currentTime / moveDuration));
                currentTime += Time.deltaTime;
                yield return null;
            }

            icons[i].position = targetPosition;
            yield return new WaitForSeconds(delayBetweenIcons);
        }
    }

    // �������� ȭ�鿡�� �����ϱ�
    IEnumerator IconMoveOff()
    {
        float moveDuration = 0.5f;
        float delayBetweenIcons = 0.2f;

        for (int i = icons.Length - 1; i >= 0; i--)
        {
            Vector3 targetPosition = icons[i].position - new Vector3(100f + (i * 50f), 0, 0);
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                icons[i].position = Vector3.Lerp(icons[i].position, targetPosition, (elapsedTime / moveDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            icons[i].position = targetPosition;
            yield return new WaitForSeconds(delayBetweenIcons);
        }
    }
}
