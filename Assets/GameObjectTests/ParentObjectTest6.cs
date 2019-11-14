using UnityEngine;

public class ParentObjectTest6 : MonoBehaviourExtended, ITest
{
    [Parent(99)]
    private GameObject go;
    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (go == null)
        {
            return true;
        }
        return false;
    }
}