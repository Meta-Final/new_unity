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
   //            Debug.Log("Firebase 초기화 성공!");
   //        }
   //        else
   //        {
   //            Debug.LogError($"Firebase 초기화 실패: {dependencyStatus}");
   //            feedbackText.text = "Firebase 초기화 실패: " + dependencyStatus;
   //        }
   //    });
   //}

    public void OnClickSignUp()
    {
        SignUp();
        print("회원가입했다");
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
            feedbackText.text = "빈칸을 입력하세요";
            return;
        }

        if (password != confirmPassword)
        {
            feedbackText.text = "비밀번호가 일치하지 않습니다.";
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
            Debug.Log("회원가입 성공!");

        }
        else
        {
            Debug.LogError("회원가입 실패!");
            foreach (var exception in task.Exception.Flatten().InnerExceptions)
            {
                Debug.LogError("회원가입 실패: " + exception.Message);
                feedbackText.text += exception.Message + "\n"; // 모든 오류 메시지 표시
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
            string path = "USER/" + auth.CurrentUser.UserId + "/내정보";
            Task task = store.Document(path).SetAsync(userInfo);

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception == null)
            {
                feedbackText.text = "저장 성공!";
            }
            else
            {
                feedbackText.text = "Firestore 저장 실패: " + task.Exception.Message;
            }
        }
        else
        {
            feedbackText.text = "사용자가 인증되지 않았습니다.";
        }
    }
}
