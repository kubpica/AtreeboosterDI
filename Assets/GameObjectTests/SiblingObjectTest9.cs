using UnityEngine;

public class SiblingObjectTest9 : MonoBehaviourExtended, ITest
{
    [Sibling("Player F")]
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
            Debug.Log("Found [Sibling(\"Player F\")] GameObject " + name + " of " + this);
            if (name.Equals("Player F") && parentName.Equals("GameObject"))
            {
                return true;
            }
        }
        return false;
    }
}