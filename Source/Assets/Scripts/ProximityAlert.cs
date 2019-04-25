using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

namespace Accessibility_Locator

{
    [AddComponentMenu("OffScreen/OffScreen Indicator")]
    [ExecuteInEditMode]
    [Serializable]
    public class ProximityAlert : MonoBehaviour
    {

        [HideInInspector()]


        public enum mode_enum { SCREEN = 1, DISC = 2 };


        public mode_enum mode = mode_enum.SCREEN;

        public bool autoSetRadius = true;

        public float radius = 0f;

        public bool displayGizmo = false;

        public Image pointer;

        ///  (optional) 
        public Image target;

        public Camera cam;

        public float offsetRotation = 0f;

        public Vector2 padding = new Vector2(90f, 90f);

        //public float myProximityThreshold = 2.0f;
        //public float myCurrentDistanceFromTarget = 0.0f;


        [SerializeField]
        public UnityEvent OnEnterScreen;

        [SerializeField]
        public UnityEvent OnExitScreen;

        private Vector2 screen;
        private float pointerCanvasScaleFactor
        {
            get
            {
                try
                {
                    return pointer.GetComponentInParent<Canvas>().scaleFactor;
                }
                catch { return 1f; }
            }
        }
        private float targetCanvasScaleFactor
        {
            get
            {
                try
                {
                    return target.GetComponentInParent<Canvas>().scaleFactor;
                }
                catch { return 1f; }
            }
        }
        /*
        protected virtual void Start()
        {
        }
        */

