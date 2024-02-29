using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : PlayerDetector
{
    protected override void DoSomething()
    {
        m_Player.SpawnPosition = transform.position;
    }
}
