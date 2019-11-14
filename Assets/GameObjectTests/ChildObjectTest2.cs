using UnityEngine;

public class ChildObjectTest2 : MonoBehaviourExtended, ITest
{
    [Child("Inventory holder F")]
    private Transform go;
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
            Debug.Log("Found [Child(\"Inventory holder F\")] GameObject " + name + " of " + this);
            if (name.Equals("Inventory holder F"))
            {
                return true;
            }
        }
        return false;
    }
}