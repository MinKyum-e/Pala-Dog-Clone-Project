using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BtnEvent_ActiveSkill : MonoBehaviour
{
    Animator auraSkillAnimator;

    public SkillName skillName;
    public MinionUnitIndex minionUnitIndex;
    private void Start()
    {
        auraSkillAnimator = Player.Instance.aura.GetComponent<Animator>();    
    }

    public void PlayAuraSKill()
    {
        auraSkillAnimator.Play(skillName.ToString());
        Player.Instance.aura_skill.ChangeAuraSkill(skillName);
    }

    public void PlayUnitActiveSkill()
    {
        //����� ���ְ� ���� �ʿ�
        //��Ƽ�� ���� ã��


        Minion minion;
        //���� �ȵȰ�� ����ó��
        if (GameManager.Instance.hero_objects.TryGetValue(minionUnitIndex, out minion ))
        {
            Actor actor = minion.actor;
            for (int i = 0; i < actor.skills.Length; i++)
            {
                if (actor.skills[i].entry.index == (int)skillName && actor.skills[i].can_use_skill)
                {
                    actor.can_action = false;
                    actor.animator.SetTrigger("Skill" + i);
                    return;
                }
            }
        }
        print("���� ���� �������� ���� , " + minionUnitIndex.ToString());

    }
}
