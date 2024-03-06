using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public void PlaySound(string soundName)
    {
        transform.parent.GetComponent<PlayerController>().PlaySound(soundName);
    }
}
