using UnityEngine;

public class ParentObjectTest5 : MonoBehaviourExtended, ITest
{
    [Parent("go2")]
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
            Debug.Log("Found [Parent(\"go2\")] GameObject " + name + " of " + this);
            if (name.Equals("go2"))
            {
                return true;
            }
        }
        return false;
    }
}