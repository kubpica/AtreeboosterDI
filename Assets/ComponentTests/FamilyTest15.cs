using UnityEngine;

public class FamilyTest15 : MonoBehaviourExtended, ITest
{
    [FamilyComponent(-1, Of = "Dead", Offset = 1, SkipItself = true)]
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
            if (name.Equals("Inventory holder C"))
            {
                return true;
            }
        }
        return false;
    }
}