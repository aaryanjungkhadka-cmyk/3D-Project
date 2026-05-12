using UnityEngine;
using System.Collections;

public class PoliceArrival : MonoBehaviour
{
    public float driveSpeed = 12f;
    public float driveDuration = 2.5f; // Adjust this to stop exactly at your gate
    public GameObject[] policeOfficers; // Array to hold multiple police objects

    void Start()
    {
        // Automatically hide all police at the very start
        foreach (GameObject cop in policeOfficers)
        {
            if (cop != null) cop.SetActive(false);
        }

        // Start the arrival sequence
        StartCoroutine(ArrivalSequence());
    }

    IEnumerator ArrivalSequence()
    {
        float timer = 0;

        // 1. Drive the car forward
        while (timer < driveDuration)
        {
            transform.Translate(Vector3.forward * driveSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // 2. Short pause after stopping
        yield return new WaitForSeconds(0.5f);

        // 3. Make the police appear
        foreach (GameObject cop in policeOfficers)
        {
            if (cop != null) cop.SetActive(true);
        }
    }
}