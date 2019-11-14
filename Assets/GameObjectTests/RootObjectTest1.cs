using UnityEngine;

public class RootObjectTest2 : MonoBehaviourExtended, ITest
{
    [Root(2)]
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
            Debug.Log("Found [Root(2)] GameObject " + name + " of " + this);
            if (name.Equals("Player F"))
            {
                return true;
            }
        }
        return false;
    }
}