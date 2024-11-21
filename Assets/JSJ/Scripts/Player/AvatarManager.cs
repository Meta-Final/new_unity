using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager instance;

    public int avatarCode;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �ƹ�Ÿ ���� �ڵ� ����
    public void RandomAvatartCode()
    {
        int randomIndex = Random.Range(0, 2);

        avatarCode = randomIndex;
        print(avatarCode);
    }

}
