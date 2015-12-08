using UnityEngine;
using System.Collections;

namespace UI
{
    public class HelpWindow_Reset : MonoBehaviour
    {
        public bool IsReset = false;
        private Indicators indicators;
        private HelpWindow_Animation helpAnimation;
        // Use this for initialization
        void Start()
        {
            indicators = FindObjectOfType<Indicators>();
            helpAnimation = FindObjectOfType<HelpWindow_Animation>();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsReset)
            {
                GetComponent<Animation>().Play();
                indicators.setHelpIndicatorChecked(0);
                helpAnimation.currentHelp = 0;
                IsReset = false;
            }
        }
    }
}