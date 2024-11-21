using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicketManager : MonoBehaviourPun
{
    public GameObject player;
     
    public GameObject panelLinkNews;   // Picket이랑 기사 링크 여부 Panel
    
    public Button btn_Picket;          // Picket 생성 버튼

    public PicketUIManager picketUIManager;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;
       
        btn_Picket.onClick.AddListener(MakePicket);

        panelLinkNews.SetActive(false);
    }

    void Update()
    {
        
    }

    // 지정된 구역에서만 Picket 생성 가능
    public void MakePicket()
    {
        Ray ray = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 'PicketZone' 에만 Picket 이 생성
            if (hit.collider.gameObject.layer == 17)
            {
                Vector3 picketPos = player.transform.position + player.transform.forward * 3f;
                Quaternion picketRot = Quaternion.LookRotation(player.transform.forward);

                GameObject obj = PhotonNetwork.Instantiate("Picket", picketPos, picketRot);

                picketUIManager.currentPicket = obj;

                if (player.GetComponent<PhotonView>().IsMine)
                {
                    panelLinkNews.SetActive(true);
                }
            }
        }
    }
}
