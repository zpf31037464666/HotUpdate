using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    /// <summary>
    /// 洗牌算法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputList"></param>
    /// <returns></returns>
    public static List<T> ShuffleList<T>(List<T> inputList)
    {
        List<T> shuffledList = new List<T>(inputList);

        int n = shuffledList.Count;
        while (n > 1)
        {
            n--;
            System.Random random = new System.Random();
            int k = random.Next(n + 1);
            T value = shuffledList[k];
            shuffledList[k] = shuffledList[n];
            shuffledList[n] = value;
        }
        return shuffledList;
    }

    /// <summary>
    /// 获得子物体的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="list"></param>
    public static void GetAllChildObjects<T>(Transform parent, List<T> list) where T : Component
    {
        list.Clear();  // 清空列表
                       // list = new List<T>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);  // 获取子物体
            var component = child.GetComponent<T>();
            if (component != null)
            {
                list.Add(component);  // 将符合条件的子物体添加到列表中
            }
        }
    }
    //冒泡排序
    public static void BubbleSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1)
            return;

        int n = array.Length;
        bool swapped;

        do
        {
            swapped = false;
            for (int i = 0; i < n - 1; i++)
            {
                if (array[i].CompareTo(array[i + 1]) > 0)
                {
                    Swap(array, i, i + 1);
                    swapped = true;
                }
            }
            n--;
        } while (swapped);
    }

    //委托 冒泡排序法 泛型
    public void BubbleSort<T>(T[] playerStatuses, Func<T, T, bool> func)
    {
        bool isSwapped = false;
        do
        {
            isSwapped=false;
            for (int i = 0; i <playerStatuses.Length-1; i++)
            {
                if (func(playerStatuses[i], playerStatuses[i+1]))
                {
                    T temp = playerStatuses[i];
                    playerStatuses[i]= playerStatuses[i+1];
                    playerStatuses[i+1]= temp;
                    isSwapped=true;
                }
            }
        } while (isSwapped);
    }


    private static void Swap<T>(T[] array, int i, int j)
    {
        T temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
}
