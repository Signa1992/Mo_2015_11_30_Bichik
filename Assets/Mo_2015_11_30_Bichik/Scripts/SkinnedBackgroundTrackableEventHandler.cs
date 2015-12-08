using System;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Collections;
using System.Runtime.InteropServices;

namespace Mo_2015_11_30_Bichik
{
    /// <summary>
    /// This trackable behavior allow to skin changes on trackable surface and use it as a model texture.  
    /// </summary>
    [RequireComponent(typeof(ImageTargetBehaviour))]
    [DisallowMultipleComponent]
    [HelpURL(General.Constants.DocumentationRoot + "class_mo__2015__11__30___bichik_1_1_skinned_background_trackable_event_handler.html")]
    public class SkinnedBackgroundTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
    {
        [Header("[General]")]
        public Vector4 Flags;
        public Rect MarkerAnchor;
        public int TextureSize = 512;

        [Space]
        public Color GoodColor = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        public Color BadColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);

        [Header("[References]")]
        public MeshRenderer ControlPlane;
        public Button ScanButton;
        public Button DrawingButton;

        /// <summary>
        /// Texture(RGB), that's contain rectified to original size image of marker in real world, gathered from background video texture of <see cref="BackgroundRenderer"/>
        /// </summary>
        public Texture SkinnedBackground
        {
            get;
            private set;
        }

        /// <summary>
        /// Is current trackable tracked?
        /// </summary>
        public bool IsTracked
        {
            get;
            private set;
        }

        public bool IsScanned
        {
            get;
            private set;
        }

        private bool _isPresentationMode = false;

        public bool IsPresentationMode
        {
            get { return _isPresentationMode; }
            set
            {
                _isPresentationMode = value;

                if(OnPresentationModeUpdated != null)
                {
                    OnPresentationModeUpdated();
                }
            }
        }

        /// <summary>
        /// Event, called when <see cref="SkinnedBackground"/> successful gathered and rectified
        /// </summary>
        public event Action OnSkinnedBackgroundUpdated;

        public event Action OnPresentationModeUpdated;
//Private

        private ImageTargetBehaviour markerBehaviour;

        private static Material SkinningMaterial
        {
            get;
            set;
        }

//Messages 

