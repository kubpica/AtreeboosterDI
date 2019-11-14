using UnityEngine;

public class ChildTest9 : MonoBehaviourExtended
{
    [ChildComponent]
    private IInventory inventory;

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
            var inv = inventory as Inventory;
            var name = inv.gameObject.name;
            Debug.Log("Found component IInventory in " + name + " for " + this);
            if (name.Equals("Inventory holder C"))
            {
                return true;
            }
        }
        return false;
    }
}