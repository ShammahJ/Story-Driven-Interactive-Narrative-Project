
using System.Collections;
using UnityEngine;

public class TimedText : MonoBehaviour
{
    public GameObject textObject;
    public float displayTime = 3f;

    void Start()
    {
        StartCoroutine(HideText());
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(displayTime);

        textObject.SetActive(false);
    }
}
