using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealthBuffSkill : Skill
{
    public AddHealthBuffSkill(SkillData skillData) : base(skillData)
    {
    }
    public override void Apply()
    {
        if (coolDownTime!=0) return;
        coolDownTime = SkillData.CoolDownTime;



        Buff buff = BuffManager.instance.GetBuff("血包");
        if (buff != null)
        {
        }
        buff.ReturnBuffDataInfo((info) =>
        {
        });
        GameObject player=GameObject.FindAnyObjectByType<Player>().gameObject;
        buff.Apply(player);
        player.GetComponent<BuffHandle>().AddBuff(buff);
    }
}
