using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticker : MonoBehaviour
{
    Vector2 position;
    RectTransform rectTransform;
    public GameObject stickerParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

    }

    void Start()
    {
        
        
    }

    public void UpdateSticker(Vector2 newPosition)
    {
        position = newPosition;
        rectTransform.anchoredPosition = position;

    }

    /*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(position);
        }
        else
        {
            position = (Vector2)stream.ReceiveNext();

            rectTransform.anchoredPosition = position;
        }
    } */

}
