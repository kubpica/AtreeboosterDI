using UnityEngine;

public class ParentObjectTest1 : MonoBehaviourExtended, ITest
{
    [Parent]
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
            Debug.Log("Found parent GameObject " + name + " of " + this);
            if (name.Equals("Red team"))
            {
                return true;
            }
        }
        return false;
    }
}