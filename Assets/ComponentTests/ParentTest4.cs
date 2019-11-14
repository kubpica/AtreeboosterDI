using UnityEngine;

public class ParentTest4 : MonoBehaviourExtended, ITest
{
    [ParentComponent(99)]
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
            if (name.Equals("Inventory") && inventory.transform.parent == null)
            {
                return true;
            }
        }
        return false;
    }
}