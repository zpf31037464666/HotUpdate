using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    buff,
    Skill
}

public class WorldItem : MonoBehaviour
{
    public string Name;
    public ItemType Type;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch(Type)
            {
                case ItemType.buff:
                    Buff buff = BuffManager.instance.GetBuff(Name);
                    buff.ReturnBuffDataInfo((info) => {     });
                    buff.Apply(collision.gameObject);
                    collision.GetComponent<BuffHandle>().AddBuff(buff);
                    break;
                case ItemType.Skill:
                    Debug.Log("增加技能");
                    Skill skill = SkillManager.instance.GetSkill(Name);
                    skill.ReturnSkillDataInfo((info) => { });
                    collision.GetComponent<PlayerSkill>().AddSkill(skill);

                    break;
            }
            Destroy(gameObject);
        }
    }
}
