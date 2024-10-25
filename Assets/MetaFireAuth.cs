using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MetaFireAuth : MonoBehaviour
{

    public static MetaFireAuth instance;

    public FirebaseAuth auth;

    private void Awake()
    {
        instance = this;

     
    }
    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        //�α��� ���� üũ �̺�Ʈ ���
        auth.StateChanged += OnChangedAuthState;
    } 

    void OnChangedAuthState(object sender, EventArgs e)
    {
        //���࿡ ���� ��������(������)�� �ִٸ� �α���
        if(auth.CurrentUser != null)
        {
            print("�α��� �Ǿ����ϴ�.");
        }
        //�׷��� ������ �α׾ƿ�
        else
        {
            print("�α� �ƿ� �Ǿ����ϴ�.");
            SignIn("a@gmail.com", "123456");
        }
    }

    public void SignUp(string email, string password)
    {
        StartCoroutine(CoSignUp(email, password));
    }

    IEnumerator CoSignUp(string email, string password)
    {
        // ȸ������ �õ�
        Task<AuthResult> task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        // ����� �Ϸ�ɶ����� ��ٸ���.
        yield return new WaitUntil(() => { return task.IsCompleted; });

        // ���࿡ ���ܰ� ���ٸ�
        if(task.Exception == null)
        {
            print("ȸ������ ����");
        }
        else
        {
            print("ȭ������ ���� : " + task.Exception);
        }
     }

    public void SignIn(string email, string password)
    {
        StartCoroutine(CoSignIn(email, password));
    }

    IEnumerator CoSignIn(string email, string password)
    {
        // �α��� �õ�
        Task<AuthResult> task = auth.SignInWithEmailAndPasswordAsync(email, password);
        // ����� �Ϸ�ɶ����� ��ٸ���.
        yield return new WaitUntil(() => { return task.IsCompleted; });

        // ���࿡ ���ܰ� ���ٸ�
        if (task.Exception == null)
        {
            print("�α��� ����");
        }
        else
        {
            print("�α��� ���� : " + task.Exception);
        }
    }

    public void SignOut()
    {
        auth.SignOut();
    }
}
