using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;

public class InventoryText_KJS : MonoBehaviourPun
{
    public List<string> inventoryPostIds = new List<string>(); // 인스펙터에서 할당할 postId 리스트
    public string inventorySlotPrefabPath = "Prefabs/InventorySlotUI"; // Resources 폴더 내 프리팹 경로
    public Transform inventoryPanel;

    private List<GameObject> inventorySlots = new List<GameObject>(); // 인벤토리 슬롯 UI 목록
    private Dictionary<string, H_PostInfo> postInfoDict = new Dictionary<string, H_PostInfo>(); // postId별로 아이템 데이터를 저장할 딕셔너리

    private int previousItemCount = 0; // 이전 inventoryPostIds 개수를 저장하여 변화 감지

    void Start()
    {
        StartCoroutine(LoadPostInfoFromJson());
    }

    void Update()
    {
        // inventoryPostIds의 개수 변화 감지
        if (inventoryPostIds.Count != previousItemCount)
        {
            UpdateInventorySlots();
            previousItemCount = inventoryPostIds.Count;
        }
    }

    // JSON 데이터를 로드하여 PostInfoList를 초기화하는 코루틴
    private IEnumerator LoadPostInfoFromJson()
    {
        string jsonFilePath = "file:///C:/Users/Admin/Documents/GitHub/new_unity/Assets/KJS/UserInfo/thumbnail.json";

        UnityWebRequest request = UnityWebRequest.Get(jsonFilePath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PostInfoList postInfoList = JsonUtility.FromJson<PostInfoList>(request.downloadHandler.text);

            // postData 리스트를 딕셔너리에 저장하여 빠르게 조회할 수 있도록 준비
            foreach (H_PostInfo post in postInfoList.postData)
            {
                postInfoDict[post.postid] = post;
            }

            // 초기 인벤토리 설정
            UpdateInventorySlots();
        }
        else
        {
            Debug.LogError("JSON 파일을 불러오는 데 실패했습니다: " + request.error);
        }
    }

    // 인벤토리 슬롯 UI를 업데이트하는 메서드
    private void UpdateInventorySlots()
    {
        // 기존 슬롯 UI 삭제
        foreach (GameObject slot in inventorySlots)
        {
            Destroy(slot);
        }
        inventorySlots.Clear();

        // inventoryPostIds 리스트를 바탕으로 새로운 슬롯 UI 생성
        foreach (string postId in inventoryPostIds)
        {
            AddItemToInventory(postId);
        }
    }

    // 특정 postId 아이템을 인벤토리에 추가하는 메서드
    public void AddItemToInventory(string postId)
    {
        // 해당 postId가 딕셔너리에 있는지 확인
        if (postInfoDict.TryGetValue(postId, out H_PostInfo postInfo))
        {
            // PhotonNetwork.Instantiate로 네트워크를 통해 프리팹 생성
            GameObject newSlot = PhotonNetwork.Instantiate(inventorySlotPrefabPath, Vector3.zero, Quaternion.identity);
            if (newSlot == null)
            {
                Debug.LogError("Inventory Slot UI 프리팹을 Resources 폴더에서 찾을 수 없습니다. 경로를 확인하세요: " + inventorySlotPrefabPath);
                return;
            }

            // 슬롯 UI를 인벤토리 패널 아래에 배치
            newSlot.transform.SetParent(inventoryPanel, false);
            inventorySlots.Add(newSlot);

            // 해당 postInfo의 thumburl을 사용하여 이미지를 로드하여 슬롯에 설정
            StartCoroutine(LoadAndSetThumbnail(postInfo.thumburl, newSlot));
        }
        else
        {
            Debug.LogWarning("postId에 해당하는 데이터를 찾을 수 없습니다: " + postId);
        }
    }

    // 이미지 파일을 로드하여 슬롯에 설정하는 코루틴
    private IEnumerator LoadAndSetThumbnail(string imagePath, GameObject slot)
    {
        string filePath = "file:///" + imagePath; // 로컬 파일 경로

        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(filePath);
        yield return textureRequest.SendWebRequest();

        if (textureRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(textureRequest);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            Image slotImage = slot.GetComponentInChildren<Image>();
            if (slotImage != null)
            {
                slotImage.sprite = sprite; // 이미지 컴포넌트에 스프라이트 설정
            }
        }
        else
        {
            Debug.LogError("이미지 로드 실패: " + textureRequest.error);
        }
    }
}