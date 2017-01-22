using UnityEngine;

class Utility
{
    public static T RandomElement<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}