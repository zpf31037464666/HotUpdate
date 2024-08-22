using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    void Start()
    {
        PlayerItemData playerItemData = GameManager.instance.currentPlyaerItemData;

        Addressables.LoadAssetAsync<GameObject>(playerItemData.PrefabPath).Completed+=(e) =>
        {
            if (e.Status ==AsyncOperationStatus.Succeeded)
            {
                GameObject clone=Instantiate(e.Result,spawnPos.position,Quaternion.identity);
                clone.GetComponent<Player>().Init(playerItemData);

                if (playerItemData.SkillName != null && playerItemData.SkillName.Length > 0)
                {
                    Debug.Log("玩家生成技能");

                    foreach (string skillName in playerItemData.SkillName)
                    {
                        if (skillName != null) // 确保技能名有效
                        {
                            Skill skill = SkillManager.instance.GetSkill(skillName);
                            skill.ReturnSkillDataInfo((info) => { });

                            // 将技能添加到玩家技能管理器中
                            GameObject.FindAnyObjectByType<PlayerSkill>().AddSkill(skill);
                        }
                    }
                }

                if (playerItemData.AbilityName != null && playerItemData.AbilityName.Length > 0)
                {
                    Debug.Log("玩家生成能力");

                    foreach (string ablityName in playerItemData.AbilityName)
                    {
                        if (ablityName != null) // 确保技能名有效
                        {
                            Debug.Log("能力名字"+ablityName);
                            Ability ability = AbilityManager.instance.GetAbility(ablityName);
                            // 将技能添加到玩家技能管理器中
                            GameObject.FindAnyObjectByType<PlayerAbility>().AddAbility(ability);
                        }
                    }
                }


            }
        };
    }


}
