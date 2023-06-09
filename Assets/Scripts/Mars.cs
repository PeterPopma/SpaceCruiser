using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mars : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float angle = (2 * Mathf.PI * Time.time / 1.88085f * Settings.TimeScale) % (2 * Mathf.PI);
        transform.position = new Vector3(Mathf.Cos(angle) * Utilities.KmToSquares(250710000), 0, Mathf.Sin(angle) * Utilities.KmToSquares(250710000));
    }
}
