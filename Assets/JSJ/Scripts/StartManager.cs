using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public RawImage img_Logo;

    public float fadeDuration;

    void SetAlpha(float alpha)
    {
        Color imgColor = img_Logo.color;

        imgColor.a = alpha;

        img_Logo.color = imgColor;
    }

    void Start()
    {
        // 로고 이미지 초기 알파값
        SetAlpha(0f);

        StartCoroutine(FadeInAndOut());
    }

    IEnumerator FadeInAndOut()
    {
        yield return StartCoroutine(FadeTo(0f, 1f, fadeDuration));

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(FadeTo(1f, 0f, fadeDuration));
        
        // Scene 이동
        SceneManager.LoadScene("Meta_LogIn_Scene");
    }

    IEnumerator FadeTo(float startAlpha, float endAlpha, float duration)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);

            SetAlpha(newAlpha);
            yield return null;
        }
    }
}
