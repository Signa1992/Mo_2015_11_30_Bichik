using System;
using UnityEngine;
using Vuforia;
using System.Collections;

namespace Mo_2015_11_30_Bichik
{
    [RequireComponent(typeof(TrackableBehaviour))]
    [DisallowMultipleComponent]
    [HelpURL(General.Constants.DocumentationRoot)]
    public class SkinnedBackgroundTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
    {
//Public

        /// <summary>
        /// Reference to background renderer (NOT NULL)
        /// </summary>
        /// <remarks>
        /// Usualy just a reference to ARCamera/Camera/BackgroundPlane mesh renderer
        /// </remarks>
        [Header("References")]
        public Renderer BackgroundRenderer;

        /// <summary>
        /// Texture(RGB), that's contain rectified to original size image of marker in real world, gathered from background video texture of <see cref="BackgroundRenderer"/>
        /// </summary>
        public Texture2D SkinnedBackground { get; private set; }  
        
        /// <summary>
        /// Event, called when <see cref="SkinnedBackground"/> successful gathered and recttified
        /// </summary>
        public event Action OnSkinnedBackgroundUpdated;

//Private

        private TrackableBehaviour trackableBehaviour;


//Messages 

        private void Awake()
        {
            trackableBehaviour = GetComponent("TrackableBehaviour") as TrackableBehaviour;
        }

        private void Start()
        {
            if(trackableBehaviour)
            {
                trackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }


// Interfaces realization

        public void OnTrackableStateChanged( TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            var isFound = 
                newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED;

            foreach (Renderer component in rendererComponents)
            {
                component.enabled = isFound;
            }

            foreach (Collider component in colliderComponents)
            {
                component.enabled = isFound;
            }

            Debug.Log("Trackable " + trackableBehaviour.TrackableName + (isFound ? " found" : " lost"));
        }

// Methods

        private IEnumerator UpdateSkinnedBackgroundCoroutine()
        {
            if (trackableBehaviour != null && BackgroundRenderer != null)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Start process of gathering and rectifing of <see cref="SkinnedBackground"/>
        /// </summary>
        /// <remarks>
        /// when processing ended <see cref="OnSkinnedBackgroundUpdated"/> will be called
        /// </remarks>
        public void UpdateSkinnedBackground()
        {
            StopCoroutine("UpdateSkinnedBackgroundCoroutine");
            StartCoroutine("UpdateSkinnedBackgroundCoroutine");
        }
    }
}