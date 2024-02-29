using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrapTrigger : PlayerDetector
{

    [SerializeField] bool m_CanDetect = true;
    public Action OnPlayerDetected;

    protected override void DoSomething()
    {
        OnPlayerDetected?.Invoke();
    }

    public void SetCanDetect(bool v)
    {
        m_CanDetect = v;
    }
}
