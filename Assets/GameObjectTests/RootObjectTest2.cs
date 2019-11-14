using UnityEngine;

public class RootObjectTest1 : MonoBehaviourExtended, ITest
{
    [Root]
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
            Debug.Log("Found [Root] GameObject " + name + " of " + this);
            if (name.Equals("Blue team"))
            {
                return true;
            }
        }
        return false;
    }
}