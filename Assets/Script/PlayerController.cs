using UnityEngine;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public int laneNumber;
    private int tapCount;
    public Transform leftLimit, rightLimit, midLimit;
    private Transform target;
    private Vector3 startTouchPosition, endTouchPosition;
    [SerializeField] private float speed = 0.35f;
    [SerializeField] private float leftRot = -10.0f;
    [SerializeField] private float RightRot = 10.0f;
    [SerializeField] private float timeToRot = 0.5f;
    [SerializeField] private float timeToRotForReset = 0.1f;
    [SerializeField] private float swipeThreshold = 10.0f;

    public float timescalePlayer;
    private Touch touch;
    private bool isLaneChanged = false;

    private void Awake()
    {
        instance = this;
        
        int screenWidth = Screen.width;

        // Calculate new positions
        float newXLeft = 20;  // Offset from the left side
        float newXRight = screenWidth - 20;  // Offset from the right side

        // Keep the original Y and Z positions
        float newY = transform.position.y;
        float newZ = transform.position.z;
        leftLimit.transform.position = new Vector3(-2.5f, newY, newZ);
        rightLimit.transform.position = new Vector3(2.5f, newY, newZ);
    }

    void Start()
    {

        Application.targetFrameRate = 60;
        LoadPlayerController();
    }

    public void LoadPlayerController()
    {
        laneNumber = 1;
        transform.position = midLimit.position;
    }

    public void RotateSlightLeft()
    {
        transform.DOKill();
        transform.DORotate(new Vector3(0.0f, 0.0f, leftRot), timeToRot).SetEase(Ease.InQuad).OnComplete(ResetRotate);
    }

    public void RotateSlightRight()
    {
        transform.DOKill();
        transform.DORotate(new Vector3(0.0f, 0.0f, RightRot), timeToRot).SetEase(Ease.InQuad).OnComplete(ResetRotate);
    }

    public void ResetRotate()
    {
        transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), timeToRotForReset);
    }

    void Update()
    {
        tapCount = Input.touchCount;

        for (int i = 0; i < tapCount; i++)
        {
            touch = Input.GetTouch(i);

            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(i).position;
                isLaneChanged = false;
            }

            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                endTouchPosition = Input.GetTouch(i).position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                endTouchPosition = Input.GetTouch(i).position;

                float swipeDistance = Mathf.Abs(startTouchPosition.x - endTouchPosition.x);

                if (swipeDistance > swipeThreshold)
                {
                    switch (laneNumber)
                    {
                        case 0: //left lane

                            if (!isLaneChanged)
                            {
                                if ((endTouchPosition.x > startTouchPosition.x) && transform.position == leftLimit.position)
                                {
                                    target = midLimit;
                                    RotateSlightLeft();
                                    MovingAnimation();
                                    laneNumber = 1;
                                    isLaneChanged = true;
                                }
                            }
                            break;

                        case 1: //midLane
                            if (!isLaneChanged)
                            {
                                if ((endTouchPosition.x > startTouchPosition.x) && transform.position == midLimit.position)
                                {
                                    target = rightLimit;
                                    RotateSlightLeft();
                                    MovingAnimation();
                                    laneNumber = 2;
                                    isLaneChanged = true;
                                }
                            }

                            if (!isLaneChanged)
                            {
                                if ((endTouchPosition.x < startTouchPosition.x) && transform.position == midLimit.position)
                                {
                                    target = leftLimit;
                                    RotateSlightRight();
                                    MovingAnimation();
                                    laneNumber = 0;
                                    isLaneChanged = true;
                                }
                            }
                            break;

                        case 2: //rightLane

                            if (!isLaneChanged)
                            {
                                if ((endTouchPosition.x < startTouchPosition.x) && transform.position == rightLimit.position)
                                {
                                    target = midLimit;
                                    RotateSlightRight();
                                    MovingAnimation();
                                    laneNumber = 1;
                                    isLaneChanged = true;
                                }
                            }
                            break;
                    }
                }
            }
        }
    }

    public void MovingAnimation()
    {
        transform.DOKill();
        transform.DOLocalMove(target.position, speed).SetEase(Ease.InQuad);
    }
}
