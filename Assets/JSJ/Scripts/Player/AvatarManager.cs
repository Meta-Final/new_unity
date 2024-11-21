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

        
        avatarCode = randomIndex;
        print(avatarCode);
    }

    public void LoadandSetAvatarCode()
    {
        
        print("아바타생성시작");
        PhotonNetwork.Instantiate("Player_" + avatarCode, Vector3.zero, Quaternion.identity);
        print("아바타 생성완료!");
    }

}
