using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MetazipDatabase : MonoBehaviour
{
    public static MetazipDatabase instance;
    public FirebaseFirestore store;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        store = FirebaseFirestore.DefaultInstance;
    }
    void Update()
    {
        
    }

    public void SaveUserInfo()
    {
        StartCoroutine(CoSaveUserInfo());
    }
    
    //USER->USERID->내정보(닉네임,직장명)
    IEnumerator CoSaveUserInfo()
    {
        //저장 경로 USER/USERID/내정보
        string path = "USER/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        //저장정보요청
        Task task = store.Document(path).SetAsync(null);

        //통신이 완료될때까지 기다려!
        yield return new WaitUntil(() => { return task.IsCompleted; });

        if (task.Exception == null)
        {
            print("저장성공!");
        }
        else
        {
            print("실패! :" + task.Exception);
        }
    }
}
