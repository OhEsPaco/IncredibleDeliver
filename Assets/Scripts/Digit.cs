using UnityEngine;

//Cada uno de los digitos de un Counter
public class Digit : MonoBehaviour
{
    [SerializeField]
    private GameObject[] digits;

    private void Awake()
    {
        HideDigits();
    }

    public void SetDigit(int digit)
    {
        if (digit < 0)
        {
            digit = 0;
        }

        if (digit > 9)
        {
            digit = 9;
        }

        HideDigits();

        digits[digit].SetActive(true);
    }

    private void HideDigits()
    {
        foreach (GameObject digit in digits)
        {
            digit.SetActive(false);
        }
    }

    public void SetMaterial(Material m)
    {
        foreach (GameObject digit in digits)
        {
            digit.GetComponent<Renderer>().material = m;
        }
    }
}