using Photon.Pun;
using UnityEngine;

public class AIBotSpawner_KJS : MonoBehaviourPunCallbacks
{
    public string helperResourceName = "Helper"; // Resources 폴더에 있는 프리팹 이름

    private void Start()
    {
        SpawnHelper(); // 씬이 시작될 때 Helper 프리팹 생성
    }

    private void SpawnHelper()
    {
        GameObject helperPrefab = (GameObject)Resources.Load(helperResourceName);
        if (helperPrefab != null)
        {
            PhotonNetwork.Instantiate(helperResourceName, Vector3.zero, Quaternion.identity); // Helper 프리팹을 네트워크 상에 생성
        }
        else
        {
            Debug.LogWarning("Helper 프리팹을 Resources 폴더에서 찾을 수 없습니다.");
        }
    }
}