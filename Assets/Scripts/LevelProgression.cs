using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("The green line/wall that blocks the path to the next level")]
    public GameObject gateToDestroy;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object touching the artifact is the Player
        if (other.CompareTag("Player"))
        {
            // Destroy the physical barrier (the green line)
            if (gateToDestroy != null)
            {
                Destroy(gateToDestroy);
            }

            // Remove the artifact item from the world
            Destroy(gameObject);
        }
    }
}