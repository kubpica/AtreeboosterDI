using System.Collections;
using UnityEngine;

public class GlobalTest7 : MonoBehaviourExtended, ITest
{
    [GlobalComponent]
    private Zombie zombie;

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
        if (zombie == null)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (zombie != null)
        {
            var name = zombie.gameObject.name;
            Debug.Log("Found component Zombie in " + name + " for " + this);
            if (name.Equals("Dead") && zombie.transform.parent == null)
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}