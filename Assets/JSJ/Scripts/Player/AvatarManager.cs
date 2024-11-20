using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarManager : MonoBehaviour
{
    public GameObject[] avatars;
    public GameObject currentAvatar;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AssignRandomAvatar()
    {
        int randomIndex = Random.Range(0, avatars.Length);

        if (currentAvatar == null)
        {
            currentAvatar = PhotonNetwork.Instantiate(avatars[randomIndex].name, Vector3.zero, Quaternion.identity);
           
            print("아바타 생성완료!");

            DontDestroyOnLoad(currentAvatar);
        }
        
    }
}
