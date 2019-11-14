using UnityEngine;

namespace AtreeboosterDemo
{
    public class GlobalDemo1 : MonoBehaviourExtended
    {
        [GlobalComponent]
        private SingletonDemo spawner;
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
                Debug.Log("Found component SingletonDemo in " + name + " for " + this);
                if (name.Equals("Sound manager"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}