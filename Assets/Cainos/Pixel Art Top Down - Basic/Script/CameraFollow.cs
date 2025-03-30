using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    // Let camera follow target and stay slightly ahead based on movement direction
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;
        public float aheadDistance = 2.0f; // Distance ahead of the player

        private Vector3 offset;
        private Rigidbody2D targetRigidbody;

        private void Start()
        {
            if (target == null)
            {
                Debug.LogError("CameraFollow script requires a target.");
                return;
            }

            // Calculate initial offset
            offset = transform.position - target.position;

            // Get the Rigidbody component from the target
            targetRigidbody = target.GetComponent<Rigidbody2D>();
            if (targetRigidbody == null)
            {
                Debug.LogError("Target does not have a Rigidbody component.");
            }
        }

        private void Update()
        {
            if (target == null || targetRigidbody == null) return;

            // Calculate the desired position ahead of the player
            Vector3 aheadOffset = targetRigidbody.linearVelocity.normalized * aheadDistance;
            Vector3 targetPos = target.position + offset + aheadOffset;

            // Smoothly interpolate to the target position
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }
    }
}
