using UnityEngine;

//Mueve al jugador dependiendo de donde mire
public class LookWalk : MonoBehaviour
{
    [SerializeField]
    [Range(0, 360)]
    private float minAngleToWalk = 0f;

    [SerializeField]
    [Range(0, 360)]
    private float maxAngleToWalk = 0f;

    [SerializeField]
    [Range(0, 1000)]
    private float speed = 5f;

    [SerializeField]
    private float maxX = 1000f;

    [SerializeField]
    private float minX = -1000f;

    [SerializeField]
    private float maxZ = 1000f;

    [SerializeField]
    private float minZ = -1000f;

    private void Update()
    {
        float cameraAngle = Camera.main.transform.eulerAngles.x;
      
        if (cameraAngle <= maxAngleToWalk && cameraAngle >= minAngleToWalk)
        {
            float currentY = transform.position.y;

            transform.position += Camera.main.transform.forward * speed * Time.deltaTime;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), currentY, Mathf.Clamp(transform.position.z, minZ, maxZ));
        }
    }
}