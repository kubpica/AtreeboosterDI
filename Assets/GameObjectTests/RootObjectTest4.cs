using UnityEngine;

public class RootObjectTest4 : MonoBehaviourExtended, ITest
{
    [Root("Red team")]
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
            Debug.Log("Found [Root(\"Red team\")] GameObject " + name + " of " + this);
            if (name.Equals("Red team"))
            {
                return true;
            }
        }
        return false;
    }
}