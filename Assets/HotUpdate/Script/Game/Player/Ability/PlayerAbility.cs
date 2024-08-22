using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    private Player player;

    public List<Ability> abilities = new List<Ability>(); // 能力列表

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        //abilities.Add(AbilityManager.instance.GetAbility("增伤能力"));
        //abilities.Add(AbilityManager.instance.GetAbility("加血能力"));
    }
    public void OnKillEnemy()
    {
        Debug.Log("敌人死亡触发能力");

        var KillEmenyAbilitys = abilities.OfType<KillEnemyAbillity>().ToList();

        foreach (var ablility in KillEmenyAbilitys)
        {
            ablility.Activate(player);
        }
    }

    public void AddAbility(Ability ability)
    {
        Debug.Log("PlayerAbility addAblity");

        if (!abilities.Contains(ability))
        {
            abilities.Add(ability);
        }
    }


}
