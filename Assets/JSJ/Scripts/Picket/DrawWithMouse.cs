using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Unity.Mathematics;

public class DrawWithMouse : MonoBehaviourPun
{
    public Camera uiCamera;

    public Canvas canvasPicket;   // Picket UI Canvas

    //public GameObject linePrefab;
    public GameObject stickerPrefab;

    public GameObject lineParent;      // Line���� ���� ������Ʈ
    public GameObject stickerParent;   // Sticker���� ���� ������Ʈ

    [Header("Tool ��ư")]
    public Button drawButton;
    public Button stickerButton;

    [Header("�׸���")]
    public float lineWidth = 0.1f;
    public Material material;
    public Color lineColor = Color.red;
    
    public bool isDrawing = false;
    public bool isAttaching = false;
    float timestep;

    public Line line;

    

    private void Start()
    {
        

        drawButton.onClick.AddListener(StartDrawing);
        stickerButton.onClick.AddListener(StartAttaching);

        
    }

    // �׸���
    void StartDrawing()
    {
        // �׸��� Ȱ��ȭ
        isDrawing = true;

        // ���̱� ��Ȱ��ȭ
        isAttaching = false;   
    }

    // ���̱�
    void StartAttaching()
    {
        // ���̱� Ȱ��ȭ
        isAttaching = true;

        // �׸��� ��Ȱ��ȭ
        isDrawing = false;   
    }


    private void Update()
    {
        // �׸��Ⱑ true�̰�, ���콺�� ������ ��
        if (Input.GetMouseButtonDown(0) && isDrawing == true)
        {
            CreateNewLine();
        }    

        // �׸��Ⱑ true�̰�, ���콺�� ������ ���� ��
        if (Input.GetMouseButton(0) && isDrawing == true)
        {
            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (Time.time - timestep > 0.01f)
                {
                    Vector3 point = hitInfo.point + new Vector3(0, 0, -0.1f);

                    line.AddPoint(point);

                    timestep = Time.time;

                    
                }
            }
        }

        // ���̱Ⱑ true�̰�, ���콺�� ������ ��
        if (Input.GetMouseButtonDown(0) && isAttaching == true)
        {
            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                AttachSticker();
            }
        }
    }

    // ���� ���� �Լ�
    void CreateNewLine()
    {
        GameObject lineObject = PhotonNetwork.Instantiate("Line", Vector3.zero, quaternion.identity);
        lineObject.transform.SetParent(lineParent.transform);

        line = lineObject.GetComponent<Line>();
    }

    // ��ƼĿ ���� �Լ�
    void AttachSticker()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasPicket.transform as RectTransform,
            Input.mousePosition,
            uiCamera,
            out Vector2 localPoint
            );

        GameObject stickerInstance = Instantiate(stickerPrefab, stickerParent.transform);
        RectTransform rectTransform = stickerInstance.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    

    
}
