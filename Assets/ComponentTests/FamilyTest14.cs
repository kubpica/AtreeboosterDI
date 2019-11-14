using UnityEngine;

public class FamilyTest14 : MonoBehaviourExtended, ITest
{
    [FamilyComponent(-999, Offset = 999, SkipItself = true)] // Offset 999 makes search start-point null which makes -999 have no effect and starts searching for Inventory from scenes roots (first scene to traverse is this.gameObject.scene). 
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
            if (name.Equals("Blue team")) // Order of roots in a scene in not guaranted (editor vs build) so it traverses roots in alphabetical order.
            {
                return true;
            }
        }
        return false;
    }
}