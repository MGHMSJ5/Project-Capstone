using UnityEngine;
using UnityEngine.SceneManagement;
// Public enum that is used to set what spawn location it is
public enum SpawnLocationsType
{
    Default,
    Factory,
    Cave
}

public class SceneSpawnManager : Singleton<SceneSpawnManager>
{
    [HideInInspector]
    public SpawnLocationsType Location = SpawnLocationsType.Default;

    private GameObject player;
    private void OnEnable()
    {
        // Subscribe to scene loaded event. This will happen when a scene has loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Function that will be called when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = null;
        // Check if the scene name is not Loading scene (will fix issue of finding the player while this scene is loaded
        if (scene.name != "LoadingScene")
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // If the player is not null and the spawnposition has been set then ↓
        if (player != null && Location != SpawnLocationsType.Default)
        {
            ChangePlayerPosition();
        }
    }
    private void ChangePlayerPosition()
    {
        // Find the gameObject in scene that has the same name as the set SpawnLocationsType Location
        GameObject spawnObject = GameObject.Find(Location.ToString());
        if (spawnObject == null)
        {
            Debug.LogError("No Spawnposition GameObject has been set for " + Location);
            return;
        }

        player.transform.localPosition = spawnObject.transform.localPosition;
        // Reset the spawnposition
        Location = SpawnLocationsType.Default;
    }
}
