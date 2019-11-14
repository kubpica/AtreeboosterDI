using UnityEngine;

public class SiblingObjectTest6 : MonoBehaviourExtended, ITest
{
    [Sibling("Red spawner")]
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
            Debug.Log("Found [Sibling(\"Red spawner\")] GameObject " + name + " of " + this);
            if (name.Equals("Red spawner"))
            {
                return true;
            }
        }
        return false;
    }
}