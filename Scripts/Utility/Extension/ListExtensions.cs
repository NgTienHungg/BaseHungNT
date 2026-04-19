using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Remove<T>(this IList<T> list, IList<T> removeList)
    {
        foreach (var item in removeList)
        {
            list.Remove(item);
        }
    }

    public static void Remove<T>(this IList<T> list, T[] removeArray)
    {
        foreach (var item in removeArray)
        {
            list.Remove(item);
        }
    }

    /// <summary>
    /// Gán giá trị cho tất cả phần tử trong IList.
    /// </summary>
    public static void Fill<T>(this IList<T> list, T value)
    {
        for (var i = 0; i < list.Count; i++)
        {
            list[i] = value;
        }
    }

    /// <summary>
    /// Gán giá trị cho tất cả phần tử trong mảng.
    /// </summary>
    public static void Fill<T>(this T[] array, T value)
    {
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
    }

    /// <summary>
    /// Gán giá trị cho tất cả phần tử trong ma trận.
    /// </summary>
    public static void Fill<T>(this T[,] array, T value)
    {
        for (var i = 0; i < array.GetLength(0); i++)
        {
            for (var j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = value;
            }
        }
    }

    /// <summary>
    /// Trộn ngẫu nhiên các phần tử trong danh sách.
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    /// <summary>
    /// Trả về một phần tử ngẫu nhiên từ danh sách, hoặc default nếu danh sách rỗng.
    /// </summary>
    public static T RandomElement<T>(this IList<T> list)
    {
        var n = list.Count;
        if (n == 0)
        {
            Debug.LogError("List is empty");
            return default;
        }

        var randomIndex = Random.Range(0, n);
        return list[randomIndex];
    }
}