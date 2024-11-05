using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Unity.Mathematics;

public class DrawWithMouse : MonoBehaviourPun, IPunObservable
{
    public List<List<Vector3>> allLines = new List<List<Vector3>>();   // ��� ���� ������ ����Ʈ
    public List<Vector3> points = new List<Vector3>();                 // ���� ���� ������ ����Ʈ

    public Camera uiCamera;

    public Canvas canvasPicket;   // Picket UI Canvas

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
                    points.Add(hitInfo.point + new Vector3 (0, 0, -0.1f));

                    timestep = Time.time;

                    lineRenderer.positionCount = points.Count;
                    lineRenderer.SetPositions(points.ToArray());
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
        GameObject go = new GameObject("Line");
        go.transform.SetParent(lineParent.transform);
        lineRenderer = go.transform.AddComponent<LineRenderer>();

        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = material;

        lineRenderer.positionCount = 0;
        points.Clear();
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ����Ʈ�� point ���� ����
            stream.SendNext(points.Count);

            // ����Ʈ�� point ��ǥ ����
            foreach (Vector3 point in points)
            {
                stream.SendNext(point);
            }

        }
        else
        {
            // ����Ʈ�� point ���� ����
            int count = (int)stream.ReceiveNext();
            
            // ����Ʈ�� point ��ǥ�� �ް� �߰���
            for (int i = 0; i < count; i++)
            {
                points.Add((Vector3)stream.ReceiveNext());
            }

            // ���� �����͸� ���� LineRenderer ������Ʈ
            UpdateLineRenderer();
        }
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer == null)
        {
            CreateNewLine();
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
