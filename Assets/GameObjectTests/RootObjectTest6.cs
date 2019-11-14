using UnityEngine;

public class RootObjectTest6 : MonoBehaviourExtended, ITest
{
    [Root("Dead", FromTop = 1)]
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
            Debug.Log("Found [Root(\"Dead\", FromTop = 1)] GameObject " + name + " of " + this);
            if (name.Equals("Dead") && parentName.Equals("Blue team"))
            {
                return true;
            }
        }
        return false;
    }
}