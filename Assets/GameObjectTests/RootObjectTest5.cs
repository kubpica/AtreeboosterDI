using System.Collections;
using UnityEngine;

public class RootObjectTest5 : MonoBehaviourExtended, ITest
{
    [Root("Dead")]
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
        StartCoroutine(DelayedTest());
        if (go != null)
        {
            var name = go.gameObject.name;
            var parentName = go.transform.parent.name;
            if (name.Equals("Dead") && parentName.Equals("Blue team"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (go != null)
        {
            var name = go.gameObject.name;
            Debug.Log("Found [Reference(\"Dead\")] GameObject " + name + " of " + this);
            if (name.Equals("Dead") && go.transform.parent == null)
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}