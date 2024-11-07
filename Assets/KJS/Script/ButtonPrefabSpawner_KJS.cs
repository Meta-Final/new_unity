using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonPrefabSpawner_KJS : MonoBehaviour
{
    public string objPath; // 각 버튼마다 로드할 .obj 파일 경로
    public string texturePath; // 각 버튼마다 로드할 텍스처 파일 경로 (예: .png, .jpg 파일)
    public GameObject player; // 플레이어 오브젝트 참조
    private InventoryText_KJS inventoryText; // Player의 InventoryText_KJS 스크립트 참조
    private Button button; // 버튼 컴포넌트 참조
    private static int currentPostIdIndex = 0; // POSTID 리스트 인덱스 추적
    private string assignedPostId; // 이 버튼에 할당된 postId

    private void Awake()
    {
        button = GetComponent<Button>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
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

        AssignPostId();

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

    private void SpawnPrefabFromObj()
    {
        if (!string.IsNullOrEmpty(objPath) && player != null && !string.IsNullOrEmpty(assignedPostId))
        {
            if (!File.Exists(objPath))
            {
                Debug.LogError("지정된 경로에 .obj 파일이 존재하지 않습니다.");
                return;
            }

            GameObject loadedObject = new OBJLoader().Load(objPath);
            if (loadedObject == null)
            {
                Debug.LogError("OBJ 파일 로드에 실패했습니다.");
                return;
            }

            // 생성된 오브젝트의 태그를 "Item"으로 설정
            loadedObject.tag = "Item";

            // 플레이어 위치에서 현재 로컬 Z 방향(forward)으로 0.5만큼 떨어진 위치 계산
            Vector3 spawnPosition = player.transform.position + player.transform.forward.normalized * 0.5f;
            loadedObject.transform.position = spawnPosition;
            loadedObject.transform.localScale = Vector3.one;

            // 오브젝트가 플레이어와 같은 방향을 바라보도록 회전 설정 (필요에 따라 설정)
            loadedObject.transform.rotation = player.transform.rotation;

            PrefabManager_KJS prefabManager = loadedObject.GetComponent<PrefabManager_KJS>();
            if (prefabManager == null)
            {
                prefabManager = loadedObject.AddComponent<PrefabManager_KJS>();
            }
            prefabManager.postId = assignedPostId;
            Debug.Log($"PrefabManager_KJS에 할당된 postId: {prefabManager.postId}");

            Text prefabText = loadedObject.GetComponentInChildren<Text>();
            if (prefabText != null)
            {
                prefabText.text = assignedPostId;
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

            // 텍스처 경로가 지정되었으면 텍스처를 로드하여 적용
            if (!string.IsNullOrEmpty(texturePath) && File.Exists(texturePath))
            {
                byte[] fileData = File.ReadAllBytes(texturePath);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(fileData)) // 텍스처 로드 성공
                {
                    foreach (MeshRenderer renderer in loadedObject.GetComponentsInChildren<MeshRenderer>())
                    {
                        foreach (Material mat in renderer.materials)
                        {
                            mat.mainTexture = texture; // 텍스처 할당
                        }
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(SpawnPrefabFromObj);
        }
    }
}