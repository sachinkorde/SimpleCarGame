using UnityEngine;


    public class PinchZoomInZoomOutSticker : MonoBehaviour
    {
        float initialDistance;
        Vector3 initialScale = Vector3.one;
        public bool isClicked = false;

        void Update()
        {
            if (isClicked)
            {
                if (Input.touchCount == 2)
                {
                    var touchZero = Input.GetTouch(0);
                    var touchOne = Input.GetTouch(1);

                    if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                        touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
                    {
                        return;
                    }

                    if (touchZero.phase == TouchPhase.Began || touchZero.phase == TouchPhase.Began)
                    {
                        initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                        initialScale = transform.localScale;
                    }
                    else
                    {
                        SoundManager.instance.aud.Stop();
                        var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                        if (Mathf.Approximately(initialDistance, 0))
                        {
                            return;
                        }
                        var factor = currentDistance / initialDistance;

                        if (transform.localScale.x < 3.0f)
                        {
                            transform.localScale = initialScale * factor;
                        }
                        else if (transform.localScale.x > 3.0f)
                        {
                            transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
                        }
                    }
                }
                else
                {
                    isClicked = false;
                }
            }
        }
    }

