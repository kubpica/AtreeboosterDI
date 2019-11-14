using UnityEngine;

public class ChildObjectTest4 : MonoBehaviourExtended, ITest
{
    [Child(3)]
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
            Debug.Log("Found [Child(3)] GameObject " + name + " of " + this);
            if (name.Equals("go"))
            {
                return true;
            }
        }
        return false;
    }
}