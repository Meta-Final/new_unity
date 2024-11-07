using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FireAuthManager : MonoBehaviour
{
    public static FireAuthManager instance;

    public FirebaseAuth auth;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // �α��� ���� üũ �̺�Ʈ ���
        auth.StateChanged += OnChangedAuthState;
        
    }

    void OnChangedAuthState(object sender, EventArgs e)
    {
        // ���࿡ ���� ������ �ִٸ�
        if (auth.CurrentUser != null)
        {
            // �α���
            print("�α��� ����");
        }
        // �׷��� �ʴٸ�
        else
        {
            // �α׾ƿ�
            print("�α׾ƿ� ����");
        }
    }

    // ȸ�� ���� ����
    public void OnSignUp(string email, string password, UserInfo userInfo)
    {
        StartCoroutine(SignUp(email, password, userInfo));
    }

    IEnumerator SignUp(string email, string password, UserInfo userInfo)
    {
        // ȸ������ �õ�
        Task<AuthResult> task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        // ����� �Ϸ�� ������ ��ٸ���.
        yield return new WaitUntil( () => task.IsCompleted );
        // ���࿡ ���ܰ� ���ٸ�
        if (task.Exception == null)
        {
            print("ȸ������ ����");

            // ȸ������ ���� �� ���� ���� ����
            FireStore.instance.SaveUserInfo(userInfo);
        }
        else
        {
            print("ȸ������ ���� : " + task.Exception);
        }
    }

    // �α��� ����
    public void OnSignIn(string email, string password)
    {
        StartCoroutine(SignIn(email, password));
        
    }

    IEnumerator SignIn(string email, string password)
    {
        // �α��� �õ�
        Task<AuthResult> task = auth.SignInWithEmailAndPasswordAsync(email, password);
        // ����� �Ϸ�� ������ ��ٸ���.
        yield return new WaitUntil(() => task.IsCompleted);
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

    // �α׾ƿ� ����
    public void LogOut()
    {
        auth.SignOut();
    }
}
