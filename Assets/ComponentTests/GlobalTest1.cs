using UnityEngine;

public class GlobalTest1 : MonoBehaviourExtended, ITest
{
    [GlobalComponent]
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
                if (soundManager.GetSelf() == SoundManager.Instance.GetSelf())
                    return true;
            }
        }
        return false;
    }
}