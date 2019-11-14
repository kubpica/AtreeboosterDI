using UnityEngine;

public class RootObjectTest3 : MonoBehaviourExtended, ITest
{
    [Root(9999)]
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
            Debug.Log("Found [Root(9999)] GameObject " + name + " of " + this);
            if (name.Equals("Inventory holder E"))
            {
                return true;
            }
        }
        return false;
    }
}