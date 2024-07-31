using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldUI : StatesBar
{
    private void OnEnable()
    {
        Player.OnChangeHealthEvent+=onTakeDamageEvent;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        Player.OnChangeHealthEvent-=onTakeDamageEvent;
    }
    private void onTakeDamageEvent(Player player)
    {
        UpdateStates(player.Health, player.MaxHealth);
    }
}
