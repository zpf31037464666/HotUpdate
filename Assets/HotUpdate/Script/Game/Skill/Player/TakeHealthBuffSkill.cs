using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHealthBuffSkill : Skill
{
    public TakeHealthBuffSkill(SkillData skillData) : base(skillData)
    {
    }
    public override void Effect()
    {
        Buff buff = BuffManager.instance.GetBuff("毒药");
        buff.ReturnBuffDataInfo((info) => { });
        GameObject player = GameObject.FindAnyObjectByType<Player>().gameObject;
        buff.Apply(player);
        player.GetComponent<BuffHandle>().AddBuff(buff);
    }
}
