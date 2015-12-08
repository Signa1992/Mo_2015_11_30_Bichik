namespace Mo_2015_11_30_Bichik
{
    using UnityEngine;
    using System.Collections;

    public class ApplicationEntryPoint : MonoBehaviour
    {
        private IEnumerator Start()
        {
            var async = Application.LoadLevelAdditiveAsync("Loading");

            yield return async;

            async = Application.LoadLevelAdditiveAsync("Application");
            FindObjectOfType<Loading>().loadingOperation = async;

            yield return async;

            Destroy(GameObject.Find("LoadingSceneRoot"));
            Destroy(GameObject.Find("EntrySceneRoot"));
        }
    }
}