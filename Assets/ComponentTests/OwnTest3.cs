using UnityEngine;

public class OwnTest3 : MonoBehaviourExtended, ITest
{
    [OwnComponent(Of = "__app")]
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