using UnityEngine;

public class LevelGate : MonoBehaviour
{
    public void Open()
    {
        Destroy(gameObject);
    }
}