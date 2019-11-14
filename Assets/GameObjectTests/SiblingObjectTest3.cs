using UnityEngine;

public class SiblingObjectTest3 : MonoBehaviourExtended, ITest
{
    [Sibling(6)]
    private Transform sibling;
    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (sibling != null)
        {
            var name = sibling.gameObject.name;
            Debug.Log("Found [Sibling(6)] GameObject " + name + " of " + this);
            if (name.Equals("sibling"))
            {
                return true;
            }
        }
        return false;
    }
}