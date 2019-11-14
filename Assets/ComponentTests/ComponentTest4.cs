using UnityEngine;

public class ComponentTest4 : MonoBehaviourExtended, ITest
{
    [Component]
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