using Firebase.Firestore;
using System;
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
    public void LoadUserInfo(Action<UserInfo> onComplete)
    {
        StartCoroutine(CoLoadUserInfo(onComplete)); 
    }

    IEnumerator CoLoadUserInfo(Action<UserInfo> onComplete)
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
            // �ҷ��� ������ ����
            if (onComplete != null)
            {
                onComplete(loadInfo);
            }
        }
        else
        {
            print("ȸ������ �ҷ����� ���� : " + task.Exception);
        }
    }
}