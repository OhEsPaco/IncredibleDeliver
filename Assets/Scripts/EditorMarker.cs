#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

//Los objetos que tienen este script añadido muestran su nombre y posición
//en el editor
public class EditorMarker : MonoBehaviour
{
    [SerializeField]
    private Color gizmoColor = Color.green;

    [SerializeField]
    private float radious = 0.1f;

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radious);
        Handles.Label(transform.position, name);
#endif
    }
}