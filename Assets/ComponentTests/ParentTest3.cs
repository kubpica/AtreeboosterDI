using UnityEngine;

public class ParentTest3 : MonoBehaviourExtended, ITest
{
    [ParentComponent]
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
            if (name.Equals("Dead"))
            {
                return true;
            }
        }
        return false;
    }
}