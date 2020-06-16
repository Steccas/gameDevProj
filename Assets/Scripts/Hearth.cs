﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearth : PowerUp
{
    protected override void Pickup()
    {
        base.Pickup();
        audioManager.Play("HealthPickup");
        playerObj.AddHealth(value);
        Destroyer();
    }
}
