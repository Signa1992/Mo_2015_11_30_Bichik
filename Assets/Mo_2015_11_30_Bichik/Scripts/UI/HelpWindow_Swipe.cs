using UnityEngine;
using System.Collections;
namespace UI
{
    public class HelpWindow_Swipe: MonoBehaviour
    {
        [Range(0, 100)]
        public float ThresholdInPercent;
       
        private int touchCount;
        private Vector2 deltaPosition = Vector2.zero;
        private HelpWindow_Animation helpWindowAnimController;
        private float deltaTime = 0;

        private void checkSwipe(Vector2 deltaPos, float deltaTime)
        {
            Debug.Log(deltaPos);
            if (Mathf.Abs(deltaPos.x) > Screen.width * ThresholdInPercent / 100 && deltaPos.x < 0)
            {
                helpWindowAnimController.Right();
            }
            if (Mathf.Abs(deltaPos.x) > Screen.width * ThresholdInPercent / 100 && deltaPos.x > 0)
            {
                helpWindowAnimController.Left();
            }
        }
        
        // Use this for initialization
        void Start()
        {
            helpWindowAnimController = FindObjectOfType<HelpWindow_Animation>();
        }

        // Update is called once per frame
        void Update()
        {
            touchCount = Input.touchCount;
            if (touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        deltaPosition = Vector2.zero;
                        deltaTime = 0;
                        break;
                    case TouchPhase.Moved:
                        deltaPosition += touch.deltaPosition;
                        deltaTime += touch.deltaTime;
                        break;
                    case TouchPhase.Ended:
                        checkSwipe(deltaPosition, deltaTime);
                        break;
                }
            }
        }
    }
}