using System;
using UnityEngine;

public class ChildObjectTest1 : MonoBehaviourExtended, ITest
{
    [Child(1)]
    [NonSerialized]
    public GameObject go;

    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (go != null)
        {
            var name = go.gameObject.name;
            Debug.Log("Found [Child(1)] GameObject " + name + " of " + this);
            if (name.Equals("Player B"))
            {
                return true;
            }
        }
        return false;
    }
}