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

    private AudioSource audioSource;

    private int maxBoxesIndex;

    private int currentBoxesIndex = 0;

    [SerializeField]
    private GameObject[] spawnedBoxes;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxBoxesIndex = boxPlaces.Length - 1;
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
            //Daba una excepción al inicio, imagino que algún objeto está llamando a este
            //método antes de que se ejecute el Start() de este objeto.
        }
    }

    public bool IsFull()
    {
        return currentBoxesIndex > maxBoxesIndex;
    }

    public void CleanPlatform()
    {
        for (int i = 0; i < spawnedBoxes.Length; i++)
        {
            if (spawnedBoxes[i])
            {
                Destroy(spawnedBoxes[i]);
                spawnedBoxes[i] = null;
            }
        }

        currentBoxesIndex = 0;
    }

    //Añade una caja a la plataforma
    public void PickBox()
    {
        if (belt.CurrentBox && spawnZone.CurrentBoxType == belt.CurrentBox.ThisBoxType && currentBoxesIndex <= maxBoxesIndex)
        {
            //Reproduce el sonido
            audioSource.PlayOneShot(boxSfx, boxVolume);

            //Instancia una caja del color apropiado
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

            //Coloca la caja en su lugar
            spawnedBoxes[currentBoxesIndex] = boxToSpawn;
            boxToSpawn.SetActive(true);
            boxToSpawn.transform.parent = transform;
            boxToSpawn.transform.position = boxPlaces[currentBoxesIndex].transform.position;
            currentBoxesIndex++;

            //Elimina la caja que tiene el jugador.
            Destroy(belt.CurrentBox.gameObject);
            belt.CurrentBox = null;
        }
    }
}