        private void Awake()
        {
            IsTracked = false;
            IsScanned = false;
            markerBehaviour = GetComponent(typeof(ImageTargetBehaviour)) as ImageTargetBehaviour;

            if (SkinningMaterial == null)
            {
                SkinningMaterial = new Material(Shader.Find("Hidden/MapInversionTransform"));
            }

            ControlPlane.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            if (markerBehaviour != null)
            {
                markerBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        private void OnDisable()
        {
            if (markerBehaviour != null)
            {
                markerBehaviour.UnregisterTrackableEventHandler(this);
            }
        }

// Interfaces realization

        private void ShowModel()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            AudioSource[] audioComponents = GetComponentsInChildren<AudioSource>(true);

            foreach (Renderer component in rendererComponents)
            {
                component.enabled = IsTracked && IsScanned;
            }

            foreach (Collider component in colliderComponents)
            {
                component.enabled = IsTracked && IsScanned;
            }

            foreach (AudioSource component in audioComponents)
            {
                BackgroundMusic.Volume = IsTracked && IsScanned ? 0f : 1f;
                component.enabled = IsTracked && IsScanned;
            }

            ControlPlane.gameObject.SetActive(IsTracked && !IsScanned);
            ScanButton.gameObject.SetActive(IsTracked);
            DrawingButton.gameObject.SetActive(IsTracked);
        }

        public void OnTrackableStateChanged( TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {           
            IsTracked = 
                newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED;
            
            ShowModel();

            Debug.Log("Trackable " + markerBehaviour.TrackableName + (IsTracked ? " found" : " lost"));
        }

        internal static void SwitchAllPresentationMode(bool enable)
        {
            foreach (var handler in FindObjectsOfType<SkinnedBackgroundTrackableEventHandler>())
            {
                handler.IsPresentationMode = enable;
            }
        }

        // Methods

        public bool IsWholeMarkerInFrame
        {
            get
            {
                var result = true;

                var AugmentedCamera = VideoBackgroundRenderer2.Instance.GetComponent(typeof(Camera)) as Camera;

                var targetRight = transform.right;
                var targetUp = transform.forward;
                var markerSize = markerBehaviour.GetSize();
                var targetSize = new Vector3(markerSize.x / MarkerAnchor.width, 1f, markerSize.y / MarkerAnchor.height);
                var targetPosition = transform.position - (markerSize.x * (MarkerAnchor.width + 2 * MarkerAnchor.x - 1) / (2 * MarkerAnchor.width) * targetRight) + (markerSize.y * (MarkerAnchor.height + 2 * MarkerAnchor.y - 1) / (2 * MarkerAnchor.height) * targetUp);


                var M = Matrix4x4.TRS(targetPosition, transform.rotation, targetSize);
                var V = AugmentedCamera.worldToCameraMatrix;
                var P = GL.GetGPUProjectionMatrix(AugmentedCamera.projectionMatrix, true);
                var targetMVP = P * V * M;

                var point = targetMVP.MultiplyPoint(new Vector3(-0.5f, 0.0f, -0.5f));
                result = result && point.x >= -1f && point.x <= 1f && point.y >= -1f && point.y <= 1f;

                point = targetMVP.MultiplyPoint(new Vector3(0.5f, 0.0f, -0.5f));
                result = result && point.x >= -1f && point.x <= 1f && point.y >= -1f && point.y <= 1f;

                point = targetMVP.MultiplyPoint(new Vector3(-0.5f, 0.0f, 0.5f));
                result = result && point.x >= -1f && point.x <= 1f && point.y >= -1f && point.y <= 1f;

                point = targetMVP.MultiplyPoint(new Vector3(0.5f, 0.0f, 0.5f));
                result = result && point.x >= -1f && point.x <= 1f && point.y >= -1f && point.y <= 1f;

                return result;
            }
        }

        private bool _updateRegionInProgress = false;

        private IEnumerator UpdateRegionCoroutine()
        {
            _updateRegionInProgress = true;

            yield return new WaitForSeconds(3f);

            if (IsTracked && IsWholeMarkerInFrame)
            {
                var AugmentedCamera = VideoBackgroundRenderer2.Instance.GetComponent(typeof(Camera)) as Camera;

                var targetRight = transform.right;
                var targetUp = transform.forward;
                var markerSize = markerBehaviour.GetSize();
                var targetSize = new Vector3(markerSize.x / MarkerAnchor.width, 1f, markerSize.y / MarkerAnchor.height);
                var targetPosition = transform.position - (markerSize.x * (MarkerAnchor.width + 2 * MarkerAnchor.x - 1) / (2 * MarkerAnchor.width) * targetRight) + (markerSize.y * (MarkerAnchor.height + 2 * MarkerAnchor.y - 1) / (2 * MarkerAnchor.height) * targetUp);

                
                var M = Matrix4x4.TRS(targetPosition, transform.rotation, targetSize);
                var V = AugmentedCamera.worldToCameraMatrix;
                var P = GL.GetGPUProjectionMatrix(AugmentedCamera.projectionMatrix, true);
                var targetMVP = P * V * M;

                SkinningMaterial.SetMatrix("_TargetMVP", targetMVP);

                var targetAspect = MarkerAnchor.height / MarkerAnchor.width;
                var renderTarget = SkinnedBackground == null ? new RenderTexture(TextureSize, Mathf.FloorToInt(TextureSize * targetAspect), 24, RenderTextureFormat.ARGB32) : SkinnedBackground as RenderTexture;

                SkinningMaterial.SetVector("_Flags", Flags);
                VideoBackgroundRenderer2.RequestBackgroundTexture(bacgroundTexture =>
                {
                    Graphics.Blit(bacgroundTexture, renderTarget, SkinningMaterial);
                    SkinnedBackground = renderTarget;

                    IsScanned = true;
                    ShowModel();

                    if (OnSkinnedBackgroundUpdated != null)
                    {
                        OnSkinnedBackgroundUpdated();
                    }
                });
            }

            _updateRegionInProgress = false;

            yield return null;
        }

        /// <summary>
        /// Start process of gathering and rectifing of <see cref="SkinnedBackground"/>
        /// </summary>
        /// <remarks>
        /// when processing ended <see cref="OnSkinnedBackgroundUpdated"/> will be called
        /// </remarks>
        public void UpdateSkinnedBackground()
        {
            StopCoroutine("UpdateRegionCoroutine");
            StartCoroutine("UpdateRegionCoroutine");
        }

        public static void CleanAllScans()
        {
            foreach( var handler in FindObjectsOfType<SkinnedBackgroundTrackableEventHandler>())
            {
                handler.IsScanned = false;
                handler.ControlPlane.gameObject.SetActive(true);
            }
        }

        private void LateUpdate()
        {
            if (IsTracked)
            {
                var targetRight = transform.right;
                var targetUp = transform.forward;
                var markerSize = markerBehaviour.GetSize();
                var targetSize = new Vector3(markerSize.x / MarkerAnchor.width, markerSize.y / MarkerAnchor.height, 1f);
                var targetPosition = transform.position - (markerSize.x * (MarkerAnchor.width + 2 * MarkerAnchor.x - 1) / (2 * MarkerAnchor.width) * targetRight) + (markerSize.y * (MarkerAnchor.height + 2 * MarkerAnchor.y - 1) / (2 * MarkerAnchor.height) * targetUp);

                ControlPlane.transform.position = targetPosition;
                ControlPlane.transform.rotation = Quaternion.Euler( transform.rotation.eulerAngles + new Vector3(90f, 0f, 0f));
                ControlPlane.transform.localScale = targetSize;

                DrawingButton.interactable = IsScanned;
                                
                if (IsWholeMarkerInFrame)
                {
                    ControlPlane.material.color = GoodColor;

                    if (!IsScanned && !_updateRegionInProgress)
                    {
                        UpdateSkinnedBackground();
                    }                  
                }
                else
                {
                    ControlPlane.material.color = BadColor;
                }
            }
        }
    }
}