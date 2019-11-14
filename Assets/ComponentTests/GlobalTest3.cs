using System.Collections;
using UnityEngine;

public class GlobalTest3 : MonoBehaviourExtended, ITest
{
    [GlobalComponent]
    private ConfigManager configManager;

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
        if (configManager == null)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (configManager != null)
        {
            var name = configManager.gameObject.name;
            Debug.Log("Found component ConfigManager in " + name + " for " + this);
            if (name.Equals("ConfigManager"))
            {
                if (configManager.GetSelf() == ConfigManager.Instance.GetSelf())
                    yield break;
            }
        }
        Debug.LogError("Test failed: " + this);
    }
}