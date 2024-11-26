using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class H_NewFolder : MonoBehaviour
{
    public static H_NewFolder inst;
    public List<Texture> texs = new List<Texture>();


    // 담은 스크린샷을 보여줄 UI
    public GameObject Popup_merge; //캔버스
    public Transform viewcontents; //스크롤뷰
    public GameObject viewRawimage; // RawImage

    //UI닫기 버튼
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
    // UI에 띄우기
    public void MergeContentView()
    {
        // 콘텐츠 영역 활성화
        Popup_merge.gameObject.SetActive(true);
        
        // RawImage 생성
        for (int i = 0; i < texs.Count; i++)
        {
            GameObject rawImageObj = Instantiate(viewRawimage, viewcontents);
            RawImage rawImage = rawImageObj.GetComponent<RawImage>();
            rawImage.texture = texs[i]; // 텍스처 할당
        }

    }
   
    private void OnClickCloseContentview()
     {
        viewcontents.gameObject.SetActive(false);
     }
    }


