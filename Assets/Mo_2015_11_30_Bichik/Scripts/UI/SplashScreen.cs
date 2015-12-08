using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UI
{
    public class SplashScreen : MonoBehaviour
    {
        private RectTransform indicatorRectTransform;

        private int Progress
        {
            set
            {
                value = Mathf.Clamp(value, 0, 100);
                indicatorRectTransform.offsetMax = new Vector2((100 - value) * -620 / 100, 0);
            }
        }

        private IEnumerator loadLevel()
        {
            AsyncOperation asyncOper = Application.LoadLevelAsync("__Begin");
            while (!asyncOper.isDone)
            {
                Progress = (int)(asyncOper.progress * 100);
                yield return null;
            }
        }

        // Use this for initialization
        void Start()
        {
            indicatorRectTransform = GameObject.Find("Indicator").GetComponent<RectTransform>();
            StartCoroutine("loadLevel");
        }
    }
}