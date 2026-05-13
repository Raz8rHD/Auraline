using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AuralineSceneLoader
{
    // This runs as soon as Unity finishes loading the project
    static AuralineSceneLoader()
    {
        EditorApplication.delayCall += ForceOpenScene;
    }

    static void ForceOpenScene()
    {
        // Path to your scene - MAKE SURE THIS MATCHES YOUR SCENE FILENAME
        string scenePath = "Assets/Scenes/SampleScene.unity";

        // Only open it if we aren't already looking at it
        if (EditorSceneManager.GetActiveScene().path != scenePath)
        {
            Debug.Log("Auraline System: Forcing Auraline scene to load...");
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}