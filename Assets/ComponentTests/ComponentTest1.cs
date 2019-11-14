using UnityEngine;

public class ComponentTest1 : MonoBehaviourExtended, ITest
{
    [Component]
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
            if (name.Equals("Blue spawner"))
            {
                return true;
            }
        }
        return false;
    }
}