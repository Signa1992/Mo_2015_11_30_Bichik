using UnityEngine;
using System.Collections;
using System;

namespace UI
{
    public class HeroWindow_Swipe : MonoBehaviour
    {
        //см. Затухающие колебания с коэффициентом 1
        [Range(1, 5)]
        public float MoveSpeedCoefficient;
        public float Decceleration;
        public float w0;

        private float inertiaSpeed;
        private bool isInertia;
        private bool isMovingToNearestHero = false;
        private float x0_backing;
        private float x0_inertia;
        private float v0;
        private float deltaPos;
        private float deltaTime;
        private RectTransform heroesContainer;
        private HeroWindow_Listener heroListener;
        private float backingMovementStartingTime;
        private float inertiaStartingTime;
        private CanvasGroup canvasGroup;
        [HideInInspector]

        public bool IsMovingToNearestHero
        {
            get
            {
                return isMovingToNearestHero;
            }
            set
            {   
                if (value == true)
                    backingMovementStartingTime = Time.timeSinceLevelLoad;
                isMovingToNearestHero = value;               
            }
        }

        public bool IsInertia
        {
            get
            {
                return isInertia;
            }
            set
            {
                if (value == true)
                    inertiaStartingTime = Time.timeSinceLevelLoad;
                isInertia = value;
            }
        }


        private void startMoving()
        {
            findNearestHero();
            IsMovingToNearestHero = true;
        }

        private void findNearestHero()
        {
            float minDistanceAbs = int.MaxValue;
            for (int i = 0; i < heroListener.HeroesCount; i++)
            {
                if (Mathf.Abs(heroesContainer.anchoredPosition.x - i * -200) < minDistanceAbs)
                {
                    minDistanceAbs = Mathf.Abs(heroesContainer.anchoredPosition.x - i * -200);
                    x0_backing = heroesContainer.anchoredPosition.x - i * -200;
                    heroListener.CurrentHero = i;
                }
            }
        }

        private void checkIsMovingFinished()
        {
            if (Mathf.Abs(heroesContainer.anchoredPosition.x - heroListener.CurrentHero * -200) < 0.5)
            {
                IsMovingToNearestHero = false;
                heroesContainer.anchoredPosition = new Vector2(heroListener.CurrentHero * -200, 0);
            }
        }

        private void move(float time)
        {
            heroesContainer.anchoredPosition = new Vector2((x0_backing + (v0 + w0 * x0_backing) * (Time.timeSinceLevelLoad - time)) * Mathf.Exp(w0 * (time - Time.timeSinceLevelLoad)) + heroListener.CurrentHero * -200, 0);
        }

        private void disableInteractable()
        {
            canvasGroup.interactable = false;
        }

        private void enableInteractable()
        {
            canvasGroup.interactable = true;
        }

        // Use this for initialization
        void Start()
        {
            heroesContainer = GameObject.Find("HeroesContainer").GetComponent<RectTransform>();
            heroListener = FindObjectOfType<HeroWindow_Listener>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Update is called once per frame
        void Update()
        {
            
            if (Input.touchCount == 1)
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            deltaPos = 0;
                            deltaTime = 0;
                            IsMovingToNearestHero = false;
                            IsInertia = false;
                            enableInteractable();
                            break;
                        case TouchPhase.Moved:
                            heroesContainer.anchoredPosition = new Vector2(heroesContainer.anchoredPosition.x + touch.deltaPosition.x * MoveSpeedCoefficient, 0);
                            deltaPos += touch.deltaPosition.x;
                            deltaTime += touch.deltaTime;
                            findNearestHero();
                            if (Mathf.Abs(deltaPos) > Screen.width / 100)
                            {
                                disableInteractable();
                            }
                            break;
                        case TouchPhase.Ended:
                            if (deltaTime < 0.01)
                            {
                                v0 = 0;
                            }
                            else if (deltaTime > 1)
                            {
                                deltaTime = Mathf.Pow(deltaTime, 20);
                            }
                            else
                            {
                                v0 = deltaPos / deltaTime;
                            }
                            //v0 = touch.deltaPosition.x / touch.deltaTime;
                            //startMoving();
                            startInertia();
                            break;
                    }
                }
            }

            if (IsInertia)
            {
                inertia(inertiaStartingTime);
                findNearestHero();
                checkInertia();
            }

            if (heroesContainer.anchoredPosition.x > 0)
            {
                heroesContainer.anchoredPosition = new Vector2(0, 0);
                IsInertia = false;
                IsMovingToNearestHero = false;
            }

            if (heroesContainer.anchoredPosition.x < (heroListener.HeroesCount - 1) * -200)
            {
                heroesContainer.anchoredPosition = new Vector2((heroListener.HeroesCount - 1) * -200, 0);
                IsInertia = false;
                IsMovingToNearestHero = false;
            }

            if (IsMovingToNearestHero)
            {
                move(backingMovementStartingTime);
                checkIsMovingFinished();
            }
        }

        private void startInertia()
        {
            x0_inertia = heroesContainer.anchoredPosition.x;
            IsInertia = true;
        }

        private void checkInertia()
        {
            if (Mathf.Abs(inertiaSpeed) < 100)
            {
                IsInertia = false;
                v0 = 0;
                startMoving();
            }
        }

        private void inertia(float inertiaStartingTime)
        {
            Debug.Log("Check");
            if (v0 > 0)
            {
                heroesContainer.anchoredPosition = new Vector2(x0_inertia + v0 * (Time.timeSinceLevelLoad - inertiaStartingTime) - Decceleration * Mathf.Pow((Time.timeSinceLevelLoad - inertiaStartingTime), 2) / 2, 0);
                inertiaSpeed = v0 - Decceleration * (Time.timeSinceLevelLoad - inertiaStartingTime);
            }
            if (v0 < 0)
            {
                heroesContainer.anchoredPosition = new Vector2(x0_inertia + v0 * (Time.timeSinceLevelLoad - inertiaStartingTime) + Decceleration * Mathf.Pow((Time.timeSinceLevelLoad - inertiaStartingTime), 2) / 2, 0);
                inertiaSpeed = v0 + Decceleration * (Time.timeSinceLevelLoad - inertiaStartingTime);
            }
            if (v0 == 0)
            {
                inertiaSpeed = 0;
            }
        }
    }
}