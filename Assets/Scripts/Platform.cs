using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConveyorBelt;

public class Platform : MonoBehaviour
{

    [SerializeField]
    private SpawnZone spawnZone;

    [SerializeField]
    private ConveyorBelt belt;

    [SerializeField]
    private Transform[] boxPlaces;

    [SerializeField]
    private GameObject boxRed;

    [SerializeField]
    private GameObject boxBlue;

    [SerializeField]
    private GameObject boxGreen;

    [SerializeField]
    private AudioClip boxSfx;

    [SerializeField]
    private AudioClip platformSfx;

    [SerializeField]
    [Range(0f, 1f)]
    private float boxVolume;

    AudioSource audioSource;

    private int maxBoxesIndex;

    private int currentBoxesIndex = 0;

    [SerializeField]
    private GameObject[] spawnedBoxes;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    maxBoxesIndex = boxPlaces.Length-1;
        spawnedBoxes = new GameObject[boxPlaces.Length];
    }

    public void PlayPlatformSound()
    {
        try
        {
            if (platformSfx != null)
            {
                audioSource.PlayOneShot(platformSfx, boxVolume);
            }
        }
        catch
        {

        }
       
     
    }

    public bool IsFull()
    {
        return currentBoxesIndex > maxBoxesIndex;
    }
    public void CleanPlatform()
    {
        for(int i = 0; i < spawnedBoxes.Length; i++)
        {
            if (spawnedBoxes[i])
            {
                Destroy(spawnedBoxes[i]);
                spawnedBoxes[i] = null;
            }
        }


        currentBoxesIndex = 0;
    }
    public void PickBox()
    {
     
        if(belt.CurrentBox && spawnZone.CurrentBoxType==belt.CurrentBox.ThisBoxType && currentBoxesIndex<=maxBoxesIndex)
        {
            audioSource.PlayOneShot(boxSfx, boxVolume);
            GameObject boxToSpawn = null;
          
            switch (belt.CurrentBox.ThisBoxType)
            {
                case BoxType.RED:
                    boxToSpawn = Instantiate(boxRed, boxRed.transform);
                   
                    break;

                case BoxType.GREEN:
                    boxToSpawn = Instantiate(boxGreen, boxGreen.transform);
                    
                    break;

                case BoxType.BLUE:
                    boxToSpawn = Instantiate(boxBlue, boxBlue.transform);
                    
                    break;
            }
            spawnedBoxes[currentBoxesIndex] = boxToSpawn;
            boxToSpawn.SetActive(true);
            boxToSpawn.transform.parent = transform;
            boxToSpawn.transform.position = boxPlaces[currentBoxesIndex].transform.position;
            currentBoxesIndex++;

            Destroy(belt.CurrentBox.gameObject);
            belt.CurrentBox = null;
        }
    }

}
