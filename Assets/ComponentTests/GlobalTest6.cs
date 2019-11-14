using System.Collections;
using UnityEngine;

public class GlobalTest6 : MonoBehaviourExtended, ITest
{
    [GlobalComponent]
    private GlobalTest5 test;

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
        if (test == null)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (test != null)
        {
            var name = test.gameObject.name;
            Debug.Log("Found component GlobalTest5 in " + name + " for " + this);
            if (name.Equals("GlobalTest5"))
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}