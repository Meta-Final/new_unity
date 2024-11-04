using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DrawWithMouse : MonoBehaviour
{
    LineRenderer lineRenderer;

    public List<Vector3> points = new List<Vector3>();

    public Color lineColor = Color.red;
    public float lineWidth = 0.3f;
    public Material material;
    public Button drawButton;

    bool isDrawing = false;
    float timestep;

    public Camera uiCam;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.startColor = lineColor;
        //lineRenderer.endColor = lineColor;
        //lineRenderer.startWidth = lineWidth;
        //lineRenderer.endWidth = lineWidth;
        //lineRenderer.material = material;
        //lineRenderer.positionCount = 0;

        drawButton.onClick.AddListener(StartDrawing);
    }

    private void Update()
    {
        // 마우스를 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = new GameObject("Line");
            lineRenderer = go.transform.AddComponent<LineRenderer>();

            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.material = material;

            lineRenderer.positionCount = 0;

            points.Clear();
        }    

        // 마우스를 누르고 있을 때
        if (Input.GetMouseButton(0))
        {
            Ray ray = uiCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (Time.time - timestep > 0.05f)
                {
                    points.Add(hitInfo.point + new Vector3 (0, 0, -0.1f));
                    timestep = Time.time;
                    CreateLine();
                }
            }
        }
    }

    // 라인 생성 함수
    void CreateLine()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    void StartDrawing()
    {
        isDrawing = !isDrawing;

        if (!isDrawing)
        {
            lineRenderer.positionCount = 0;
            points.Clear();
        }
    }
}
