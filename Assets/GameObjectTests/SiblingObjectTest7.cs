using System.Collections;
using UnityEngine;

public class SiblingObjectTest7 : MonoBehaviourExtended, ITest
{
    [Sibling("ConfigManager")]
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
            Debug.Log("Found [Reference(\"ConfigManager\")] GameObject " + name + " of " + this);
            if (name.Equals("ConfigManager"))
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}