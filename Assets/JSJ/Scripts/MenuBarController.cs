using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarController : MonoBehaviour
{
    public RectTransform[] btnRectTransform;

    public float distance = 100f;
    public float spacing = 5f;
    public float duration = 0.5f;
    public float delayBetween = 0.5f;

    bool isMenuOpen = false;
    
    
    void Start()
    {
        
    }

    public void MenuToggle()
    {
        if (isMenuOpen)
        {
            //StartCoroutine(MenuClose());

        }
        else
        {
            StartCoroutine(MenuOpen());

        }
    }

    IEnumerator MenuOpen()
    {
        for (int i = 0; i < btnRectTransform.Length; i++)
        {
            Vector2 orginPos = btnRectTransform[i].anchoredPosition;
            Vector2 targetPos = new Vector2(distance + (i * spacing), orginPos.y);
            float currentTime = 0;

            while (currentTime < duration)
            {
                btnRectTransform[i].anchoredPosition = Vector2.Lerp(orginPos, targetPos, (currentTime / duration));
                currentTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(delayBetween);
        }

    }

    //IEnumerator MenuClose()
    //{

    //}

    














}
