using System.Collections;
using UnityEngine;
using UnityEngine.XR.WSA;

namespace HoloBrain.IsolateMode
{
    public class FlyToObject : MonoBehaviour
    {
        public bool InPosition
        {
            get
            {
                return inPosition;
            }
        }

        public bool Presenting
        {
            get
            {
                return presenting;
            }
        }

        public float PresentationDistance = 1f;
        public float TravelTime = 1f;
        public AnimationCurve SmoothPosition = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public Transform TransformStructure;
        public GameObject TargetMainBrain;

        Vector3 initialPosition;
        Quaternion initialRotation;
        Vector3 initialScale;

        bool presenting = false;
        bool inPosition = false;
        float normalizedProgress;
        float startTime;
        Vector3 targetPosition;
        Vector3 targetScale;

        public void Present()
        {

            if (presenting)
            {
                this.gameObject.SetActive(true);
                StartCoroutine(ReturnOverTime());
                return;
            }
            presenting = true;
            StartCoroutine(PresentOverTime());
        }

        IEnumerator PresentOverTime()
        {

            if (TransformStructure == null)
            {
                TransformStructure = transform;
            }

            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
            initialScale = transform.localScale;


            Quaternion targetRotation = TargetMainBrain.transform.rotation;
            targetPosition = TargetMainBrain.transform.position;
            targetScale = TargetMainBrain.transform.localScale;

            inPosition = false;

            normalizedProgress = 0f;
            startTime = Time.time;

            TargetMainBrain.gameObject.SetActive(true);
            while (!inPosition)
            {
                // Move the object directly in front of player
                normalizedProgress = (Time.time - startTime) / TravelTime;
                TransformStructure.localPosition = Vector3.Lerp(initialPosition, targetPosition, SmoothPosition.Evaluate(normalizedProgress));
                TransformStructure.localScale = Vector3.Lerp(initialScale, targetScale, SmoothPosition.Evaluate(normalizedProgress));
                inPosition = Vector3.Distance(TransformStructure.localPosition, targetPosition) < 0.05f;

                yield return null;
            }

            this.gameObject.SetActive(false);
            yield return null;
        }

        IEnumerator ReturnOverTime()
        {
            // Move back to our initial position
            //inPosition = false;

            TargetMainBrain.gameObject.SetActive(false);
            targetPosition = TargetMainBrain.transform.position;
            normalizedProgress = 0f;
            startTime = Time.time;
            while (normalizedProgress < 1f)
            {
                normalizedProgress = (Time.time - startTime) / TravelTime;
                TransformStructure.localPosition = Vector3.Lerp(targetPosition, initialPosition, SmoothPosition.Evaluate(normalizedProgress));
                TransformStructure.localScale = Vector3.Lerp(targetScale, initialScale, SmoothPosition.Evaluate(normalizedProgress));
                inPosition = Vector3.Distance(TransformStructure.localPosition, initialPosition) < 0.05f;
                yield return null;
            }
            TransformStructure.localPosition = initialPosition;
            TransformStructure.localRotation = initialRotation;
            TransformStructure.localScale = initialScale;

            presenting = false;

            this.gameObject.SetActive(true);
            Debug.Log("I'm back");
            yield return null;
        }
    }
}