using UnityEngine;
using System.Collections.Generic;

namespace UI
{
    public class AnimationLoader : MonoBehaviour
    {
        private AnimationClip loadClip(string path)
        {
            return Resources.Load<AnimationClip>(path);
        }
        
        private void loadAnimClipForGameObject(GameObject gameObject, params string[] animationsName)
        {
            foreach (var animName in animationsName)
            {
                AnimationClip animation = loadClip("UI/Animations/" + animName);
                animation.legacy = true;
                gameObject.GetComponent<Animation>().AddClip(animation, animation.name);
            }
        } 

        // Use this for initialization
        void Start()
        {
            loadAnimClipForGameObject(GameObject.Find("RootCanvas"), "HelpWindow_Show", "HelpWindow_Hide", "ARWindow_Show", "HeroWindow_Show");
            loadAnimClipForGameObject(GameObject.Find("ARWindow"), "ARWindow_MenuShow", "ARWindow_MenuHide", "ARWindow_InstructionShow", "ARWindow_InstructionHide");
            loadAnimClipForGameObject(GameObject.Find("HeroWindow"), "HeroWindow_Right", "HeroWindow_Left");
            loadAnimClipForGameObject(GameObject.Find("HelpWindow"), "Help_0_1", "Help_1_2", "Help_2_3", "Help_3_2", "Help_2_1", "Help_1_0");
        }
    }
}