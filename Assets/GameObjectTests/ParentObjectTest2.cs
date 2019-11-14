using UnityEngine;

public class ParentObjectTest2 : MonoBehaviourExtended, ITest
{
    [Parent("Blue team")]
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
            Debug.Log("Found [Parent(\"Blue team\")] GameObject " + name + " of " + this);
            if (name.Equals("Blue team"))
            {
                return true;
            }
        }
        return false;
    }
}