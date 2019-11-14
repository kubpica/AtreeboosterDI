using System.Collections;
using UnityEngine;

public class ReferenceObjectTest2 : MonoBehaviourExtended, ITest
{
    [Reference("Inventory holder M")]
    private Transform go;
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
            Debug.Log("Found [Reference(\"Inventory holder M\")] GameObject " + name + " of " + this);
            if (name.Equals("Inventory holder M"))
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}