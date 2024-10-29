using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PicketManager : MonoBehaviour
{
    public GameObject picketPrefab;
    public GameObject player;

    Vector3 picketPos;

    void Start()
    {
        player = GameObject.Find("PlayerPrefab");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickPicket();
        }
        
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

            }
        }
    }

    // Picket 클릭시, 씬 이동
    public void ClickPicket()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.gameObject.layer == 18)
            {
                SceneManager.LoadScene("Meta_Picket_Scene");
            }
        }
    }
}
