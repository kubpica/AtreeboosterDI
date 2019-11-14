using UnityEngine;

public class ReferenceTest3 : MonoBehaviourExtended, ITest
{
    [ReferencePoint]
    public GameObject go;

    [ReferencePoint]
    public GameObject go2;

    [ReferencePoint]
    public GameObject go3;

    [ReferenceComponent]
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
            if (name.Equals("Inventory holder C"))
            {
                return true;
            }
        }
        return false;
    }
}