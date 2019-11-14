using UnityEngine;

public class SiblingObjectTest1 : MonoBehaviourExtended, ITest
{
    [Sibling(1)]
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
            Debug.Log("Found [Sibling(1)] GameObject " + name + " of " + this);
            if (name.Equals("Player B"))
            {
                return true;
            }
        }
        return false;
    }
}