using UnityEngine;

public class ParentObjectTest9 : MonoBehaviourExtended, ITest
{
    // In case of combined dependency attributes...
    [Sibling(10)] //it automaticly sets to Optional all but the last one - so if 11th sibling not found...
    [Child("Dead")] //and GameObject named Dead not found in children...
    [Parent("Dead")] //search in parents. (if still not found create new GameObject named Dead and make it parent of this - as the last attribute is not set Optional)
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
            Debug.Log("Found [Parent(\"Blue team\")] GameObject " + name + " of " + this);
            if (name.Equals("Dead") && parentName.Equals("Blue team"))
            {
                return true;
            }
        }
        return false;
    }
}