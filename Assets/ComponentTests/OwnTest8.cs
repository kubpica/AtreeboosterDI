using System.Collections;
using UnityEngine;

public class OwnTest8 : MonoBehaviourExtended, ITest
{
    [OwnComponent(Optional = true)]
    private Inventory inv;

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
        if (inv == null)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (inv == null)
        {
            yield break;
        }
        Debug.LogError("Test failed: " + this);
    }
}