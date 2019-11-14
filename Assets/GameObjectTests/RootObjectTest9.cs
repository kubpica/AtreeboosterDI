using UnityEngine;

public class RootObjectTest9 : MonoBehaviourExtended, ITest
{
    [Root("Player F", FromTop = 2)]
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
            Debug.Log("Found [Root(\"Player F\", FromTop = 2)] GameObject " + name + " of " + this);
            if (name.Equals("Player F"))
            {
                return true;
            }
        }
        return false;
    }
}