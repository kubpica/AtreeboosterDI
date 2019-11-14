using UnityEngine;

public class ChildTest8 : MonoBehaviourExtended, ITest
{
    [ChildComponent(Optional = true)]
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
        if (inventory == null)
        {
            return true;
        }
        return false;
    }
}