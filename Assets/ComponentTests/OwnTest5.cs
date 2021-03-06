﻿using System.Collections;
using UnityEngine;

public class OwnTest5 : MonoBehaviourExtended, ITest
{
    [OwnComponent(Of = "Inventory holder K")]
    private Inventory inventory;

    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (inventory != null)
        {
            var name = inventory.gameObject.name;
            Debug.Log("Found component Inventory in " + name + " for " + this);
            if (name.Equals("Inventory holder K"))
            {
                return true;
            }
        }
        return false;
    }
}