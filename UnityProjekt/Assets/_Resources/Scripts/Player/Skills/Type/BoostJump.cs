﻿using UnityEngine;

public class BoostJump : PlayerSkill
{
    public Vector2 direction;

    public BoostJump(string name, float skillCooldown)
        : base(name, skillCooldown)
    {

    }

    public override void LateUpdateSkill(PlayerClass player)
    {
        base.LateUpdateSkill(player);

        if(skillRunTimer > 0f)
            player.overrideVelocity += direction * player.GetAttributeValue(AttributeType.ATTACKSPEED);
    }

    public override void Do(PlayerClass player)
    {
        base.Do(player);

        player.overrideVelocity += direction * player.GetAttributeValue(AttributeType.ATTACKSPEED);
    }
}

