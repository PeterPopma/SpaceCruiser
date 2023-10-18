using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    [SerializeField] float minimumViewDistance;
    [SerializeField] Vector3 startPosition;

    public float MinimumViewDistance { get => minimumViewDistance; set => minimumViewDistance = value; }
    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }

}
