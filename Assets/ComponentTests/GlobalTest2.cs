using System.Collections;
using UnityEngine;

public class GlobalTest2 : MonoBehaviourExtended, ITest
{
    [GlobalComponent]
    private Spawner spawner;

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
        if (spawner != null)
        {
            var name = spawner.gameObject.name;
            //Debug.Log("Found component Spawner in " + name + " for " + this);
            if (name.Equals("Blue spawner"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (spawner != null)
        {
            var name = spawner.gameObject.name;
            Debug.Log("Found component Spawner in " + name + " for " + this);
            if (name.Equals("Main spawner"))
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}