using UnityEngine;

public class RootObjectTest10 : MonoBehaviourExtended, ITest
{
    [Root("Player F", FromTop = 999)]
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
            Debug.Log("Found [Root(\"Player F\", FromTop = 999)] GameObject " + name + " of " + this);
            if (name.Equals("Player F") && parentName.Equals("GameObject"))
            {
                return true;
            }
        }
        return false;
    }
}