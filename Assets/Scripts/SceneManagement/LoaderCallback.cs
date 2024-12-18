using UnityEngine;

namespace SceneManagement
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool _isFirstUpdate = true;

        private void Update()
        {
            if (!_isFirstUpdate) return;

            _isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}