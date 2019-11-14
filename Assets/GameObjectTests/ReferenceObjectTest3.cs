using System.Collections;
using UnityEngine;

public class ReferenceObjectTest3 : MonoBehaviourExtended, ITest
{
    [Reference("ReferenceObject")]
    private GameObject go;
    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this + " " + go);
        }
    }

    public bool Test()
    {
        StartCoroutine(DelayedTest());
        if (go == null)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (go != null)
        {
            var name = go.gameObject.name;
            Debug.Log("Found [Reference(\"ReferenceObject\")] GameObject " + name + " of " + this);
            if (name.Equals("ReferenceObject"))
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}