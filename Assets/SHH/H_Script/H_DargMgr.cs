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

        //드래그 가능 함수
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
                mouseWorldPos.z = transform.position.z; // Z축 고정
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

            // noticePos의 경계값 계산
            Bounds bounds = GetNoticePosBounds();

            // X, Y 축 제한
            targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);

            return targetPosition;
        }

        private Bounds GetNoticePosBounds()
        {
            // noticePos의 중심과 크기를 기반으로 경계 생성
            Renderer renderer = noticePos.GetComponent<Renderer>();
            if (renderer != null)
            {
                return renderer.bounds;
            }

            // Renderer가 없을 경우, 대체 경계 설정
            Vector3 size = new Vector3(10f, 10f, 0); // X, Y 크기 가정
            return new Bounds(noticePos.position, size);
        }
    }
