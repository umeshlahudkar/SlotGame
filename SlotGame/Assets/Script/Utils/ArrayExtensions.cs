using UnityEngine;
using System;

public static class ArrayExtensions
{
    public static void Shuffle<T>(this T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }

    public static void Clear(this int[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            array[i] = -1;
        }
    }
}