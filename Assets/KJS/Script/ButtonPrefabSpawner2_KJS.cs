using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class ButtonPrefabSpawner2_KJS : MonoBehaviourPunCallbacks
{
    public string prefabResourcePath; // 각 버튼마다 로드할 프리팹의 Resources 경로
    private InventoryText_KJS inventoryText; // 부모의 InventoryText_KJS 스크립트 참조
    private Button button; // 버튼 컴포넌트 참조
    private static int currentPostIdIndex = 0; // POSTID 리스트 인덱스 추적
    private string assignedPostId; // 이 버튼에 할당된 postId

    private void Awake()
    {
        button = GetComponent<Button>();

        // 부모 오브젝트에서 InventoryText_KJS 컴포넌트 찾기
        inventoryText = GetComponentInParent<InventoryText_KJS>();

        if (inventoryText == null)
        {
            Debug.LogError("InventoryText_KJS 스크립트를 부모 오브젝트에서 찾을 수 없습니다.");
            return;
        }

        AssignPostId();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private void AssignPostId()
    {
        List<string> postIdList = inventoryText.inventoryPostIds;

        if (postIdList == null || postIdList.Count == 0)
        {
            Debug.LogWarning("POSTID 리스트가 비어있습니다.");
            return;
        }

        if (currentPostIdIndex < postIdList.Count)
        {
            assignedPostId = postIdList[currentPostIdIndex];
            Debug.Log($"버튼 생성 순서에 따라 할당된 POSTID: {assignedPostId}");
            currentPostIdIndex++;
        }
        else
        {
            Debug.LogWarning("POSTID 리스트의 모든 항목을 이미 사용했습니다.");
        }
    }

    private void OnButtonClicked()
    {
        // 모든 클라이언트에서 프리팹을 생성하는 RPC 호출
        photonView.RPC("SpawnPrefabFromResource_RPC", RpcTarget.AllBuffered, assignedPostId);
    }

    [PunRPC]
    private void SpawnPrefabFromResource_RPC(string postId)
    {
        if (string.IsNullOrEmpty(prefabResourcePath))
        {
            Debug.LogError("프리팹의 Resources 경로가 설정되지 않았습니다.");
            return;
        }

        // Resources 폴더에서 프리팹 로드
        GameObject prefab = Resources.Load<GameObject>(prefabResourcePath);
        if (prefab == null)
        {
            Debug.LogError($"경로 '{prefabResourcePath}'에서 프리팹을 찾을 수 없습니다.");
            return;
        }

        // 프리팹 인스턴스화
        GameObject loadedObject = Instantiate(prefab);

        // 생성된 오브젝트의 태그를 "Item"으로 설정
        loadedObject.tag = "Item";

        // 플레이어 위치에서 현재 로컬 Z 방향(forward)으로 0.5만큼 떨어진 위치 계산
        Vector3 spawnPosition = transform.parent.position + transform.parent.forward.normalized * 0.5f;
        loadedObject.transform.position = spawnPosition;
        loadedObject.transform.localScale = Vector3.one;

        // 오브젝트가 부모 오브젝트와 같은 방향을 바라보도록 회전 설정
        loadedObject.transform.rotation = transform.parent.rotation;

        // PrefabManager_KJS 컴포넌트를 추가하고 postId 설정
        PrefabManager_KJS prefabManager = loadedObject.GetComponent<PrefabManager_KJS>();
        if (prefabManager == null)
        {
            prefabManager = loadedObject.AddComponent<PrefabManager_KJS>();
        }
        prefabManager.postId = postId;
        Debug.Log($"PrefabManager_KJS에 할당된 postId: {prefabManager.postId}");

        Text prefabText = loadedObject.GetComponentInChildren<Text>();
        if (prefabText != null)
        {
            prefabText.text = postId;
        }
        else
        {
            Debug.LogWarning("생성된 프리팹에 Text 컴포넌트가 없습니다.");
        }

        Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLitShader != null)
        {
            MeshRenderer[] meshRenderers = loadedObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in meshRenderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    mat.shader = urpLitShader;
                }
            }
        }
        else
        {
            Debug.LogError("URP의 Lit 쉐이더를 찾을 수 없습니다. URP가 프로젝트에 적용되었는지 확인하세요.");
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}