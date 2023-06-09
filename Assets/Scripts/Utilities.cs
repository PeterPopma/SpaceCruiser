using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public const float KM_PER_SQUARE = 2785400f;     // planets are drawn 100 times larger to show them

    public static float KmToSquares(float km)
    {
        return km / KM_PER_SQUARE;
    }
}
