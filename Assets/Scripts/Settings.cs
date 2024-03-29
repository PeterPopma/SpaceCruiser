using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static float TimeScale = 1;  // in years/second
    public static float SpaceshipSpeed = 61500;  // in km/h
    public static string SpaceshipDestination = "Mars";
    public static int planetSize = 100;    // planets can be drawn 100 times larger so they are visible together with sun
}
