using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class H_DargMg : MonoBehaviour
    {
        private Vector3 H_offset;
        private bool isDragging = false;
        public Transform noticePos;
        


        private void Start()
        {
            noticePos = transform.parent;
        }

        //�巡�� ���� �Լ�
        private void OnMouseDown()
        {
            isDragging = true;
            //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //H_offset = transform.position - mouseWorldPos;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void OnMouseDrag()
        {
            if (isDragging)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = transform.position.z; // Z�� ����
                Vector3 targetPosition = mouseWorldPos + H_offset;

                targetPosition = ClampToBounds(targetPosition);
                transform.position = targetPosition;
            }

        }

        private void OnMouseUp()
        {
            isDragging = false;
        }
        private Vector3 ClampToBounds(Vector3 targetPosition)
        {
            if (noticePos == null)
                return targetPosition;

            // noticePos�� ��谪 ���
            Bounds bounds = GetNoticePosBounds();

            // X, Y �� ����
            targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);

            return targetPosition;
        }

        private Bounds GetNoticePosBounds()
        {
            // noticePos�� �߽ɰ� ũ�⸦ ������� ��� ����
            Renderer renderer = noticePos.GetComponent<Renderer>();
            if (renderer != null)
            {
                return renderer.bounds;
            }

            // Renderer�� ���� ���, ��ü ��� ����
            Vector3 size = new Vector3(10f, 10f, 0); // X, Y ũ�� ����
            return new Bounds(noticePos.position, size);
        }
    }
