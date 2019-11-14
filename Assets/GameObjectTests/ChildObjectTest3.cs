using System.Collections;
using UnityEngine;

public class ChildObjectTest3 : MonoBehaviourExtended, ITest
{
    [Child("Inventory holder M", Offset = 999)]
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
            Debug.Log("Found [Child(\"Inventory holder M\")] GameObject " + name + " of " + this);
            if (name.Equals("Inventory holder M"))
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}