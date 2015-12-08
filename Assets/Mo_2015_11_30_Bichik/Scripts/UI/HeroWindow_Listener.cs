using UnityEngine;
using System.Collections;

namespace UI
{
    public class HeroWindow_Listener : MonoBehaviour
    {
        public GameObject[] Targets;

        [HideInInspector]
        public int HeroesCount;

        private HeroWindow_Swipe swipe;
        private Indicators indicatorsController;
        private Animation rootCanvasAnimation;
        private int currentHero = 0;
        public int CurrentHero
        {
            get
            {
                return currentHero;
            }
            set
            {
                GameObject.Find("Hero_" + currentHero).GetComponent<Animator>().SetBool("isCenter", false);
                GameObject.Find("Hero_" + value).GetComponent<Animator>().SetBool("isCenter", true);
                indicatorsController.setHeroIndicatorChecked(value);
                currentHero = value;
            }
        }

        public void ButtonClick(GameObject sender)
        {
            int numberOfSender = int.Parse(sender.name.Split(new char[] { '_' })[1]);

            if (swipe.IsMovingToNearestHero)
            {
                return;
            }

            if (numberOfSender == CurrentHero)
            {
                foreach(var target in Targets)
                {
                    target.SetActive(false);
                }

                Targets[CurrentHero].SetActive(true);

                rootCanvasAnimation.Play("ARWindow_Show");
            }
        }
        // Use this for initialization
        void Start()
        {
            foreach (var target in Targets)
            {
                target.SetActive(false);
            }

            indicatorsController = FindObjectOfType<Indicators>();
            swipe = FindObjectOfType<HeroWindow_Swipe>();
            GameObject.Find("Hero_0").GetComponent<Animator>().SetBool("isCenter", true);
            rootCanvasAnimation = GameObject.Find("RootCanvas").GetComponent<Animation>();
            HeroesCount = GameObject.Find("HeroesContainer").transform.childCount;
        }
    }
}