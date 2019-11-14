using UnityEngine;

namespace AtreeboosterDemo
{
    public class SiblingDemo1 : MonoBehaviourExtended
    {
        [SiblingComponent]
        private InventoryDemo spawner;
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
                Debug.Log("Found component InventoryDemo in " + name + " for " + this);
                if (name.Equals("Team Blue")) // Roots are searched in alphabetical order as order of root GameObjects in scene is not guaranteed; siblings having common parent are traversed by siblingIndex in the hierarchy
                {
                    return true;
                }
            }
            return false;
        }
    }
}