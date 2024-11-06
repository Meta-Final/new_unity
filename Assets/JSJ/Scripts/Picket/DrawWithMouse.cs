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

    public GameObject lineParent;      // Line���� ���� ������Ʈ
    public GameObject stickerParent;   // Sticker���� ���� ������Ʈ

    [Header("Tool ��ư")]
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
        GameObject lineInstance = PhotonNetwork.Instantiate("Line", Vector3.zero, quaternion.identity);

        // ���ÿ��� ���� ������Ʈ�� �θ� ����
        lineInstance.transform.SetParent(lineParent.transform);

        PhotonView linePhotonView = lineInstance.GetComponent<PhotonView>();

        // ���濡�Ե� ���� ������Ʈ�� �θ� ����ȭ
        photonView.RPC("SetLineParent", RpcTarget.OthersBuffered, linePhotonView.ViewID);

        line = lineInstance.GetComponent<Line>();
    }

    // ���� ������Ʈ �θ� ����ȭ �Լ�
    [PunRPC]
    void SetLineParent(int lineViewID)
    {
        GameObject lineObject = PhotonView.Find(lineViewID).gameObject;
        lineObject.transform.SetParent(lineParent.transform);
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

        photonView.RPC("Sticker", RpcTarget.AllBuffered, localPoint);
    }

    
    // ��ƼĿ ���� ����ȭ
    [PunRPC]
    void Sticker(Vector2 localPoint)
    {
        GameObject stickerInstance = Instantiate(stickerprefab, canvasPicket.transform);
        stickerInstance.transform.localPosition = localPoint;

        

        photonView.RPC("SetStickerParent", RpcTarget.AllBuffered, stickerInstance.GetComponent<PhotonView>().ViewID);
    }

    // ��ƼĿ ������Ʈ �θ� ����ȭ �Լ�
    [PunRPC]
    void SetStickerParent(int stickerViewID)
    {
        GameObject stickerObject = PhotonView.Find(stickerViewID).gameObject;
        stickerObject.transform.SetParent(stickerParent.transform);
    }
}
