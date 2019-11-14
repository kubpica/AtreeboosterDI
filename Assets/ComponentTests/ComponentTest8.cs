using System.Collections;
using UnityEngine;

public class ComponentTest8 : MonoBehaviourExtended, ITest
{
    [Component]
    private Monster3 monster;

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
            Debug.Log("Found component Monster3 in " + name + " for " + this);
            if (name.Equals("Monster3") && monster.transform.parent == null)
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}