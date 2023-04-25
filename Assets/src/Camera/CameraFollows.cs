using System;
using UnityEngine;

    public class CameraFollows : MonoBehaviour
    {
        public Transform[] targets;

        void LateUpdate()
        {
            float averageX = 0;
            float averageY = 0;

            foreach (Transform target in targets)
            {
                averageX += target.position.x;
                averageY += target.position.y;
            }

            averageX /= targets.Length;
            averageY /= targets.Length;

            transform.position = new Vector3(averageX, averageY, transform.position.z);
        }
    }
