using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class H_NewFolder : MonoBehaviour
{
    public static H_NewFolder inst;
    public List<Texture> texs = new List<Texture>();


    // ���� ��ũ������ ������ UI
    public GameObject Popup_merge; //ĵ����
    public Transform viewcontents; //��ũ�Ѻ�
    public GameObject viewRawimage; // RawImage

    //UI�ݱ� ��ư
    public Button Btn_close_mergeview;

    private void Awake()
    {
       inst = this;
    }
    void Start()
    {
        Popup_merge = GameObject.Find("Popup_merge");
        Popup_merge.SetActive(false);
        Btn_close_mergeview.onClick.AddListener(OnClickCloseContentview);

    }

     void Update()
     {
         
     }
    // UI�� ����
    public void MergeContentView()
    {
        // ������ ���� Ȱ��ȭ
        Popup_merge.gameObject.SetActive(true);
        
        // RawImage ����
        for (int i = 0; i < texs.Count; i++)
        {
            GameObject rawImageObj = Instantiate(viewRawimage, viewcontents);
            RawImage rawImage = rawImageObj.GetComponent<RawImage>();
            rawImage.texture = texs[i]; // �ؽ�ó �Ҵ�
        }

    }
   
    private void OnClickCloseContentview()
     {
        viewcontents.gameObject.SetActive(false);
     }
    }


