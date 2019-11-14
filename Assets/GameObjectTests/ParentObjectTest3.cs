using UnityEngine;

public class ParentObjectTest3 : MonoBehaviourExtended, ITest
{
    [Parent(2)]
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
            Debug.Log("Found [Parent(2)] GameObject " + name + " of " + this);
            if (name.Equals("Blue team"))
            {
                return true;
            }
        }
        return false;
    }
}