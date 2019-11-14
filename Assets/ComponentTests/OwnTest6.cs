using System.Collections;
using UnityEngine;

public class OwnTest6 : MonoBehaviourExtended, ITest
{
    [OwnComponent(-99, Of = "Player I")]
    private Inventory inventory;

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
        if (inventory == null)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (inventory != null)
        {
            var name = inventory.gameObject.name;
            Debug.Log("Found component Inventory in " + name + " for " + this);
            if (name.Equals("Inventory holder I"))
            {
                yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}