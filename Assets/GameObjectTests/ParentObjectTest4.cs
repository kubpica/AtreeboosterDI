using UnityEngine;

public class ParentObjectTest4 : MonoBehaviourExtended, ITest
{
    [Parent]
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
            Debug.Log("Found [Parent] GameObject " + name + " of " + this);
            if (name.Equals("go"))
            {
                return true;
            }
        }
        return false;
    }
}