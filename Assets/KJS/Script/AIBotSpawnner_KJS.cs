using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBotSpawnner_KJS : MonoBehaviourPunCallbacks
{
    private GameObject specialObjectInstance;

    public override void OnJoinedRoom()
    {
        // �濡 ������ �÷��̾ �ڽ��� ������Ʈ�� �����ϵ��� �մϴ�.
        if (specialObjectInstance == null)
        {
            SpawnSpecialObject();
        }
    }

    private void SpawnSpecialObject()
    {
        // Resources �������� �������� ��Ʈ��ũ �� ����
        Vector3 spawnPosition = GetSpawnPosition();
        specialObjectInstance = PhotonNetwork.Instantiate("Helper", spawnPosition, Quaternion.identity, 0);
    }

    private Vector3 GetSpawnPosition()
    {
        // ���ϴ� ��ġ�� ������Ʈ�� �����ϱ� ���� ��ġ ���� ������ �߰��� �� �ֽ��ϴ�.
        // ���⼭�� ���÷� ���� ��ġ�� ����
        return new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }
}
