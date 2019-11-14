using System.Collections;
using UnityEngine;

public class FamilyTest13 : MonoBehaviourExtended, ITest
{
    [FamilyComponent(Optional = true)]
    private Monster4 monster;

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
        if (monster == null)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (monster == null)
        {
            yield break;
        }
        Debug.LogError("Test failed: " + this);
    }
}