using UnityEngine;

public class SiblingTest6 : MonoBehaviourExtended, ITest
{
    [SiblingComponent(SkipItself = true)]
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
            var parentName = inventory.transform.parent.name;
            Debug.Log("Found component Inventory in " + name + " of " + parentName + " for " + this);
            if (name.Equals("Inventory") && parentName.Equals("Player B"))
            {
                return true;
            }
        }
        return false;
    }
}