using UnityEngine;

public class GlobalTest5 : MonoBehaviourExtended, ITest
{
    [GlobalComponent]
    private InputManager inputManager;

    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (inputManager != null)
        {
            var name = inputManager.gameObject.name;
            Debug.Log("Found component InputManager in " + name + " for " + this);
            if (name.Equals("InputManager"))
            {
                if (inputManager.GetSelf() == InputManager.Instance.GetSelf())
                    return true;
            }
        }
        return false;
    }
}