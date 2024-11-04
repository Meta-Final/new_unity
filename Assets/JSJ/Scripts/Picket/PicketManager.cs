using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicketManager : MonoBehaviour
{
    public GameObject picketPrefab;   
    public GameObject player;
    public GameObject panelLinkNews;   // Picket이랑 기사 링크 여부 Panel
    
    public Button btn_Picket;          // Picket 생성 버튼

    Vector3 picketPos;

    public PicketUIManager picketUIManager;

    void Start()
    {
        player = GameObject.Find("PlayerPrefab");

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
                picketPos = player.transform.position + player.transform.forward * 3f;

                GameObject obj = Instantiate(picketPrefab, picketPos, Quaternion.identity);
                obj.transform.LookAt(player.transform);
                picketUIManager.currentPicket = obj;

                // "피켓에 기사를 링크하시겠습니까?" UI 뜸
                panelLinkNews.SetActive(true);
            }
        }
    }
}
