using UnityEngine;
using System.Collections;

namespace Mo_2015_11_30_Bichik
{
    [RequireComponent(typeof(Renderer))]
    [DisallowMultipleComponent]
    [HelpURL(General.Constants.DocumentationRoot + "index.html")]
    public class SkinnedTextureActuator : MonoBehaviour
    {
        [Header("References")]
        public SkinnedBackgroundTrackableEventHandler EventHandler;

        private new Renderer renderer;
        private Texture oldMainTexture;

        private void EventHandler_OnSkinnedBackgroundUpdated()
        {
            if( !EventHandler.IsPresentationMode && renderer != null && renderer.material != null )
            {
                renderer.material.mainTexture = EventHandler.SkinnedBackground;
            }
        }

        private void EventHandler_OnPresentationModeUpdated()
        {
            if (renderer != null && renderer.material != null)
            {
                renderer.material.mainTexture = EventHandler.IsPresentationMode?oldMainTexture:EventHandler.SkinnedBackground;
            }
        }

        private void Awake()
        {
            renderer = GetComponent("Renderer") as Renderer;
            oldMainTexture = renderer.material.mainTexture;
        }

        private void OnEnable()
        {
            if (EventHandler != null)
            {
                EventHandler.OnSkinnedBackgroundUpdated += EventHandler_OnSkinnedBackgroundUpdated;
                EventHandler.OnPresentationModeUpdated += EventHandler_OnPresentationModeUpdated;
            }
        }
        
        private void OnDisable()
        {
            if (EventHandler != null)
            {
                EventHandler.OnSkinnedBackgroundUpdated -= EventHandler_OnSkinnedBackgroundUpdated;
                EventHandler.OnPresentationModeUpdated -= EventHandler_OnPresentationModeUpdated;
            }
        }
    }
}
