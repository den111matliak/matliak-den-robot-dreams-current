using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson3 : MonoBehaviour
{
    [SerializeField] private string value;
    [SerializeField] private List<string> _list;

    [ContextMenu("Hello World")]
    private void HelloWorld()
    {
        Debug.Log("Hello World");
    }

    [ContextMenu("Print")]
    private void Print()
    {
        string msg = "List: ";
        for (int i = 0; i < _list.Count; ++i)
        {
            msg += $"\n{_list[i]}";
        }

        Debug.Log(msg);
    }

    [ContextMenu("Add")]
    private void Add()
    {
        _list.Add(value);
    }

    [ContextMenu("Remove")]
    private void Remove()
    {
        _list.Remove(value);
    }

    [ContextMenu("Sort")]
    private void Sort()
    {
        _list.Sort();
    }

    [ContextMenu("Clear")]
    private void Clear()
    {
        _list.Clear();
    }
}
