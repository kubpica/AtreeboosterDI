using UnityEngine;

public class ParentTest6 : MonoBehaviourExtended, ITest
{
    [Child("Blue spawner")] // Automaticly optional as there are multiple dependency attributes
    [ReferencePoint]
    [Parent("Blue spawner")] // Automaticly optional
    [Reference("Blue spawner")] // Automaticly optional
    [Root("Red team")] // Not optional, but in this case [Reference("Blue spawner")] will be found so the rest (this) is skiped
    private GameObject goRef;

    [ParentComponent] // Search in parent-GameObjects of this component. (Automaticly optional)
    [ChildComponent(Of = "goRef")] // Search in children of goRef (GameObject pointed by the field above). (Automaticly optional)
    [ParentComponent(Of = "goRef")] // Search in parents of goRef... (Automaticly optional)
    [OwnComponent] // Automaticly optional, but in this case skiped as an Inventory component will be found by [ParentComponent(Of = "goRef")] so the rest is skiped
    [GlobalComponent] // Not optional, but in this case skiped...
    private Inventory inventory;

    [OwnComponent(Optional = true)]
    private Inventory inv2;

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
            if (name.Equals("Blue team") && inv2 == null && goRef.name == "Blue spawner")
            {
                return true;
            }
        }
        return false;
    }
}