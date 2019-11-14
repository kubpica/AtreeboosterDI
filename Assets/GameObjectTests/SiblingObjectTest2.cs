using System.Collections;
using UnityEngine;

public class SiblingObjectTest2 : MonoBehaviourExtended, ITest
{
    [Sibling(1, Of = "ConfigManager")]
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
            Debug.Log("Found [Sibling(1, Of = \"ConfigManager\")] GameObject " + name + " of " + this);
            if (name.Equals("Dead"))
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}