using Firebase.Firestore;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// 내정보 or 회원정보
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

    // 회원 정보 저장 진행
    public void SaveUserInfo(UserInfo info)
    {
        StartCoroutine(CoSaveUserInfo(info));
    }

    IEnumerator CoSaveUserInfo(UserInfo info)
    {
        // 저장 경로
        string path = "USER/" + FireAuthManager.instance.auth.CurrentUser.UserId;
        // 정보 저장 요청
        Task task = store.Document(path).SetAsync(info);
        // 통신이 완료될 때까지 기다린다.
        yield return new WaitUntil(() => task.IsCompleted);
        // 만약에 예외가 없다면
        if (task.Exception == null)
        {
            print("회원정보 저장 성공");
        }
        else
        {
            print("회원정보 저장 실패 : " + task.Exception);
        }
    }

    // 회원 정보 불러오기 진행
    public void LoadUserInfo()
    {
        StartCoroutine(CoLoadUserInfo()); 
    }

    IEnumerator CoLoadUserInfo()
    {
        // 저장 경로
        string path = "USER/" + FireAuthManager.instance.auth.CurrentUser.UserId;
        // 정보 조회 요청
        Task<DocumentSnapshot> task = store.Document(path).GetSnapshotAsync();
        // 통신이 완료될 때까지 기다린다.
        yield return new WaitUntil(() => task.IsCompleted);
        // 만약에 예외가 없다면
        if (task.Exception == null)
        {
            print("회원정보 불러오기 성공");

            // 불러온 정보를 UserInfo 변수에 저장
            UserInfo loadInfo = task.Result.ConvertTo<UserInfo>();

            // 불러온 정보 전달
            MetaConnectionMgr.instance.JoinLobby(loadInfo);
           
        }
        else
        {
            print("회원정보 불러오기 실패 : " + task.Exception);
        }
    }
}
