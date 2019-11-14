using UnityEngine;

public class SiblingTest4 : MonoBehaviourExtended, ITest
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
            Debug.Log("Found component Inventory in " + name + " for " + this);
            if (name.Equals("Inventory holder A2"))
            {
                return true;
            }
        }
        return false;
    }
}