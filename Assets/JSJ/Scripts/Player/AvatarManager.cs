using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager instance;

    public GameObject currentAvatar;

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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    

    public void RandomAvatartCode()
    {
        int randomIndex = Random.Range(0, 2);
        SaveAvatarCode(randomIndex);
    }

    public void SaveAvatarCode(int code)
    {
        avatarCode = code;
    }

    public void AssignRandomAvatar(int avatarInt)
    {
        if (currentAvatar == null)
        {
            currentAvatar = PhotonNetwork.Instantiate("Player_" + avatarInt, Vector3.zero, Quaternion.identity);

            print("아바타 생성완료!");
        }

    }
}
