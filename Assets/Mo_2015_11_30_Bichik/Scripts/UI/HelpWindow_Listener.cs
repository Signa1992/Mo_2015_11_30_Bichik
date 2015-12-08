using UnityEngine;
using System.Collections;

namespace UI
{
    public class HelpWindow_Listener : MonoBehaviour
    {

        private Animation rootAnimation;

        public void BackButtonClick()
        {
            rootAnimation.Play("HelpWindow_Hide");
        }

        // Use this for initialization
        void Start()
        {
            rootAnimation = GameObject.Find("RootCanvas").GetComponent<Animation>();
        }
    }
}