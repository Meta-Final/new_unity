using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarController : MonoBehaviour
{
    public GameObject[] icons;   // 아이콘 배열

    public float duration = 0.5f; 

    Vector3[] originPos;

    bool isMenuOpen = false;
    
    void Start()
    {
        originPos = new Vector3[icons.Length];

        for (int i = 0; i < icons.Length; i++)
        {
            // 초기 위치 저장
            originPos[i] = icons[i].transform.localPosition;

            // 아이콘 비활성화
            icons[i].SetActive(false);
        }
    }

    // Menu Toggle 함수
    public void MenuToggle()
    {
        if (isMenuOpen)
        {
            MenuClose();

        }
        else
        {
            MenuOpen();
        }

        isMenuOpen = !isMenuOpen;
    }

    // Menu 여는 함수
    public void MenuOpen()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].transform.DOLocalMove(new Vector3(250 - 960 + (i * 150), 450, 0), duration);

            // 아이콘 활성화
            icons[i].SetActive(true);
        }
    }

    // Menu 닫는 함수
    public void MenuClose()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            // 애니메이션이 끝나면,
            icons[i].transform.DOLocalMove(originPos[i], duration).OnComplete(Gameoff);
        }
    }

    public void Gameoff()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            // 아이콘 비활성화
            icons[i].SetActive(false);
        }
    }
}
