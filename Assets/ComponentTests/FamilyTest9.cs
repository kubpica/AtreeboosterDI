using System.Collections;
using UnityEngine;

public class FamilyTest9 : MonoBehaviourExtended, ITest
{
    [FamilyComponent(SkipItself = true)]
    private Monster monster;

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
        if (monster != null)
        {
            var name = monster.gameObject.name;
            var parentName = monster.transform.parent.name;
            Debug.Log("Found component Monster in " + name + " of " + parentName + " for " + this);
            if (name.Equals("Dead") && parentName.Equals("Red team"))
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}