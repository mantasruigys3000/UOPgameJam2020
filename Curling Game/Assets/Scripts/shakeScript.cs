using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeScript : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject menuScreen2;
    public GameObject menuScreen3;
    private bool coroutineAllowed;

    // Start is called before the first frame update
    void Start()
    {
        coroutineAllowed = true;
        StartCoroutine("StartPulsing");
    }

    public IEnumerator StartPulsing()
    {
        coroutineAllowed = false;
        menuScreen.transform.localScale = Vector3.Lerp(menuScreen.transform.localScale, menuScreen2.transform.localScale, 2f * Time.deltaTime);
        yield return new WaitForSeconds(2f * Time.deltaTime);
        menuScreen.transform.localScale = Vector3.Lerp(menuScreen.transform.localScale, menuScreen3.transform.localScale, 2f * Time.deltaTime);
        yield return new WaitForSeconds(2f * Time.deltaTime);
        coroutineAllowed = true;
    }
}
