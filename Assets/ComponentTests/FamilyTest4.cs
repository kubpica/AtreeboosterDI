using UnityEngine;

public class FamilyTest4 : MonoBehaviourExtended, ITest
{
    [ReferencePoint]
    public GameObject go;

    [FamilyComponent(Of = "go")]
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
            if (name.Equals("Red team"))
            {
                return true;
            }
        }
        return false;
    }
}