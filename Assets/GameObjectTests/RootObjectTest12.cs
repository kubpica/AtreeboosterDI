using UnityEngine;

public class RootObjectTest12 : MonoBehaviourExtended, ITest
{
    [Root("Player E", FromTop = 5)]
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
            var parentName = go.transform.parent.name;
            Debug.Log("Found [Root(\"Player E\", FromTop = 5)] GameObject " + name + " of " + this);
            if (name.Equals("Player E") && parentName.Equals("GameObject"))
            {
                return true;
            }
        }
        return false;
    }
}