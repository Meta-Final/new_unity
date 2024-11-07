using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonPrefabSpawner_KJS : MonoBehaviour
{
    public string objPath; // 각 버튼마다 로드할 .obj 파일 경로
    private GameObject player; // 플레이어 오브젝트 참조
    private InventoryText_KJS inventoryText; // Player의 InventoryText_KJS 스크립트 참조
    private Button button; // 버튼 컴포넌트 참조
    private static int currentPostIdIndex = 0; // POSTID 리스트 인덱스 추적
    private string assignedPostId; // 이 버튼에 할당된 postId

    private void Awake()
    {
        // 버튼 컴포넌트 가져오기
        button = GetComponent<Button>();

        // Player 태그를 가진 오브젝트 자동 검색
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Player 오브젝트에서 InventoryText_KJS 스크립트 가져오기
            inventoryText = player.GetComponent<InventoryText_KJS>();

            if (inventoryText == null)
            {
                Debug.LogError("InventoryText_KJS 스크립트를 찾을 수 없습니다.");
                return;
            }
        }
        else
        {
            Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
            return;
        }

        // POSTID 리스트에서 버튼 생성 순서에 따라 postId 할당
        AssignPostId();

        // 버튼 클릭 이벤트에 메서드 등록
        if (button != null)
        {
            button.onClick.AddListener(SpawnPrefabFromObj);
        }
        else
        {
            Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private void AssignPostId()
    {
        List<string> postIdList = inventoryText.inventoryPostIds;

        // POSTID 리스트가 비어 있는지 확인
        if (postIdList == null || postIdList.Count == 0)
        {
            Debug.LogWarning("POSTID 리스트가 비어있습니다.");
            return;
        }

        // 현재 인덱스가 리스트 범위 내에 있는지 확인하고 할당
        if (currentPostIdIndex < postIdList.Count)
        {
            assignedPostId = postIdList[currentPostIdIndex];
            Debug.Log($"버튼 생성 순서에 따라 할당된 POSTID: {assignedPostId}");
            currentPostIdIndex++; // 다음 버튼이 생성될 때 다음 POSTID 사용
        }
        else
        {
            Debug.LogWarning("POSTID 리스트의 모든 항목을 이미 사용했습니다.");
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    private void SpawnPrefabFromObj()
    {
        if (!string.IsNullOrEmpty(objPath) && player != null && !string.IsNullOrEmpty(assignedPostId))
        {
            // .obj 파일이 존재하는지 확인
            if (!File.Exists(objPath))
            {
                Debug.LogError("지정된 경로에 .obj 파일이 존재하지 않습니다.");
                return;
            }

            // OBJLoader를 이용하여 .obj 파일 로드
            GameObject loadedObject = new OBJLoader().Load(objPath);
            if (loadedObject == null)
            {
                Debug.LogError("OBJ 파일 로드에 실패했습니다.");
                return;
            }

            // 플레이어 위치에서 (1, 0, 0) 오프셋 적용
            Vector3 spawnPosition = player.transform.position + new Vector3(1, 0, 0);
            loadedObject.transform.position = spawnPosition;

            // 생성된 프리팹의 크기를 1로 조정
            loadedObject.transform.localScale = Vector3.one;

            // PrefabManager_KJS 컴포넌트 추가 후 postId 설정
            PrefabManager_KJS prefabManager = loadedObject.GetComponent<PrefabManager_KJS>();
            if (prefabManager == null)
            {
                // PrefabManager_KJS 컴포넌트가 없으면 추가
                prefabManager = loadedObject.AddComponent<PrefabManager_KJS>();
            }
            prefabManager.postId = assignedPostId;  // 버튼에 미리 할당된 postId 사용
            Debug.Log($"PrefabManager_KJS에 할당된 postId: {prefabManager.postId}");

            // 생성된 프리팹의 텍스트 컴포넌트에 POSTID 표시
            Text prefabText = loadedObject.GetComponentInChildren<Text>();
            if (prefabText != null)
            {
                prefabText.text = assignedPostId;
            }
            else
            {
                Debug.LogWarning("생성된 프리팹에 Text 컴포넌트가 없습니다.");
            }

            // 생성된 오브젝트의 모든 Material을 URP의 Lit 쉐이더로 설정
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
        else
        {
            Debug.LogWarning("Prefab을 생성할 수 없습니다. 필수 구성 요소가 누락되었습니다.");
        }
    }

    private void OnDestroy()
    {
        // 이벤트 등록 해제 (메모리 누수 방지)
        if (button != null)
        {
            button.onClick.RemoveListener(SpawnPrefabFromObj);
        }
    }
}