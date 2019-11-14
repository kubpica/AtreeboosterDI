using UnityEngine;

public class ChildTest10 : MonoBehaviourExtended, ITest
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
        if (inventory == null)
        {
            return true;
        }
        return false;
    }
}