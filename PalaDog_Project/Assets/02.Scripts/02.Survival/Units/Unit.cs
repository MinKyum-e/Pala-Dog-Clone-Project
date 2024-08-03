using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public Actor actor;
    public Actions action;
    public Skill skill;
    public UnitType atkType;

    protected virtual void OnEnable()
    {
        actor = GetComponent<Actor>();
        action = GetComponent<Actions>();
        atkType = (gameObject.tag == "Enemy") ? UnitType.Minion : UnitType.Enemy;
        setStatus();

        actor.cur_status.HP = actor.status.HP;
        actor.atkTarget = null;
        actor.isDie = false;
        actor.can_action = true;
        skill = GetComponent<Skill>();

        /*        StartCoroutine(NormalAttack());*/

        //GameManager.Instance.UpdateCost(info.cost); //cost �߰�
        for (int i = 0; i < 3; i++)
        {
            SkillEntry entry;
            if (Parser.skill_table_dict.TryGetValue(actor.cur_status.skill[i], out entry))
            {
                actor.skills[i].entry = entry;
                actor.skills[i].can_use_skill = true;
            }
            else
            {
                actor.skills[i].can_use_skill = false;
            }

        }
        
    }


    private void Update()
    {

        if (actor.cur_status.HP <= 1.0f)
        {
            actor.isDie = true;
            actor.can_action = false;
            actor.atkTarget = null;

            actor.animator.speed = 1.0f;
            actor.animator.Play("Die");
            actor.transform.Find("Buff").GetComponent<BuffSystem>().SlotFree();
        }

        AnimatorStateInfo stateInfo = actor.animator.GetCurrentAnimatorStateInfo(0);

        // ���� �ִϸ����� ���°� Ÿ�� �ִϸ��̼� ���¿� ��ġ�ϴ��� Ȯ���մϴ�.
        if (stateInfo.IsName("Attack") || stateInfo.IsName("Skill0") || stateInfo.IsName("Skill1") || stateInfo.IsName("Skill2"))
        {
            // Ÿ�� �ִϸ��̼� ������ �ӵ��� �����մϴ�.
            actor.animator.speed = actor.cur_status.atkSpeed;
        }
        else
        {
            // Ÿ�� �ִϸ��̼� ���°� �ƴ� �� �⺻ �ӵ��� �ǵ����ϴ�.
            actor.animator.speed = 1.0f;
        }


        //����
        if(actor.cur_buff.stun && actor.isDie == false)
        {
            actor.can_action = false;
            actor.can_walk = false;
            {
                actor.animator.SetTrigger("Stun");
            }
        }
        else
        {
            if (stateInfo.IsName("Stun"))
            {
                actor.animator.Play("Walk");
                actor.can_action = true;
                actor.can_walk = true;
            }
                
        }



        if (actor.can_action)
        {

            //��ų �켱 ����
            for (int i = 0; i < 3; i++)
            {
                if (actor.skills[i].entry.act == SkillAct.A)//�нú길 ����Ǿ���
                    continue;

                if (actor.can_action && actor.skills[i].can_use_skill)
                {
                    //Ÿ�� ���� ����?
                    if (actor.skills[i].entry.need_searching)
                    {
                        //��ų ���� Ÿ�� ���� ��� ����
                        Actor target = skill.FirstSearchingTarget((SkillName)actor.skills[i].entry.index);

                        if(target != null)
                        {
                            actor.skills[i].target =target;
                            if (actor.can_action && actor.skills[i].target != null && actor.skills[i].target.GetComponent<Actor>().isDie == false)
                            {
                                actor.can_action = false;
                                actor.animator.SetTrigger("Skill" + i);
                                print((SkillName)actor.skills[i].entry.index);
                            }
                        }
                        
                    }
                    else
                    {
                        actor.can_action = false;
                        actor.skills[i].can_use_skill = false;
                        actor.animator.SetTrigger("Skill" + i);
                        print((SkillName)actor.skills[i].entry.index);
                    }

                }
            }

            

            //�Ϲݰ���(����)
            if (actor.can_action)
            {

                    actor.atkTarget = setAttackTarget(actor.atkTarget, actor.cur_status.atkRange, atkType);
                    if (actor.atkTarget != null && actor.atkTarget.GetComponent<Actor>().isDie == false)
                    {
                        

                        actor.can_action = false;
                        actor.animator.SetTrigger("Attack");
 
                    }

            }
        }

        
    }

    void FixedUpdate()
    {
        if (actor.can_action)
        {
           
            action.Move();
            if(gameObject.tag == "Enemy")
            {
                action.Move(); action.SetMoveDir("Player");
            }
        }


    }

    public abstract void setStatus();
    public abstract void Die();
    public abstract GameObject setAttackTarget(GameObject cur_target, float range, UnitType unitType);

}
