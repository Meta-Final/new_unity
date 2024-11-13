using Firebase.Firestore;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// ������ or ȸ������
[FirestoreData]
public class UserInfo
{
    [FirestoreProperty]
    public string userId { get; set; }
    [FirestoreProperty]
    public string name { get; set; }
    [FirestoreProperty]
    public string nickName { get; set; }
    [FirestoreProperty]
    public int userBirth { get; set; }
}

public class FireStore : MonoBehaviour
{
    public static FireStore instance;

    FirebaseFirestore store;

    public UserInfo userInfo;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        store = FirebaseFirestore.DefaultInstance;
    }

    // ȸ�� ���� ���� ����
    public void SaveUserInfo(UserInfo info)
    {
        StartCoroutine(CoSaveUserInfo(info));
    }

    IEnumerator CoSaveUserInfo(UserInfo info)
    {
        // ���� ���
        string path = "USER/" + FireAuthManager.instance.auth.CurrentUser.UserId;
        // ���� ���� ��û
        Task task = store.Document(path).SetAsync(info);
        // ����� �Ϸ�� ������ ��ٸ���.
        yield return new WaitUntil(() => task.IsCompleted);
        // ���࿡ ���ܰ� ���ٸ�
        if (task.Exception == null)
        {
            print("ȸ������ ���� ����");
        }
        else
        {
            print("ȸ������ ���� ���� : " + task.Exception);
        }
    }

    // ȸ�� ���� �ҷ����� ����
    public void LoadUserInfo()
    {
        StartCoroutine(CoLoadUserInfo()); 
    }

    IEnumerator CoLoadUserInfo()
    {
        // ���� ���
        string path = "USER/" + FireAuthManager.instance.auth.CurrentUser.UserId;
        // ���� ��ȸ ��û
        Task<DocumentSnapshot> task = store.Document(path).GetSnapshotAsync();
        // ����� �Ϸ�� ������ ��ٸ���.
        yield return new WaitUntil(() => task.IsCompleted);
        // ���࿡ ���ܰ� ���ٸ�
        if (task.Exception == null)
        {
            print("ȸ������ �ҷ����� ����");

            // �ҷ��� ������ UserInfo ������ ����
            UserInfo loadInfo = task.Result.ConvertTo<UserInfo>();

            string userId = loadInfo.userId;
            string userNickName = loadInfo.nickName;

            SaveUserInfo(userId, userNickName);

            // �ҷ��� ���� ����
            MetaConnectionMgr.instance.JoinLobby(loadInfo);

        }
        else
        {
            print("ȸ������ �ҷ����� ���� : " + task.Exception);
        }
    }

    // ���� ���̵� / �г��� �����ϴ� �Լ�
    public void SaveUserInfo(string userId, string userNickName)
    {
        PlayerPrefs.SetString("UserId", userId);
        PlayerPrefs.SetString("Nickname", userNickName);
        PlayerPrefs.Save();
        print("���� ���̵�� �г����� ����Ǿ����ϴ�.");

    }

    public UserInfo GetUserInfo()
    {
        return userInfo;
    }

    // ���� ���̵� / �г����� �ҷ����� �Լ�
    //public void GetUserId()
    //{
    //    string userId = PlayerPrefs.GetString("UserId", "DefaultUserId");
    //    string userNickName = PlayerPrefs.GetString("Nickname", "DefaultNickname");
    //}

}
