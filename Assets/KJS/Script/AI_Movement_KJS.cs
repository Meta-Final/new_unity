using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AI_Movement_KJS : MonoBehaviourPun
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    private Vector3 offset = new Vector3(1, 3, -1);

    // Scene에서 tool UI를 저장하기 위한 변수
    private GameObject Chat;

    //삐약이 사용법
    public GameObject npctutorial;
    
    void Start()
    {
        npctutorial = GameObject.Find("aitutorial");
        StartCoroutine(SetFalse(2));
        agent = GetComponent<NavMeshAgent>();

        // 클라이언트의 로컬 플레이어 찾기
        FindLocalPlayer();

        // "MagazineView" 오브젝트 안에 있는 "Tool" UI를 찾기
        GameObject magazineView = GameObject.Find("MagazineView");
        if (magazineView != null)
        {
            Chat = magazineView.transform.Find("Chat")?.gameObject;
            if (Chat != null)
            {
                // tool UI, NPC를 비활성화된 상태로 초기화
                Chat.SetActive(false);
               
            }
            else
            {
                Debug.LogError("Tool UI not found within MagazineView.");
            }
        }
        else
        {
            Debug.LogError("MagazineView object not found in the scene.");
        }
    }

    void Update()
    {
        // AI가 이 클라이언트의 로컬 플레이어만 따라가도록 설정
        if (playerTransform != null && photonView.IsMine)
        {
            // 플레이어의 회전을 적용한 상대 위치 계산
            Vector3 rotatedOffset = playerTransform.rotation * offset;
            Vector3 targetPosition = playerTransform.position + rotatedOffset;

            agent.SetDestination(targetPosition);
        }

        if (Input.GetMouseButtonDown(0) && photonView.IsMine)  // 마우스 왼쪽 클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 이 스크립트가 부착된 오브젝트를 클릭했는지 확인
                if (hit.transform == transform)
                {
                    if (Chat != null)
                    {
                        Chat.SetActive(true);
                      
                        }
                }
            }
        }

        // ESC 키가 눌렸을 때 tool UI를 비활성화
        if (Input.GetKeyDown(KeyCode.Escape) && Chat != null)
        {
            Chat.SetActive(false);
           
        }
    }

    private void FindLocalPlayer()
    {
        // 모든 플레이어 오브젝트를 찾아 로컬 플레이어를 설정
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();

            // 이 클라이언트에 속한 플레이어인지 확인
            if (photonView != null && photonView.IsMine)
            {
                playerTransform = player.transform;
                break;
            }
        }

        if (playerTransform == null)
        {
            Debug.LogError("Local player object with tag 'Player' not found.");
        }
    }

    private void OnMouseDown()
    {
        // 이 스크립트가 할당된 오브젝트를 로컬 플레이어가 클릭한 경우 tool UI를 활성화
        if (photonView.IsMine && Chat != null)
        {
            Chat.SetActive(true);
        }
     

    }

    IEnumerator SetFalse(float t)
    {
        yield return new WaitForSeconds(t);
        npctutorial.SetActive(false);
    }

}