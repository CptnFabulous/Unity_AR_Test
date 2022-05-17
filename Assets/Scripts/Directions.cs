using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Directions
{
    public static readonly Vector3[] all = new Vector3[]
    {
        Vector3.right,
        Vector3.left,
        Vector3.up,
        Vector3.down,
        Vector3.forward,
        Vector3.back,
    };

    public static Vector3 LocalToCardinal(Vector3 localDirection)
    {
        int closest = 0; // Start off with the second entry. If a higher one is found, change it. If not, keep the current one. There cannot be two equal values unless the Vector3 is zero.
        float closestDot = Mathf.NegativeInfinity;
        for (int i = 0; i < all.Length; i++)
        {
            float newDot = Vector3.Dot(localDirection, all[i]);
            if (newDot > closestDot)
            {
                closest = i; // Saves index for closest value
                closestDot = newDot; // And its dot product so it doesn't need to be calculated again.
            }
        }
        return all[closest];
    }
}
