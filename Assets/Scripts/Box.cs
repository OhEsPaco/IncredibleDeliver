using System.Collections;
using UnityEngine;
using static ConveyorBelt;

public class Box : MonoBehaviour
{
    public float InterpolationValue
    { get; set; }

    public Transform MoveFrom
    { get; set; }

    public Transform MoveTo
    { get; set; }

    public bool Moving
    { get; set; }

    public float MovementSpeed
    { get; set; }

    public bool DespawnOnEnd
    { get; set; }

    public BoxType ThisBoxType
    { get; set; }

    public ConveyorBelt SpawnedBy
    { get; set; }

    public Transform Ceiling
    { get; set; }

    public Transform Floor
    { get; set; }

    public bool MovingBySight
    { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        MovingBySight = false;
        InterpolationValue = 0f;
    }

    public void MoveBox()
    {
        if (SpawnedBy && !SpawnedBy.CurrentBox)
        {
            SpawnedBy.CurrentBox = this;
            Moving = false;
            MovingBySight = true;

            StartCoroutine(MoveAndCheck());
        }
        else if (SpawnedBy)
        {
            if (SpawnedBy.CurrentBox)
            {
                SpawnedBy.CurrentBox.MovingBySight = false;
                SpawnedBy.CurrentBox.InterpolationValue = this.InterpolationValue;
                SpawnedBy.CurrentBox.transform.position = this.transform.position;
                SpawnedBy.CurrentBox.transform.rotation = this.transform.rotation;
                SpawnedBy.CurrentBox.Moving = true;
                SpawnedBy.CurrentBox.EnableCollider(1f);
                SpawnedBy.CurrentBox = this;
            }

            Moving = false;
            MovingBySight = true;

            StartCoroutine(MoveAndCheck());
        }

        //SpawnedBy.BoxDespawned(ThisBoxType);
        //Destroy(this.gameObject);
    }

    public void EnableCollider(float delaySeconds)
    {
        StartCoroutine(EnableColliderCoroutine(delaySeconds));
    }

    private IEnumerator EnableColliderCoroutine(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        GetComponent<BoxCollider>().enabled = true;
    }

    private IEnumerator MoveAndCheck()
    {
        GetComponent<BoxCollider>().enabled = false;
        float distance = 3;
        do
        {
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward) * distance;
            transform.position = Camera.main.transform.position + forward;

            if (transform.position.y > Ceiling.position.y)
            {
                transform.position = new Vector3(transform.position.x, Ceiling.position.y, transform.position.z);
            }

            if (transform.position.y < Floor.position.y)
            {
                transform.position = new Vector3(transform.position.x, Floor.position.y, transform.position.z);
            }

            yield return null;
        } while (MovingBySight);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Moving && MoveTo && MoveFrom)
        {
            InterpolationValue += MovementSpeed * Time.deltaTime;

            if (InterpolationValue >= 1)
            {
                transform.position = new Vector3(
                    Mathf.Lerp(MoveFrom.position.x, MoveTo.position.x, 1),
                    Mathf.Lerp(MoveFrom.position.y, MoveTo.position.y, 1),
                    Mathf.Lerp(MoveFrom.position.z, MoveTo.position.z, 1));

                Moving = false;
                InterpolationValue = 1;

                if (DespawnOnEnd)
                {
                    SpawnedBy.BoxDespawned(ThisBoxType);
                    Destroy(gameObject);
                }
            }
            else
            {
                transform.position = new Vector3(
                 Mathf.Lerp(MoveFrom.position.x, MoveTo.position.x, InterpolationValue),
                 Mathf.Lerp(MoveFrom.position.y, MoveTo.position.y, InterpolationValue),
                 Mathf.Lerp(MoveFrom.position.z, MoveTo.position.z, InterpolationValue));
            }
        }
    }
}