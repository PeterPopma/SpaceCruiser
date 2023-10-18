using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    private GameObject destination;
    private float speed = 16900;        // in m/s

    public GameObject Destination { get => destination; set => destination = value; }
    public float Speed { get => speed; set => speed = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destination!=null)
        {
            Vector3 direction = transform.position - destination.transform.position;
            if (direction.magnitude > 1)
            {
                // TODO : take into account simulation speed
                transform.position += direction.normalized * Time.deltaTime * Speed / (Utilities.KM_PER_SQUARE * 1000);
            }
        }
    }
}
