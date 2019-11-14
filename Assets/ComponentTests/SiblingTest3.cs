using UnityEngine;

public class SiblingTest3 : MonoBehaviourExtended, ITest
{
    [SiblingComponent(1, SkipItself = true)]
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
            if (name.Equals("Green team"))
            {
                return true;
            }
        }
        return false;
    }
}