using UnityEngine;

public class FamilyTest6 : MonoBehaviourExtended, ITest
{
    [FamilyComponent(SkipItself = true)]
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
            if (name.Equals("Inventory holder G"))
            {
                return true;
            }
            if (transform.position.z != -10)
                Debug.LogError("Position of the gameobject changed!");
        }
        return false;
    }
}