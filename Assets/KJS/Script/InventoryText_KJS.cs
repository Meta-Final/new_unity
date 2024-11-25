using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using Photon.Pun;

public class InventoryText_KJS : MonoBehaviour
{
    public List<string> inventoryPostIds = new List<string>(); // 인스펙터에서 할당할 postId 리스트
    public string inventorySlotPrefabPath = "Prefabs/InventorySlotUI"; // Resources 폴더 내 프리팹 경로
    public Transform inventoryPanel;

    private List<GameObject> inventorySlots = new List<GameObject>(); // 인벤토리 슬롯 UI 목록
    private Dictionary<string, H_PostInfo> postInfoDict = new Dictionary<string, H_PostInfo>(); // postId별로 아이템 데이터를 저장할 딕셔너리

    private int previousItemCount = 0; // 이전 inventoryPostIds 개수를 저장하여 변화 감지
    private string baseDirectory = Application.dataPath; // 기본 저장 경로

    void Start()
    {
        StartCoroutine(LoadPostInfoFromFolders());
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

    // 각 postId별 폴더에서 JSON 데이터를 로드하여 PostInfoList를 초기화하는 코루틴
    private IEnumerator LoadPostInfoFromFolders()
    {
        if (Directory.Exists(baseDirectory))
        {
            string[] postDirectories = Directory.GetDirectories(baseDirectory);

            foreach (string postDirectory in postDirectories)
            {
                string jsonFilePath = Path.Combine(postDirectory, "thumbnail.json");

                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    PostInfoList postInfoList = JsonUtility.FromJson<PostInfoList>(jsonContent);

                    // postData 리스트를 딕셔너리에 저장하여 빠르게 조회할 수 있도록 준비
                    foreach (H_PostInfo post in postInfoList.postData)
                    {
                        postInfoDict[post.postid] = post;
                    }
                }
            }

            // 초기 인벤토리 설정
            UpdateInventorySlots();
        }
        yield return null;
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
            // Resources 폴더에서 프리팹 로드 (PhotonNetwork.Instantiate에서는 경로에 Resources를 포함하지 않습니다)
            if (Resources.Load<GameObject>(inventorySlotPrefabPath) == null)
            {
                Debug.LogError("Inventory Slot UI 프리팹을 Resources 폴더에서 찾을 수 없습니다. 경로를 확인하세요: " + inventorySlotPrefabPath);
                return;
            }

            // 새로운 슬롯 UI 생성 (PhotonNetwork.Instantiate 사용)
            GameObject newSlot = PhotonNetwork.Instantiate(inventorySlotPrefabPath, inventoryPanel.position, Quaternion.identity);
            newSlot.transform.SetParent(inventoryPanel, false); // inventoryPanel의 자식으로 설정
            inventorySlots.Add(newSlot);

            // 해당 postInfo의 thumburl을 사용하여 이미지를 로드하여 슬롯에 설정
            StartCoroutine(LoadAndSetThumbnail(postInfo, newSlot));
        }
        else
        {
            Debug.LogWarning("postId에 해당하는 데이터를 찾을 수 없습니다: " + postId);
        }
    }

    // 이미지 파일을 로드하여 슬롯에 설정하는 코루틴
    private IEnumerator LoadAndSetThumbnail(H_PostInfo postInfo, GameObject slot)
    {
        string filePath = "file:///" + postInfo.thumburl; // 각 postId별 폴더 내 이미지 파일 경로
        Debug.Log("baseDirectory: " + baseDirectory);
        Debug.Log("filePath: " + filePath);

        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(filePath);
        Debug.LogError("이미지 로드 실패: " + textureRequest.error);
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