        protected virtual void LateUpdate()
        {
            if (!Application.isPlaying) return;
            //myCurrentDistanceFromTarget = Vector3.Distance(cam.transform.position, transform.position);


            try
            {
                if (cam == null) cam = Camera.main;
                if (pointer == null && target == null) return;
                if (pointer == null) return;
                Canvas canvas = pointer.GetComponentInParent<Canvas>();
                switch (canvas.GetComponent<CanvasScaler>().uiScaleMode)
                {
                    default:
                    case CanvasScaler.ScaleMode.ConstantPixelSize:
                        //screen = new Vector2(Screen.width, Screen.height);
                        screen = new Vector2(cam.pixelWidth, cam.pixelHeight);
                        //screen = GetMainGameViewSize();
                        break;
                    case CanvasScaler.ScaleMode.ConstantPhysicalSize:
                        Debug.LogError("Proximity Alert doesn't support Constant physical size canvas scaler mode.");
                        return;
                        break;
                    case CanvasScaler.ScaleMode.ScaleWithScreenSize:
                        Debug.LogError("Proximity Alert doesn't support Scale with screen size canvas scaler mode.");
                        return;
                        break;
                }
                // Get the GameObject screen coordinates (zero centered)
                Vector3 vPos = cam.WorldToScreenPoint(this.transform.position) - new Vector3(screen.x * .5f, screen.y * .5f, 0f);
                // Knowing if the target is behind or in front of the camera.
                float angle = Vector3.Angle(-cam.transform.forward, cam.transform.position - this.transform.position);
                // Get angle between center of screen and the target.
                float rotation = Mathf.Atan2(vPos.y, vPos.x) * Mathf.Rad2Deg;
                float computedAngle = rotation + (angle > 90f ? 180f : 0f);
                computedAngle %= 360f;
                // ----------- Set bounds
                Vector4 onScreenBounds = Vector4.zero;
                bool OnScreen = false;
                ComputeRadius();
                // When inbound
                switch (mode)
                {
                    default:
                    case mode_enum.SCREEN:
                        onScreenBounds = new Vector4(-screen.x * .5f + padding.x, screen.x * .5f - padding.x, -screen.y * .5f + padding.y, screen.y * .5f - padding.y);
                        OnScreen = (vPos.x >= onScreenBounds.x && vPos.x <= onScreenBounds.y) && (vPos.y >= onScreenBounds.z && vPos.y <= onScreenBounds.w);
                        break;
                    case mode_enum.DISC:
                        OnScreen = Mathf.Pow((vPos.x - 0f), 2) + Mathf.Pow((vPos.y - 0f), 2) < Mathf.Pow(radius, 2);
                        break;
                }
                // -------------------
                if (OnScreen && angle < cam.fieldOfView)
                {
                    // =============== OnScreen position
                    if (target != null)
                    {
                        target.gameObject.SetActive(true);

                        // ************************* @#$%^&*&^%$#@#$%^&*&^%$#@#$%^&*&^%$#$%^& *************************                       
                        //if (myCurrentDistanceFromTarget < myProximityThreshold)
                        //{                                     
                        //}
                        //Debug.Log(vPos + "  /  "+targetCanvasScaleFactor);
                        Vector2 s = screen * .5f - new Vector2(screen.x * target.transform.parent.GetComponent<RectTransform>().pivot.x, screen.y * target.transform.parent.GetComponent<RectTransform>().pivot.y);
                        target.rectTransform.localPosition = (vPos * targetCanvasScaleFactor) + new Vector3(s.x, s.y);
                    }
                    if (pointer != null && pointer.gameObject.activeSelf)
                    {
                        // ************************* @#$%^&*&^%$#@#$%^&*&^%$#@#$%^&*&^%$#$%^& *************************                       
                        //if (myCurrentDistanceFromTarget < myProximityThreshold)
                        //{
                        OnEnterScreen.Invoke();
                        pointer.gameObject.SetActive(false);
                        //}
                    }
                }
                else
                {
                    // ============== OffScreen position
                    Vector2 position_screen = Vector2.zero;
                    Vector2 ScreenPos = Vector2.zero;
                    switch (mode)
                    {
                        default:
                        case mode_enum.SCREEN:

                            position_screen = ComputePointerPosition(angle > 90f);
                            // Debug.Log("position_screen : " + position_screen);
                            // ************************* @#$%^&*&^%$#@#$%^&*&^%$#@#$%^&*&^%$#$%^& *************************                       
                            //if (myCurrentDistanceFromTarget < myProximityThreshold)
                            //{
                            ScreenPos = new Vector2((screen.x - pointer.preferredWidth - padding.x) * position_screen.x, (screen.y - pointer.preferredHeight - padding.y) * position_screen.y);

                            //}
                            // ************************* @#$%^&*&^%$#@#$%^&*&^%$#@#$%^&*&^%$#$%^& *************************
                            //ScreenPos = new Vector2((screen.x - pointer.preferredWidth - padding.x) * position_screen.x, (screen.y - pointer.preferredHeight - padding.y) * position_screen.y);
                            break;
                        case mode_enum.DISC:
                            Vector2 s = screen * .5f - new Vector2(screen.x * pointer.transform.parent.GetComponent<RectTransform>().pivot.x, screen.y * pointer.transform.parent.GetComponent<RectTransform>().pivot.y);
                            ScreenPos = new Vector2(s.x + radius * Mathf.Sin((computedAngle + 90f) * Mathf.Deg2Rad), s.y + radius * Mathf.Cos((computedAngle - 90f) * Mathf.Deg2Rad));
                            break;
                    }
                    if (pointer != null)
                    {
                        pointer.rectTransform.localRotation = Quaternion.Euler(0f, 0f, computedAngle + offsetRotation);
                        if (float.IsInfinity(Mathf.Abs(ScreenPos.x)) || float.IsInfinity(Mathf.Abs(ScreenPos.y)))
                            return;
                        //Debug.Log(ScreenPos);
                        pointer.rectTransform.localPosition = ScreenPos * pointerCanvasScaleFactor;
                        if (!pointer.gameObject.activeSelf)
                            pointer.gameObject.SetActive(true);
                    }
                    if (target != null && target.gameObject.activeSelf)
                    {
                        OnExitScreen.Invoke();
                        target.gameObject.SetActive(false);
                    }
                }
            }
            catch (Exception e) { Debug.LogError("error OffscreenIndicator : " + e.Message); }
        }

