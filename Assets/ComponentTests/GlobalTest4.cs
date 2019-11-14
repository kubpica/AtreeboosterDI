using UnityEngine;

public class GlobalTest4 : MonoBehaviourExtended, ITest
{
    [GlobalComponent(Of = "Green spawner")]
    private Spawner spawner;

    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (spawner != null)
        {
            var name = spawner.gameObject.name;
            Debug.Log("Found component Spawner in " + name + " for " + this);
            if (name.Equals("Green spawner"))
            {
                return true;
            }
        }
        return false;
    }
}