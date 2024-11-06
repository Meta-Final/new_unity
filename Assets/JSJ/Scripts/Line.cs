using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour, IPunObservable
{
    public LineRenderer lineRenderer;
    public List<Vector3> points = new List<Vector3>();

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }
    
    public void AddPoint(Vector3 point)
    {
        points.Add(point);

        UpdateLineRenderer();

    }

    public void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(points.Count);

            foreach (Vector3 point in points)
            {
                stream.SendNext(point);
            }
        }
        else
        {
            int count = (int)stream.ReceiveNext();

            for (int i = 0; i < count; i++)
            {
                points.Add((Vector3)stream.ReceiveNext());
            }

            UpdateLineRenderer();
        }
    }

    
}
