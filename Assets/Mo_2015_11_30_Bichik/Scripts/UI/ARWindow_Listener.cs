using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;

namespace UI
{
    public class ARWindow_Listener : MonoBehaviour
    {
        private Animation rootCanvasAnimation;
        private Animation arWindowAnimation;
        private bool isMenuShow = false;
        private Toggle menuToggle;

        public void BackButtonClick()
        {
            rootCanvasAnimation.Play("HeroWindow_Show");
            if (isMenuShow)
            {
                arWindowAnimation.Play("ARWindow_MenuHide");
                isMenuShow = false;
                menuToggle.isOn = false;
            }
        }

        public void HelpButtonClick()
        {
            rootCanvasAnimation.Play("HelpWindow_Show");
        }

        private bool isShareProcessing = false;

        public IEnumerator ShareScreenshot()
        {
            if (!isShareProcessing)
            {
                isShareProcessing = true;

                yield return new WaitForEndOfFrame();

                Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
                screenTexture.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
                screenTexture.Apply();

                byte[] dataToSave = screenTexture.EncodeToPNG();

                string destination = Path.Combine(Application.persistentDataPath, DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");

                File.WriteAllBytes(destination, dataToSave);

                if (!Application.isEditor)
                {
                    // block to open the file and share it ------------START
                    AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                    AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
                    intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
                    AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                    AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
                    intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
                    //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "testo");
                    //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
                    intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
                    AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

                    // option one:
                    currentActivity.Call("startActivity", intentObject);
                }

                isShareProcessing = false;
            }
        }

        public void PhotoButtonClick()
        {
            StartCoroutine("ShareScreenshot");
        }

        public void ShotButtonClick()
        {
            Mo_2015_11_30_Bichik.SkinnedBackgroundTrackableEventHandler.CleanAllScans();
        }

        private bool allPresentationMode = false;

        public void DrawindButtonClick()
        {
            allPresentationMode = !allPresentationMode;
            Mo_2015_11_30_Bichik.SkinnedBackgroundTrackableEventHandler.SwitchAllPresentationMode(allPresentationMode);
        }

        public void MenuToggleClick(GameObject sender)
        {
            bool isOn = sender.GetComponent<Toggle>().isOn;
            if (isOn)
            {
                arWindowAnimation.Play("ARWindow_MenuShow");
                isMenuShow = true;
            }
            else
            {
                arWindowAnimation.Play("ARWindow_MenuHide");
                isMenuShow = false;
            }
        }
        
        public void SoundToggleClick(GameObject sender)
        {
            bool isOn = sender.GetComponent<Toggle>().isOn;
            if (isOn)
            {
                AudioListener.volume = 1f;
            }
            else
            {
                AudioListener.volume = 0f;
            }
        }
        // Use this for initialization
        void Start()
        {
            rootCanvasAnimation = GameObject.Find("RootCanvas").GetComponent<Animation>();
            arWindowAnimation = GameObject.Find("ARWindow").GetComponent<Animation>();
            menuToggle = GameObject.Find("MenuToggle").GetComponent<Toggle>();
        }
    }
}