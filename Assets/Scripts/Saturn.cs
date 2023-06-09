using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saturn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = (2 * Mathf.PI * Time.time / 29.4571f * Settings.TimeScale) % (2 * Mathf.PI);
        transform.position = new Vector3(Mathf.Cos(angle) * Utilities.KmToSquares(1463800000), 0, Mathf.Sin(angle) * Utilities.KmToSquares(1463800000));
    }
}
