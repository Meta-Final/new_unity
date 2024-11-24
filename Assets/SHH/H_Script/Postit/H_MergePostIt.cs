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

            if (isCol)
                return;

            GameObject go = Instantiate(postit, transform.position, Quaternion.identity, noticepos);
            go.transform.localEulerAngles = new Vector3(-180, -180, 0);

            //H_DragManager.inst.folders.Add(go);
            H_NewFolder hn = go.GetComponent<H_NewFolder>();
            hn.texs.Add(image);


            Destroy(gameObject);
            isCol = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (H_DragManager.inst.isDragging && other.gameObject.name.Contains("PostItPink"))
        {
            isCol = false;
            
            //H_DragManager.inst.GetGameObjs(gameObject);
            //H_DragManager.inst.SetFolder();
        }
        Destroy(gameObject);
    }

    public Texture GetTexture(Texture tex)
    {
        tex = image;
        return tex;
    }
}
