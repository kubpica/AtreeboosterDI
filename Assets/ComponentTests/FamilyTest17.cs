using UnityEngine;

public class FamilyTest17 : MonoBehaviourExtended, ITest
{
    [FamilyComponent(2, Of = "Player E", Offset = -1, SkipItself = true)]
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