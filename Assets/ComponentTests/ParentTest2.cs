﻿using UnityEngine;

public class ParentTest2 : MonoBehaviourExtended, ITest
{
    [ParentComponent(SkipItself = true)]
    private readonly Inventory inventory;

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
            if (name.Equals("Blue team"))
            {
                return true;
            }
        }
        return false;
    }
}