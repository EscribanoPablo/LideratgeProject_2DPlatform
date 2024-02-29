using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : PlayerDetector
{
    protected override void DoSomething()
    {
        GameManager.Instance.SpawnPosition = transform.position;
    }
}
