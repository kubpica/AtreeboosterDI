using System.Collections;
using UnityEngine;

public class OwnTest4 : MonoBehaviourExtended, ITest
{
    [OwnComponent(Of = "Main spawner")]
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
        if (spawner == null)
        {
            return true;
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