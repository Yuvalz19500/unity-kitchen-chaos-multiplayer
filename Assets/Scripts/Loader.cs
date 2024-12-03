using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scenes
    {
        MainMenuScene,
        GameScene,
        LoadingScene
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
}