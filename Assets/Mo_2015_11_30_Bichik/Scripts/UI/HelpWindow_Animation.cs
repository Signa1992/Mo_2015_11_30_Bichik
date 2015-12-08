using UnityEngine;
using System.Collections;

namespace UI {
    public class HelpWindow_Animation : MonoBehaviour {

        private Animation helpWindowAnimation;
        private Indicators indicatorsController;
        public int currentHelp = 0;

        public void Right()
        {
            switch (currentHelp)
            {
                case 0:
                    helpWindowAnimation.Play("Help_0_1");
                    indicatorsController.setHelpIndicatorChecked(++currentHelp);
                    break;
                case 1:
                    helpWindowAnimation.Play("Help_1_2");
                    indicatorsController.setHelpIndicatorChecked(++currentHelp);
                    break;
                case 2:
                    helpWindowAnimation.Play("Help_2_3");
                    indicatorsController.setHelpIndicatorChecked(++currentHelp);
                    break;
                case 3:
                    return;
                default:
                    return;
            }
        }

        public void Left()
        {
            switch (currentHelp)
            {
                case 0:
                    return;
                case 1:
                    helpWindowAnimation.Play("Help_1_0");
                    indicatorsController.setHelpIndicatorChecked(--currentHelp);
                    break;
                case 2:
                    helpWindowAnimation.Play("Help_2_1");
                    indicatorsController.setHelpIndicatorChecked(--currentHelp);
                    break;
                case 3:
                    helpWindowAnimation.Play("Help_3_2");
                    indicatorsController.setHelpIndicatorChecked(--currentHelp);
                    break;
                default:
                    return;
            }
        }

        // Use this for initialization
        void Start() {
            helpWindowAnimation = GameObject.Find("HelpWindow").GetComponent<Animation>();
            indicatorsController = FindObjectOfType<Indicators>();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
