using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewark : MonoBehaviour
{
    private List<SoundFX> _firearmSoundFXList = new List<SoundFX>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _firearmSoundFXList.Add(transform.GetChild(i).GetComponent<SoundFX>());
        }
    }


    public void PlayFX()
    {
        for (int i = 0; i < _firearmSoundFXList.Count; i++)
        {
            _firearmSoundFXList[i].StartFX();
        }
    }
}
