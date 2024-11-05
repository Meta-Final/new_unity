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
    
    //USER->USERID->������(�г���,�����)
    IEnumerator CoSaveUserInfo()
    {
        //���� ��� USER/USERID/������
        string path = "USER/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        //����������û
        Task task = store.Document(path).SetAsync(null);

        //����� �Ϸ�ɶ����� ��ٷ�!
        yield return new WaitUntil(() => { return task.IsCompleted; });

        if (task.Exception == null)
        {
            print("���强��!");
        }
        else
        {
            print("����! :" + task.Exception);
        }
    }
}
