using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class H_MergePostIt : MonoBehaviour
{
    public GameObject postit;
    public Transform noticepos;
    private Texture image;

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
            GameObject go = Instantiate(postit, transform.position, Quaternion.identity, noticepos);


            Destroy(gameObject);
        }
    }

    public Texture GetTexture(Texture tex)
    {
        tex = image;
        return tex;
    }
}
