using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarController : MonoBehaviour
{
    public RectTransform[] icons;
    public Vector2 iconStartPos; // 아이콘의 초기 위치
    public Canvas canvas; // 아이콘이 속한 Canvas

    void Start()
    {
        // 아이콘의 초기 위치 설정 (UI 좌표계 기준)
        foreach (var icon in icons)
        {
            icon.position = canvas.transform.TransformPoint(iconStartPos);
            icon.gameObject.SetActive(false); // 아이콘 초기에는 비활성화
        }
    }

    // 메뉴 열 때 아이콘이 화면에 나오도록
    public void ToggleMenu(bool isMenuOpen)
    {
        foreach (var icon in icons)
        {
            icon.gameObject.SetActive(isMenuOpen); // 메뉴가 열리면 아이콘을 활성화
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

    // 아이콘을 화면에 옮기기
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

    // 아이콘을 화면에서 제거하기
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
