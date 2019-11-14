using UnityEngine;

public class SiblingObjectTest5 : MonoBehaviourExtended, ITest
{
    [Sibling(3, Of = "Black team")]
    private GameObject ddolsSibling;
    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (ddolsSibling != null)
        {
            var name = ddolsSibling.gameObject.name;
            Debug.Log("Found [Sibling(3, Of = \"Black team\")] GameObject " + name + " of " + this);
            if (name.Equals("ddolsSibling"))
            {
                return true;
            }
        }
        return false;
    }
}