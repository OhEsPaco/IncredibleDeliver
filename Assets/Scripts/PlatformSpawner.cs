using UnityEngine;
using static ConveyorBelt;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField]
    private ConveyorBelt belt;

    public void AddPoints(int points)
    {
        belt.AddPoints(points);
    }

    public void SpawnedPlatform(BoxType boxType)
    {
        belt.SpawnedPlatform(boxType);
    }
}