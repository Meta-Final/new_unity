using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using System.Collections;

public class ButtonPrefabSpawner_KJS : MonoBehaviourPun
{
    public string objName; // 각 버튼마다 로드할 .obj 파일 경로 (로컬 파일 경로)
    public string texturePath; // 각 버튼마다 로드할 텍스처 파일 경로 (로컬 파일 경로)
    private InventoryText_KJS inventoryText; // 부모의 InventoryText_KJS 스크립트 참조
    private Button button; // 버튼 컴포넌트 참조
    private static int currentPostIdIndex = 0; // POSTID 리스트 인덱스 추적
    private string assignedPostId; // 이 버튼에 할당된 postId

    private string localObjSavePath; // 로컬에 저장될 .obj 파일 경로
    private string localTextureSavePath; // 로컬에 저장될 텍스처 파일 경로

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
        }

        // 0.1초 후에 InventoryText_KJS 찾기 시도
        StartCoroutine(FindInventoryTextWithDelay());
    }

    private IEnumerator FindInventoryTextWithDelay()
    {
        yield return new WaitForSeconds(0.1f);

        inventoryText = FindObjectOfType<InventoryText_KJS>();

        if (inventoryText == null)
        {
            Debug.LogError("InventoryText_KJS 스크립트를 찾을 수 없습니다. InventoryText_KJS가 씬에 존재하는지 확인하세요.");
            yield break;
        }

        AssignPostId();
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
        // 버튼 클릭 시 RPC 호출을 통해 네트워크 상에서 오브젝트 생성
        photonView.RPC(objName, RpcTarget.AllBuffered, assignedPostId);
    }

    [PunRPC]
    private void SpawnPrefabFromObj_RPC(string postId)
    {
        // objName은 프리팹 이름으로 사용됩니다 (예: Resources 폴더 내 프리팹 이름)
        string prefabName = objName; // 프리팹 이름을 지정 (예: "MyPrefab"이라는 프리팹이 Resources 폴더에 있어야 함)

        // PhotonNetwork.Instantiate를 사용하여 네트워크 상에서 프리팹을 생성
        GameObject prefab = PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity);

        if (prefab == null)
        {
            Debug.LogError($"프리팹 '{prefabName}'을(를) 생성하지 못했습니다. Resources 폴더에 해당 프리팹이 있는지 확인하세요.");
            return;
        }

        // 생성된 프리팹에 postId 설정
        PrefabManager_KJS prefabManager = prefab.GetComponent<PrefabManager_KJS>();
        if (prefabManager == null)
        {
            prefabManager = prefab.AddComponent<PrefabManager_KJS>();
        }
        prefabManager.postId = postId;

        // 추가적인 설정 및 초기화
        InitializePrefab(prefab, postId);
    }

    private void InitializePrefab(GameObject prefab, string postId)
    {
        // 생성된 오브젝트의 태그를 "Item"으로 설정
        prefab.tag = "Item";

        // 플레이어 위치에서 현재 로컬 Z 방향(forward)으로 0.5만큼 떨어진 위치 계산
        Vector3 spawnPosition = transform.parent.position + transform.parent.forward.normalized * 0.5f;
        prefab.transform.position = spawnPosition;
        prefab.transform.localScale = Vector3.one;

        // 오브젝트가 부모 오브젝트와 같은 방향을 바라보도록 회전 설정 (필요에 따라 설정)
        prefab.transform.rotation = transform.parent.rotation;

        // PrefabManager_KJS 컴포넌트를 추가하고 postId 설정
        PrefabManager_KJS prefabManager = prefab.GetComponent<PrefabManager_KJS>();
        if (prefabManager == null)
        {
            prefabManager = prefab.AddComponent<PrefabManager_KJS>();
        }
        prefabManager.postId = postId;
        Debug.Log($"PrefabManager_KJS에 할당된 postId: {prefabManager.postId}");

        // 텍스트 컴포넌트를 사용하여 postId 표시
        Text prefabText = prefab.GetComponentInChildren<Text>();
        if (prefabText != null)
        {
            prefabText.text = postId;
        }
        else
        {
            Debug.LogWarning("생성된 프리팹에 Text 컴포넌트가 없습니다.");
        }

        // URP 쉐이더 설정 (필요 시)
        Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLitShader != null)
        {
            MeshRenderer[] meshRenderers = prefab.GetComponentsInChildren<MeshRenderer>();
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

    private void LoadAndInstantiateModel(string objFilePath, string textureFilePath, string postId)
    {
        if (!File.Exists(objFilePath))
        {
            Debug.LogError("로컬에 저장된 .obj 파일을 찾을 수 없습니다.");
            return;
        }

        GameObject loadedObject = new OBJLoader().Load(objFilePath);
        if (loadedObject == null)
        {
            Debug.LogError("OBJ 파일 로드에 실패했습니다.");
            return;
        }

        // 생성된 오브젝트의 태그를 "Item"으로 설정
        loadedObject.tag = "Item";

        // 플레이어 위치에서 현재 로컬 Z 방향(forward)으로 0.5만큼 떨어진 위치 계산
        Vector3 spawnPosition = transform.parent.position + transform.parent.forward.normalized * 0.5f;
        loadedObject.transform.position = spawnPosition;
        loadedObject.transform.localScale = Vector3.one;

        // 오브젝트가 부모 오브젝트와 같은 방향을 바라보도록 회전 설정 (필요에 따라 설정)
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

        // 텍스처 파일이 있으면 로드하여 적용
        if (!string.IsNullOrEmpty(textureFilePath) && File.Exists(textureFilePath))
        {
            byte[] fileData = File.ReadAllBytes(textureFilePath);
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

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}