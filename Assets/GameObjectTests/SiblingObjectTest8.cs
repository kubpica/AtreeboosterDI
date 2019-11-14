using UnityEngine;

public class SiblingObjectTest8 : MonoBehaviourExtended, ITest
{
    [Sibling("Raptor")]
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
            Debug.Log("Found [Sibling(\"Raptor\")] GameObject " + name + " of " + this);
            if (name.Equals("Raptor"))
            {
                return true;
            }
        }
        return false;
    }
}