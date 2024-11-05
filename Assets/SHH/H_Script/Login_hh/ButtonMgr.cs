using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMgr : MonoBehaviour
{

 //  public TMP_InputField textName;

    public InputField inputemail;
    public InputField inputpassword;
    public InputField inputpassword2;

    //public GameObject Loginbox;
    public GameObject Joinbox;

    void Start()
    {
        Joinbox.SetActive(false);
    }

    void Update()
    {
        
    }
    public void OnClickJoinbox()
    {
        
        //print("회원가입창 까꿍");
        Joinbox.SetActive(true);
     
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
