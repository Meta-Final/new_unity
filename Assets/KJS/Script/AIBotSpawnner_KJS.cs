using Photon.Pun;
using UnityEngine;

public class AIBotSpawner_KJS : MonoBehaviourPunCallbacks
{
    public string helperResourceName = "Helper"; // Resources ������ �ִ� ������ �̸�

    private void Start()
    {
        SpawnHelper(); // ���� ���۵� �� Helper ������ ����
    }

    private void SpawnHelper()
    {
        GameObject helperPrefab = (GameObject)Resources.Load(helperResourceName);
        if (helperPrefab != null)
        {
            PhotonNetwork.Instantiate(helperResourceName, Vector3.zero, Quaternion.identity); // Helper �������� ��Ʈ��ũ �� ����
        }
        else
        {
            Debug.LogWarning("Helper �������� Resources �������� ã�� �� �����ϴ�.");
        }
    }
}