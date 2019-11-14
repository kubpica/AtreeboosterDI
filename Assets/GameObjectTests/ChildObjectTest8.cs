using UnityEngine;

public class ChildObjectTest8 : MonoBehaviourExtended, ITest
{
    [ReferencePoint]
    [Root]
    private Transform rootRef;

    [Child("Red spawner", Of = "rootRef")]
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
            Debug.Log("Found [Child(\"Red spawner\", Of = \"rootRef\")] GameObject " + name + " of " + this);
            if (name.Equals("Red spawner"))
            {
                return true;
            }
        }
        return false;
    }
}