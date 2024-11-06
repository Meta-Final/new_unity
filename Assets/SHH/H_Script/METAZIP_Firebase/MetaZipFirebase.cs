using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[FirestoreData]
public class h_info
{
    [FirestoreProperty]
    public string name { get; set; }

    [FirestoreProperty]
    public string nickname { get; set; }
}

public class MetaZipFirebase : MonoBehaviour
{
    public static MetaZipFirebase instance;
    public FirebaseAuth auth;
    public FirebaseFirestore store;

    public InputField inputemail;
    public InputField inputpassword;
    public InputField inputpassword2;
    public InputField nameInput;
    public InputField nicknameInput;
    public InputField workplace;

   // public Button SignUpButton;
    public Text feedbackText;

    void Awake()
    {
        instance = this;
        //InitializeFirebase();
    }

   //private async void InitializeFirebase()
   //{
   //    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
   //    {
   //        DependencyStatus dependencyStatus = task.Result;
   //        if (dependencyStatus == DependencyStatus.Available)
   //        {
   //            auth = FirebaseAuth.DefaultInstance;
   //            store = FirebaseFirestore.DefaultInstance;
   //            Debug.Log("Firebase �ʱ�ȭ ����!");
   //        }
   //        else
   //        {
   //            Debug.LogError($"Firebase �ʱ�ȭ ����: {dependencyStatus}");
   //            feedbackText.text = "Firebase �ʱ�ȭ ����: " + dependencyStatus;
   //        }
   //    });
   //}

    public void OnClickSignUp()
    {
        SignUp();
        print("ȸ�������ߴ�");
    }

    public void SignUp()
    {
        string email = inputemail.text;
        string password = inputpassword.text;
        string confirmPassword = inputpassword2.text;
        string name = nameInput.text;
        string nickname = nicknameInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(name) ||
            string.IsNullOrEmpty(nickname))
        {
            feedbackText.text = "��ĭ�� �Է��ϼ���";
            return;
        }

        if (password != confirmPassword)
        {
            feedbackText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            return;
        }

        StartCoroutine(CoSignUp(email, password, name, nickname));

        h_info userInfo = new h_info
        {
            name = name,
            nickname = nickname
        };
        SaveUserInfo(userInfo);
    }

    IEnumerator CoSignUp(string email, string password, string name, string nickname)
    {
        Task<AuthResult> task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            Debug.Log("ȸ������ ����!");

        }
        else
        {
            Debug.LogError("ȸ������ ����!");
            foreach (var exception in task.Exception.Flatten().InnerExceptions)
            {
                Debug.LogError("ȸ������ ����: " + exception.Message);
                feedbackText.text += exception.Message + "\n"; // ��� ���� �޽��� ǥ��
            }
        }
    }

    public void SaveUserInfo(h_info userInfo)
    {
        StartCoroutine(CoSaveUserInfo(userInfo));
    }

    private IEnumerator CoSaveUserInfo(h_info userInfo)
    {
        if (auth.CurrentUser != null)
        {
            string path = "USER/" + auth.CurrentUser.UserId + "/������";
            Task task = store.Document(path).SetAsync(userInfo);

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception == null)
            {
                feedbackText.text = "���� ����!";
            }
            else
            {
                feedbackText.text = "Firestore ���� ����: " + task.Exception.Message;
            }
        }
        else
        {
            feedbackText.text = "����ڰ� �������� �ʾҽ��ϴ�.";
        }
    }
}
