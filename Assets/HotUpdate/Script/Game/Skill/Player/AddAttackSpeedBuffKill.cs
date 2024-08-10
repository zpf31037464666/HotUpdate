using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAttackSpeedBuffKill : Skill
{
    public AddAttackSpeedBuffKill(SkillData skillData) : base(skillData)
    {
    }
    public override void Effect()
    {
        base.Effect();
        //Buff buff = BuffManager.instance.GetBuff("活力覆盖");
        //buff.ReturnBuffDataInfo((info) => { });
        //GameObject player = GameObject.FindAnyObjectByType<Player>().gameObject;
        //buff.Apply(player);
        //player.GetComponent<BuffHandle>().AddBuff(buff);
    }
}
