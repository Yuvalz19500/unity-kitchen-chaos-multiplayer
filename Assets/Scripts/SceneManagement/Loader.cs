using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public static class Loader
    {
        public enum Scenes
        {
            MainMenuScene,
            GameScene,
            LoadingScene,
            LobbyScene,
            CharacterSelectScene
        }

        private static Scenes _targetScene;

        public static void Load(Scenes targetScene)
        {
            _targetScene = targetScene;

            SceneManager.LoadScene(Scenes.LoadingScene.ToString());
        }

        public static void LoaderCallback()
        {
            SceneManager.LoadScene(_targetScene.ToString());
        }

        public static void LoadNetworked(Scenes targetScene)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
        }
    }
}