        private void WaitForSeconds(float v)
        {
            throw new NotImplementedException();
        }

        private Vector2 ComputePointerPosition(bool behind)
        {

            float x;
            float y;
            Vector3 vPos = cam.WorldToViewportPoint(this.transform.position) - new Vector3(.5f, .5f, 0f);
            vPos = behind ? -vPos : vPos;
            float m = vPos.y / vPos.x;
            //Debug.Log(vPos);
            if (vPos.x > 0)
            {
                // Right side of the screen
                if (vPos.y > 0)
                {
                    // on top of the screen
                    x = 0.5f / m;
                    if (x > 0.5f)
                    {
                        // offscreen, we're getting the left side position
                        return new Vector2(0.5f, 0.5f * m);
                    }
                    else
                    {
                        // we're getting to top position
                        return new Vector2(0.5f / m, 0.5f);
                    }
                }
                else
                {
                    //  at the bottom of the screen
                    x = -0.5f / m;
                    if (x > 0.5f)
                    {
                        // offscreen, we're getting the left side position
                        return new Vector2(0.5f, 0.5f * m);
                    }
                    else
                    {
                        // we're getting to bottom position
                        return new Vector2(-0.5f / m, -0.5f);
                    }
                }
            }
            else
            {
                // Left side of the screen
                if (vPos.y > 0)
                {
                    // Top of the screen
                    x = 0.5f / m;
                    if (x < -0.5f)
                    {
                        // offscreen we're getting the right side position
                        return new Vector2(-0.5f, -0.5f * m);
                    }
                    else
                    {
                        // we're getting the top position
                        return new Vector2(0.5f / m, 0.5f);
                    }
                }
                else
                {
                    //  at the bottom of the screen
                    x = -0.5f / m;
                    if (x < -0.5f)
                    {
                        // offscreen, we're getting the right side position
                        return new Vector2(-0.5f, -0.5f * m);
                    }
                    else
                    {
                        // we're getting the bottom position
                        return new Vector2(-0.5f / m, -0.5f);
                    }
                }
                return Vector2.zero;
            }
        }

        public void ComputeRadius()
        {
            if (autoSetRadius)
            {
                if (Screen.width > Screen.height)
                    radius = (float)Screen.height * .5f - padding.y;
                else
                    radius = (float)Screen.width * .5f - padding.x;
            }
            else
            {
                radius = Mathf.Abs(radius);
            }
        }

        private static Vector2 GetMainGameViewSize()
        {
#if UNITY_EDITOR
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            Vector2 resolution = new Vector2(((Vector2)Res).x * 1.0f, ((Vector2)Res).y * 1.0f);
            return resolution;
#else
            return new Vector2(Screen.width, Screen.height);
#endif
        }


        private Texture disc;
        private Texture2D rect;

        void OnGUI()
        {
#if UNITY_EDITOR
            if (Application.isPlaying || !displayGizmo) return;
            Camera m = cam == null ? Camera.main : cam;
            switch (mode)
            {
                case mode_enum.DISC:
                    if (disc == null)
                        disc = Instantiate<Texture>(Resources.Load<Texture>("Disc_OSI"));
                    ComputeRadius();
                    GUI.DrawTexture(new Rect(m.pixelWidth * .5f - radius, m.pixelHeight * .5f - radius, radius * 2f, radius * 2f), disc, ScaleMode.ScaleToFit, true, 1.0f);
                    break;
                case mode_enum.SCREEN:
                    if (rect == null)
                    {
                        Color col = new Color(1f, 1f, 1f, 0.5f);
                        rect = new Texture2D(1, 1);
                        rect.SetPixel(0, 0, col);
                        rect.Apply();
                    }
                    GUI.skin.box.normal.background = rect;
                    GUI.Box(new Rect(padding.x, padding.y, m.pixelWidth - padding.x * 2f, m.pixelHeight - padding.y * 2f), GUIContent.none);
                    break;
            }
#endif
        }



    }

}