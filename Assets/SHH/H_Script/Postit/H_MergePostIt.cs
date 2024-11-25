using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class H_MergePostIt : MonoBehaviour
{
    public GameObject postit;
    public Transform noticepos;
    public Texture image;
    bool isCol = false;

    void Start()
    {
        image = GetComponentInChildren<RawImage>().texture;
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (H_DragManager.inst.isDragging && other.gameObject.name.Contains("PostItPink"))
        {
            if(H_DragManager.inst.currentPostIt == gameObject)
            {
                GameObject go = Instantiate(postit, transform.position, Quaternion.identity, noticepos);
                
                go.transform.localEulerAngles = new Vector3(180, 0, 90);
                go.transform.localScale = new Vector3(10, 10, 10);
                H_NewFolder hn = go.GetComponent<H_NewFolder>();
                hn.texs.Add(image);
                hn.texs.Add(other.transform.GetComponent<H_MergePostIt>().image);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }


    public Texture GetTexture(Texture tex)
    {
        tex = image;
        return tex;
    }
}
