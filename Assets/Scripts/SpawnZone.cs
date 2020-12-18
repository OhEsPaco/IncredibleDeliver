using System;
using System.Collections;
using UnityEngine;
using static ConveyorBelt;

public class SpawnZone : MonoBehaviour
{
    [SerializeField]
    private Material red;

    [SerializeField]
    private Material blue;

    [SerializeField]
    private Material green;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private Transform stopPoint;

    [SerializeField]
    private Transform despawnPoint;

    [SerializeField]
    private Platform platform;

    [SerializeField]
    [Range(0f, 1f)]
    private float movementSpeed = 1f;

    [SerializeField]
    private Counter counter;

    [SerializeField]
    [Range(1f, 20f)]
    private float secondsBetweenPoints = 15f;

    [SerializeField]
    private PlatformSpawner platformSpawner;

    public BoxType CurrentBoxType
    { get; set; }

    private int currentPoints = 9;
    private bool isMoving = false;

    [SerializeField]
    [Range(0f, 1f)]
    private float lessMoneyVolume;

    [SerializeField]
    private AudioClip lessMoney;

    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlatformFullMovement());
    }

    private BoxType GetRandomBoxColor()
    {
        BoxType randomBox;

        Array values = Enum.GetValues(typeof(BoxType));
        randomBox = (BoxType)values.GetValue(RandomGenerator.Instance.Random.Next(values.Length));

        return randomBox;
    }

    private IEnumerator CalculatePoints()
    {
        do
        {
            yield return new WaitForSeconds(secondsBetweenPoints);
            int currentPointsCopy = currentPoints;
            currentPoints--;

            if (currentPoints < 1)
            {
                currentPoints = 1;
            }

            counter.SetPoints(currentPoints);

            if (currentPoints < currentPointsCopy)
            {
                if (lessMoney)
                {
                    audioSource.PlayOneShot(lessMoney, lessMoneyVolume);
                }
            }
        } while (true);
    }

    private void ChangePlatformColor(BoxType randomBox)
    {
        switch (randomBox)
        {
            case BoxType.RED:
                platform.gameObject.GetComponent<Renderer>().material = red;

                counter.SetMaterial(red);
                break;

            case BoxType.GREEN:
                platform.gameObject.GetComponent<Renderer>().material = green;
                counter.SetMaterial(green);
                break;

            case BoxType.BLUE:
                platform.gameObject.GetComponent<Renderer>().material = blue;
                counter.SetMaterial(blue);
                break;
        }
    }

    private IEnumerator PlatformFullMovement()
    {
        do
        {
            platform.CleanPlatform();
            currentPoints = 9;
            BoxType typeOfThisPlatform = GetRandomBoxColor();
            CurrentBoxType = typeOfThisPlatform;
            platformSpawner.SpawnedPlatform(CurrentBoxType);
            ChangePlatformColor(typeOfThisPlatform);

            isMoving = true;
            counter.SetPoints(currentPoints);

            platform.PlayPlatformSound();
            StartCoroutine(MoveObject(spawnPoint, stopPoint, platform.transform, movementSpeed));

            do
            {
                yield return null;
            } while (isMoving);

            Coroutine cPoints = StartCoroutine(CalculatePoints());

            do
            {
                yield return null;
            } while (!platform.IsFull());
            StopCoroutine(cPoints);
            platformSpawner.AddPoints(currentPoints);
            isMoving = true;
            platform.PlayPlatformSound();
            StartCoroutine(MoveObject(stopPoint, despawnPoint, platform.transform, movementSpeed));

            do
            {
                yield return null;
            } while (isMoving);
        } while (true);
    }

    private IEnumerator MoveObject(Transform from, Transform To, Transform what, float speed)
    {
        isMoving = true;
        what.position = new Vector3(from.position.x, from.position.y, from.position.z);

        for (float interpolationValue = 0; interpolationValue <= 1; interpolationValue += speed * Time.deltaTime)
        {
            what.position = new Vector3(
            Mathf.Lerp(from.position.x, To.position.x, interpolationValue),
            Mathf.Lerp(from.position.y, To.position.y, interpolationValue),
            Mathf.Lerp(from.position.z, To.position.z, interpolationValue));
            yield return null;
        }

        what.position = new Vector3(To.position.x, To.position.y, To.position.z);

        isMoving = false;
    }
}