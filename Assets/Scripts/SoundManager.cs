using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    public void Sound()
    {
        Debug.Log("Sound played by " + this);
    }

    public SoundManager GetSelf()
    {
        return this;
    }
}
