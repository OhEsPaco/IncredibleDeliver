using System;
using System.Collections;
using UnityEngine;

//Script de la cinta transportadora
public class ConveyorBelt : MonoBehaviour
{
    public enum BoxType
    {
        RED,
        GREEN,
        BLUE
    }

    [SerializeField]
    private float timeBetweenSpawns = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float movementSpeed = 1f;

    [SerializeField]
    private bool despawnOnEnd = true;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private Transform despawnPoint;

    [SerializeField]
    private Box blueBox;

    [SerializeField]
    private Box redBox;

    [SerializeField]
    private Box greenBox;

    [SerializeField]
    private int maxNumberOfBoxesOnBelt = 10;

    [SerializeField]
    private Transform ceiling;

    [SerializeField]
    private Transform floor;

    [SerializeField]
    private Counter counter;

    [SerializeField]
    [Range(0f, 0.5f)]
    private float randomOffsetBetweenSpawns = 0.3f;

    [SerializeField]
    [Range(0f, 17f)]
    private float randomOffsetBoxAngle = 17f;

    private float currentTimeBetweenSpawns;

    public int AvailableReds
    { get; set; }

    public int AvailableBlues
    { get; set; }

    public int AvailableGreens
    { get; set; }

    public void AddPoints(int points)
    {
        counter.AddToScore(points);
    }

    public Box CurrentBox
    { get; set; }

    private void Start()
    {
        currentTimeBetweenSpawns = timeBetweenSpawns;
        StartCoroutine(SpawnBoxes());
    }

    //Suma 9 cajas disponibles por cada plataforma de un color que aparece
    public void SpawnedPlatform(BoxType boxType)
    {
        switch (boxType)
        {
            case BoxType.RED:
                AvailableReds += 9;
                break;

            case BoxType.GREEN:
                AvailableGreens += 9;
                break;

            case BoxType.BLUE:
                AvailableBlues += 9;
                break;
        }
    }

    //Genera una caja de un color valido
    private BoxType GetRandomBoxColor()
    {
        bool validBox = false;

        BoxType randomBox;
        do
        {
            Array values = Enum.GetValues(typeof(BoxType));
            randomBox = (BoxType)values.GetValue(RandomGenerator.Instance.Random.Next(values.Length));

            switch (randomBox)
            {
                case BoxType.RED:
                    if (AvailableReds > 0)
                    {
                        validBox = true;
                    }
                    break;

                case BoxType.GREEN:
                    if (AvailableGreens > 0)
                    {
                        validBox = true;
                    }
                    break;

                case BoxType.BLUE:
                    if (AvailableBlues > 0)
                    {
                        validBox = true;
                    }
                    break;
            }
        } while (!validBox);
        return randomBox;
    }

    //Si una caja llega al final de la cinta se suma 1 a las cajas disponibles
    public void BoxDespawned(BoxType boxType)
    {
        switch (boxType)
        {
            case BoxType.RED:
                AvailableReds++;
                break;

            case BoxType.GREEN:
                AvailableGreens++;
                break;

            case BoxType.BLUE:
                AvailableBlues++;
                break;
        }
    }

    //Genera cajas
    private IEnumerator SpawnBoxes()
    {
        do
        {
            if (AvailableReds > 0 || AvailableGreens > 0 || AvailableBlues > 0)
            {
                currentTimeBetweenSpawns = timeBetweenSpawns + GetRandomFloat(0, randomOffsetBetweenSpawns);
                GameObject boxToSpawn = null;
                BoxType boxColor = GetRandomBoxColor();
                switch (boxColor)
                {
                    case BoxType.RED:
                        boxToSpawn = Instantiate(redBox.gameObject, redBox.transform);
                        AvailableReds--;
                        break;

                    case BoxType.GREEN:
                        boxToSpawn = Instantiate(greenBox.gameObject, greenBox.transform);
                        AvailableGreens--;
                        break;

                    case BoxType.BLUE:
                        boxToSpawn = Instantiate(blueBox.gameObject, blueBox.transform);
                        AvailableBlues--;
                        break;
                }

                if (boxToSpawn)
                {
                    boxToSpawn.transform.Rotate(Vector3.forward, GetRandomFloat(-randomOffsetBoxAngle, randomOffsetBoxAngle));
                    boxToSpawn.transform.position = spawnPoint.transform.position;
                    boxToSpawn.transform.parent = gameObject.transform;
                    Box boxScript = boxToSpawn.GetComponent<Box>();
                    boxScript.Ceiling = ceiling;
                    boxScript.Floor = floor;
                    boxScript.ThisBoxType = boxColor;
                    boxScript.SpawnedBy = this;
                    boxScript.MoveFrom = spawnPoint;
                    boxScript.MoveTo = despawnPoint;
                    boxScript.MovementSpeed = movementSpeed;
                    boxScript.DespawnOnEnd = despawnOnEnd;
                    boxScript.Moving = true;
                }
            }

            yield return new WaitForSeconds(currentTimeBetweenSpawns);
        } while (true);
    }

    public float GetRandomFloat(float minimum, float maximum)
    {
        return (float)RandomGenerator.Instance.Random.NextDouble() * (maximum - minimum) + minimum;
    }
}