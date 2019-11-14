using UnityEngine;

public class ReferenceTest2 : MonoBehaviourExtended, ITest
{
    [ReferencePoint]
    public GameObject go;

    [ReferenceComponent(Of = "Inventory holder C")]
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