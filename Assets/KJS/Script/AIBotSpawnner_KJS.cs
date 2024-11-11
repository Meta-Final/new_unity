using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBotSpawnner_KJS : MonoBehaviourPunCallbacks
{
    private GameObject specialObjectInstance;

    public override void OnJoinedRoom()
    {
        // 방에 입장한 플레이어가 자신의 오브젝트를 생성하도록 합니다.
        if (specialObjectInstance == null)
        {
            SpawnSpecialObject();
        }
    }

    private void SpawnSpecialObject()
    {
        // Resources 폴더에서 프리팹을 네트워크 상에 생성
        Vector3 spawnPosition = GetSpawnPosition();
        specialObjectInstance = PhotonNetwork.Instantiate("Helper", spawnPosition, Quaternion.identity, 0);
    }

    private Vector3 GetSpawnPosition()
    {
        // 원하는 위치에 오브젝트를 생성하기 위해 위치 설정 로직을 추가할 수 있습니다.
        // 여기서는 예시로 랜덤 위치를 설정
        return new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }
}
