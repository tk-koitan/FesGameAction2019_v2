using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuzzleRotator : Mover
{
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdatedAction()
    {
        transform.Rotate(0f, 0f, 1f);
    }
}
