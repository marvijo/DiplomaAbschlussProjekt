﻿using UnityEngine;

[System.Serializable]
public class Item_DamageAmplifier : Item
{
    protected override string[] names
    {
        get
        {
            return new string[] { "Knive", "Hammer", "Sword" };
        }
    }

    public override string Description
    {
        get
        {
            return "<color=" + colors[prefixID] + ">" + Name + "</color>\nDamage mult: " + DamageAmplifier.ToString("#0%");
        }
    }

    public float DamageAmplifier = 0.15f;

    public override void UpdateStats(float value)
    {
        DamageAmplifier *= value;
    }

    public override void Start(PlayerClass playerClass)
    {
        base.Start(playerClass);

        playerClass.GetAttribute(AttributeType.DAMAGE).AddMult(DamageAmplifier);
    }
}
