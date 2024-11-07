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
}
