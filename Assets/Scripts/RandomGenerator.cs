using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{

    public System.Random Random
    { get; set; }


    private void Awake()
    {
        Random = new System.Random();
    }
    private static RandomGenerator randomGenerator;

    
    public static RandomGenerator Instance
    {
        get
        {
            if (!randomGenerator)
            {
                randomGenerator = FindObjectOfType(typeof(RandomGenerator)) as RandomGenerator;

                if (!randomGenerator)
                {
                    Debug.LogError("There needs to be one active RandomGenerator script on a GameObject in your scene.");
                }
            }

            return randomGenerator;
        }
    }

}
