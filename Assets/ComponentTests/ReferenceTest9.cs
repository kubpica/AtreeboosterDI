using System.Collections;
using UnityEngine;

public class ReferenceTest9 : MonoBehaviourExtended, ITest
{
    [ReferencePoint]
    public GameObject go;

    [ReferencePoint]
    public GameObject go2;

    [ReferencePoint]
    public GameObject go3;

    [ReferenceComponent(true, Optional = true)]
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