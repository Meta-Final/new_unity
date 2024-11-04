using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMgr : MonoBehaviour
{
 //  public TMP_InputField texttId;
 //  public TMP_InputField textPassword;
 //  public TMP_InputField textPassword2;
 //  public TMP_InputField textName;

    public InputField inputemail;
    public InputField inputpassword;

    //public GameObject Loginbox;
    public GameObject Joinbox;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickSignbox()
    {
        
        print("회원가입창 까꿍");
    }
    public void OnClickSignUp()
    {
        MetazipAuth.instance.SignUp(inputemail.text, inputpassword.text);

      
    }

    public void OnClickSignIn()
    {
        MetazipAuth.instance.SignIn(inputemail.text, inputpassword.text);
    }

    public void OnClickSignOut()
    {
        MetazipAuth.instance.SignOut();
    }
}
