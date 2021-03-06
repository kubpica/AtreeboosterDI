﻿using UnityEngine;

public class ParentObjectTest7 : MonoBehaviourExtended, ITest
{
    [Parent("Player F")]
    private GameObject go;
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
            var parentName = go.transform.parent.name;
            Debug.Log("Found [Parent(\"Player F\")] GameObject " + name + " of " + this);
            if (name.Equals("Player F") && parentName.Equals("Player F"))
            {
                return true;
            }
        }
        return false;
    }
}