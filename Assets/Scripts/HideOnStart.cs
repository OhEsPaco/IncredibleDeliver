using UnityEngine;

public class HideOnStart : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer rendererToDisable;

    // Start is called before the first frame update
    private void Start()
    {
        if (rendererToDisable)
        {
            rendererToDisable.enabled = false;
        }
    }
}