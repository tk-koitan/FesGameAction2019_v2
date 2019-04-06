using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerMaterial : MonoBehaviour
{
    static public void SetMaterial(SpriteRenderer spriteRen, Material material)
    {
        spriteRen.material = material;
    }
}
