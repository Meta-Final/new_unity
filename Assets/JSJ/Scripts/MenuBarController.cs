using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarController : MonoBehaviour
{
    public GameObject[] icons;   // ������ �迭

    public float duration = 0.5f; 

    Vector3[] originPos;

    bool isMenuOpen = false;
    
    void Start()
    {
        originPos = new Vector3[icons.Length];

        for (int i = 0; i < icons.Length; i++)
        {
            // �ʱ� ��ġ ����
            originPos[i] = icons[i].transform.localPosition;

            // ������ ��Ȱ��ȭ
            icons[i].SetActive(false);
        }
    }

    // Menu Toggle �Լ�
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

    // Menu ���� �Լ�
    public void MenuOpen()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].transform.DOLocalMove(new Vector3(250 - 960 + (i * 150), 450, 0), duration);

            // ������ Ȱ��ȭ
            icons[i].SetActive(true);
        }
    }

    // Menu �ݴ� �Լ�
    public void MenuClose()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            // �ִϸ��̼��� ������,
            icons[i].transform.DOLocalMove(originPos[i], duration).OnComplete(Gameoff);
        }
    }

    public void Gameoff()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            // ������ ��Ȱ��ȭ
            icons[i].SetActive(false);
        }
    }
}
