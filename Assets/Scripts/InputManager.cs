using UnityEngine;

public class InputManager : MonoBehaviourSingleton<InputManager>, ITest
{
    [GlobalComponent(Of = "Player B", Offset = 1)]
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
        if (spawner == null)
            return false;

        var name = spawner.gameObject.name;
        Debug.Log("Found component Spawner in " + name + " for " + this);
        if (!name.Equals("Main spawner"))
            return false;

        return true;
    }

    public InputManager GetSelf()
    {
        return this;
    }
}