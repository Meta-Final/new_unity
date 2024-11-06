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

    public GameObject stickerprefab;

    public GameObject lineParent;      // Line들이 모인 오브젝트
    public GameObject stickerParent;   // Sticker들이 모인 오브젝트

    [Header("Tool 버튼")]
    public Button drawButton;
    public Button stickerButton;

    public bool isDrawing = false;
    public bool isAttaching = false;

    float timestep;

    public Line line;
   
    
    private void Start()
    {
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
                    Vector3 point = hitInfo.point + new Vector3(0, 0, -0.1f);

                    line.AddPoint(point);

                    timestep = Time.time;

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
        GameObject lineInstance = PhotonNetwork.Instantiate("Line", Vector3.zero, quaternion.identity);

        // 로컬에서 라인 오브젝트의 부모 설정
        lineInstance.transform.SetParent(lineParent.transform);

        PhotonView linePhotonView = lineInstance.GetComponent<PhotonView>();

        // 상대방에게도 라인 오브젝트의 부모를 동기화
        photonView.RPC("SetLineParent", RpcTarget.OthersBuffered, linePhotonView.ViewID);

        line = lineInstance.GetComponent<Line>();
    }

    // 라인 오브젝트 부모 동기화 함수
    [PunRPC]
    void SetLineParent(int lineViewID)
    {
        GameObject lineObject = PhotonView.Find(lineViewID).gameObject;
        lineObject.transform.SetParent(lineParent.transform);
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

        photonView.RPC("Sticker", RpcTarget.AllBuffered, localPoint);
    }

    
    // 스티커 생성 동기화
    [PunRPC]
    void Sticker(Vector2 localPoint)
    {
        GameObject stickerInstance = Instantiate(stickerprefab, canvasPicket.transform);
        stickerInstance.transform.localPosition = localPoint;

        

        photonView.RPC("SetStickerParent", RpcTarget.AllBuffered, stickerInstance.GetComponent<PhotonView>().ViewID);
    }

    // 스티커 오브젝트 부모 동기화 함수
    [PunRPC]
    void SetStickerParent(int stickerViewID)
    {
        GameObject stickerObject = PhotonView.Find(stickerViewID).gameObject;
        stickerObject.transform.SetParent(stickerParent.transform);
    }
}
