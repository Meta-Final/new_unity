using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FireStore : MonoBehaviour
{
    public static FireStore instance;

    FirebaseFirestore store;

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
}
