using UnityEngine;

public class FamilyTest12 : MonoBehaviourExtended, ITest
{
    [FamilyComponent(true, -1, SkipItself = true)] // In this case same as [FamilyComponent(SkipItself = true)] and [FamilyComponent(1, SkipItself = true)]
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
            if (name.Equals("Inventory holder F"))
            {
                return true;
            }
        }
        return false;
    }
}