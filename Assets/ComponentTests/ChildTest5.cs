using UnityEngine;

public class ChildTest5 : MonoBehaviourExtended, ITest
{
    [ChildComponent(99)]
    private SoundManager soundManager;
    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (soundManager != null)
        {
            var name = soundManager.gameObject.name;
            Debug.Log("Found component SoundManager in " + name + " for " + this);
            if (name.Equals("__app"))
            {
                return true;
            }
        }
        return false;
    }
}