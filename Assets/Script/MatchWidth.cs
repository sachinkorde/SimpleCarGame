using UnityEngine;
using TMPro;

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class MatchWidth : MonoBehaviour
    {
        // Set this to the in-world distance between the left & right edges of your scene.
        public float sceneWidth = 7.68f;
        float offSetiPad = 7.68f;
        float offSetiPhone = 5.6f;
        Camera _camera;
        //public TMP_Text cameraSize;

        void Start()
        {
            offSetiPad = 7.68f;
            offSetiPhone = 5.95f;
            _camera = GetComponent<Camera>();

            var identifier = SystemInfo.deviceModel;
            if (identifier.StartsWith("iPad"))
            {
                sceneWidth = offSetiPad;
                ///Debug.Log(identifier);
            }
            else
            {
                sceneWidth = offSetiPhone;
                //Debug.Log(identifier);
            }
        }

        // Adjust the camera's height so the desired scene width fits in view
        // even if the screen/window size changes dynamically.
        void Update()
        {
            float unitsPerPixel = sceneWidth / Screen.width;

            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            _camera.orthographicSize = desiredHalfHeight;
            //cameraSize.text = "Camera Size- " + _camera.orthographicSize.ToString();
        }
    }

