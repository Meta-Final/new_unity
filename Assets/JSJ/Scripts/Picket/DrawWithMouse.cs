using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Unity.Mathematics;

public class DrawWithMouse : MonoBehaviourPun, IPunObservable
{
    public List<List<Vector3>> allLines = new List<List<Vector3>>();   // 모든 라인 점들의 리스트
    public List<Vector3> points = new List<Vector3>();                 // 현재 라인 점들의 리스트

    public Camera uiCamera;

    public Canvas canvasPicket;   // Picket UI Canvas

    public GameObject stickerPrefab;

    public GameObject lineParent;      // Line들이 모인 오브젝트
    public GameObject stickerParent;   // Sticker들이 모인 오브젝트

    [Header("Tool 버튼")]
    public Button drawButton;
    public Button stickerButton;

    [Header("그리기")]
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

    // 그리기
    void StartDrawing()
    {
        // 그리기 활성화
        isDrawing = true;

        // 붙이기 비활성화
        isAttaching = false;   
    }

    // 붙이기
    void StartAttaching()
    {
        // 붙이기 활성화
        isAttaching = true;

        // 그리기 비활성화
        isDrawing = false;   
    }


    private void Update()
    {
        // 그리기가 true이고, 마우스를 눌렀을 때
        if (Input.GetMouseButtonDown(0) && isDrawing == true)
        {
            CreateNewLine();
        }    

        // 그리기가 true이고, 마우스를 누르고 있을 때
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

        // 붙이기가 true이고, 마우스를 눌렀을 때
        if (Input.GetMouseButtonDown(0) && isAttaching == true)
        {
            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                AttachSticker();
            }
        }
    }

    // 라인 생성 함수
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

    // 스티커 생성 함수
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
            // 리스트의 point 갯수 보냄
            stream.SendNext(points.Count);

            // 리스트의 point 좌표 보냄
            foreach (Vector3 point in points)
            {
                stream.SendNext(point);
            }

        }
        else
        {
            // 리스트의 point 갯수 받음
            int count = (int)stream.ReceiveNext();
            
            // 리스트의 point 좌표를 받고 추가함
            for (int i = 0; i < count; i++)
            {
                points.Add((Vector3)stream.ReceiveNext());
            }

            // 받은 데이터를 통해 LineRenderer 업데이트
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
