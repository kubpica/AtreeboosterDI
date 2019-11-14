using UnityEngine;

public class ChildTest7 : MonoBehaviourExtended, ITest
{
    [ChildComponent(Of = "Player L")]
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
            if (name.Equals("Inventory") && parentName.Equals("Player L"))
            {
                return true;
            }
        }
        return false;
    }
}