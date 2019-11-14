using UnityEngine;

public class ReferenceObjectTest1 : MonoBehaviourExtended, ITest
{
    [Reference("Inventory holder G")]
    private GameObject go;
    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (go != null)
        {
            var name = go.gameObject.name;
            Debug.Log("Found [Reference(\"Inventory holder G\")] GameObject " + name + " of " + this);
            if (name.Equals("Inventory holder G"))
            {
                return true;
            }
        }
        return false;
    }
}