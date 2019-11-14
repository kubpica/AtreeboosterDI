using UnityEngine;

public class SiblingObjectTest4 : MonoBehaviourExtended, ITest
{
    [Sibling(0, Of = "Black team")]
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
            Debug.Log("Found [Sibling(1, Of = \"Black team\")] GameObject " + name + " of " + this);
            if (name.Equals("__app"))
            {
                return true;
            }
        }
        return false;
    }
}