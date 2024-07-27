using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldUI : StatesBar
{
    private void OnEnable()
    {
        Player.OnTakeDamageEvent+=onTakeDamageEvent;
    }



    public override void OnDisable()
    {
        base.OnDisable();
        Player.OnTakeDamageEvent-=onTakeDamageEvent;
    }
    private void onTakeDamageEvent(Player player)
    {
        UpdateStates(player.Health, player.MaxHealth);
    }
}
