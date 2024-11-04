using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DrawWithMouse : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();

    public Canvas canvasPicket;

    public Camera uiCamera;

    public GameObject stickerPrefab;

    public Transform lineParent;
    
    public Button drawButton;
    public Button stickerButton;

    [Header("�׸���")]
    public float lineWidth = 0.1f;
    public Material material;
    public Color lineColor = Color.red;
    
    bool isDrawing = false;
    bool isAttaching = false;
    float timestep;

    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        drawButton.onClick.AddListener(StartDrawing);
        stickerButton.onClick.AddListener(StartAttaching);
    }

    // �׸���
    void StartDrawing()
    {
        isDrawing = !isDrawing;
        isAttaching = false;   // ���̱� ��Ȱ��ȭ

        if (!isDrawing)
        {
            lineRenderer.positionCount = 0;
            points.Clear();
        }
    }

    // ���̱�
    void StartAttaching()
    {
        isAttaching = !isAttaching;
        isDrawing = false;   // �׸��� ��Ȱ��ȭ
    }

    private void Update()
    {
        // �׸��Ⱑ true�̰�, ���콺�� ������ ��
        if (Input.GetMouseButtonDown(0) && isDrawing == true)
        {
            GameObject go = new GameObject("Line");
            go.transform.SetParent(lineParent);
            lineRenderer = go.transform.AddComponent<LineRenderer>();

            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.material = material;

            lineRenderer.positionCount = 0;

            points.Clear();
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
                    points.Add(hitInfo.point + new Vector3 (0, 0, -0.1f));
                    timestep = Time.time;
                    CreateLine();
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
    void CreateLine()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
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

        GameObject stickerInstance = Instantiate(stickerPrefab, canvasPicket.transform);
        RectTransform rectTransform = stickerInstance.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = localPoint;
        }

        


    }
}
