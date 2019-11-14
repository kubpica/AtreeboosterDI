using System.Collections;
using UnityEngine;

public class RootObjectTest13 : MonoBehaviourExtended, ITest
{
    [Root("Nonexisting root", Optional = true)]
    private Transform go;
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
        if (go == null)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DelayedTest()
    {
        yield return new WaitForSeconds(1);
        if (go == null && GameObject.Find("Nonexisting root") == null)
        {
            yield break;
        }
        Debug.LogError("Test failed: " + this);
    }
}