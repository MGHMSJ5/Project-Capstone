using UnityEngine;

public class WallVisibilityManager : MonoBehaviour
{
    [System.Serializable]
    public class Wall
    {
        public Renderer renderer;

        // Direction pointing INTO the playable/interior side.
        public Vector3 insideNormal = Vector3.forward;

        [HideInInspector]
        public bool lastVisible = true;
    }

    [SerializeField] private Wall[] walls;

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;

        foreach (Wall wall in walls)
        {
            if (wall.renderer == null)
                continue;

            Vector3 toCamera =
                cameraPosition - wall.renderer.transform.position;

            bool visible =
                Vector3.Dot(
                    toCamera.normalized,
                    wall.insideNormal.normalized
                ) > 0f;

            // Only change the renderer when its state actually changes.
            if (visible != wall.lastVisible)
            {
                wall.renderer.enabled = visible;
                wall.lastVisible = visible;
            }
        }
    }
}