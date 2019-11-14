using UnityEngine;

public class ChildObjectTest5 : MonoBehaviourExtended, ITest
{
    [Child("Raptor")]
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
            Debug.Log("Found [Child(\"Raptor\")] GameObject " + name + " of " + this);
            if (name.Equals("Raptor"))
            {
                return true;
            }
        }
        return false;
    }
}