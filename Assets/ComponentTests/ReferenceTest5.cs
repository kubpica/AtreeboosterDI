using System.Collections;
using UnityEngine;

public class ReferenceTest5 : MonoBehaviourExtended, ITest
{
    [ReferencePoint]
    public GameObject go;

    [ReferencePoint]
    public GameObject go2;

    [ReferenceComponent(Offset = 1)]
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
            if (name.Equals(go2.name))
            {
                return true;
            }
        }
        return false;
    }
}