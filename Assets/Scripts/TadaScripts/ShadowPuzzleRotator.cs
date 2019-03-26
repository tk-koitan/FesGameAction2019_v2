using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuzzleRotator : IsActionEnabled
{

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    protected override void UpdatedAction()
    {
        transform.Rotate(0f, 0f, 1f);
    }
}
