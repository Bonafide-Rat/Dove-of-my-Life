using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUniqueFollower : UniqueFollower
{
    public override void UseAbility()
    {
        Debug.Log("AbilityUsed");
    }
}
