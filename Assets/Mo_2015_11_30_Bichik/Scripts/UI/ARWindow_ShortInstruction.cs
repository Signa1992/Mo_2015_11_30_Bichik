using UnityEngine;
using System.Collections;
using Vuforia;

namespace UI
{
    public class ARWindow_ShortInstruction : MonoBehaviour
    {
        public bool isTiming = false;
        private float timeFromStart = 0;
        private Animation shortInstructionAnimation;

        public void CloseShortInstructionButtonClick()
        {
            Hide();

            foreach( var trackable in FindObjectsOfType(typeof(TrackableBehaviour)))
            {
                enabled = true;
            }
        }

        private void Show()
        {
            shortInstructionAnimation.Play("ARWindow_InstructionShow");
        }

        private void Hide()
        {
            shortInstructionAnimation.Play("ARWindow_InstructionHide");
        }

        // Use this for initialization
        void Start()
        {
            shortInstructionAnimation = GameObject.Find("ShortInstructions").GetComponent<Animation>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isTiming)
            {
                timeFromStart += Time.deltaTime;
                if (timeFromStart > 1.5f)
                {
                    isTiming = false;
                    Show();
                }
            }
            else if (timeFromStart > 0)
            {
                timeFromStart = 0;
            }
              
        }
    }
}