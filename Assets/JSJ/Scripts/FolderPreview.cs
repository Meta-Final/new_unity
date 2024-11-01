using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class FolderPreview : MonoBehaviour
{
    public GameObject previewImage; // �̸����� �̹��� UI ���
    public Vector3 offset = new Vector3(100, 50, 0); // Ŀ�� ��ġ���� ������ �̸����� ��ġ

    private void Start()
    {
        // �̸����� �̹����� ó������ ����ϴ�.
        previewImage.SetActive(false);
    }

    // ���콺�� ��ư ���� �÷��� ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        previewImage.SetActive(true); // �̸����� �̹��� Ȱ��ȭ
        UpdatePreviewPosition();
    }

    // ���콺�� ��ư���� ����� ��
    public void OnPointerExit(PointerEventData eventData)
    {
        previewImage.SetActive(false); // �̸����� �̹��� ��Ȱ��ȭ
    }

    // ���콺 ��ġ�� ���� �̸����� �̹��� ��ġ ������Ʈ
    private void Update()
    {
        if (previewImage.activeSelf)
        {
            UpdatePreviewPosition();
        }
    }

    // �̸����� ��ġ ���� �Լ�
    private void UpdatePreviewPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        previewImage.transform.position = mousePos + offset;
    }



}
