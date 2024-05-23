using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public Actor actor;
    public Actions action;
    public UnitType atkType;

    private void OnEnable()
    {
        actor = GetComponent<Actor>();
        action = GetComponent<Actions>();

        atkType = (gameObject.tag == "Enemy") ? UnitType.Minion : UnitType.Enemy;
        setStatus();
        if (Parser.skill_info_dict.TryGetValue(actor.cur_status.skill[0], out actor.skill_info) == false)
        {
            actor.can_use_skill = false;
        }
        else
        {
            actor.can_use_skill = true;
        }
        actor.cur_status.HP = actor.status.HP;
        actor.atkTarget = null;

        actor.is_faint = false;
        actor.isDie = false;
        actor.can_action = true;

        /*        StartCoroutine(NormalAttack());*/

        //GameManager.Instance.UpdateCost(info.cost); //cost �߰�
    }


    private void Update()
    {

        if (actor.cur_status.HP <= 0)
        {
            actor.isDie = true;
            actor.can_action = false;
            actor.atkTarget = null;
            actor.animator.Play("Die");
        }

        AnimatorStateInfo stateInfo = actor.animator.GetCurrentAnimatorStateInfo(0);

        // ���� �ִϸ����� ���°� Ÿ�� �ִϸ��̼� ���¿� ��ġ�ϴ��� Ȯ���մϴ�.
        if (stateInfo.IsName("Attack"))
        {
            // Ÿ�� �ִϸ��̼� ������ �ӵ��� �����մϴ�.
            actor.animator.speed = actor.cur_status.atkSpeed;
        }
        else
        {
            // Ÿ�� �ִϸ��̼� ���°� �ƴ� �� �⺻ �ӵ��� �ǵ����ϴ�.
            actor.animator.speed = 1.0f;
        }


        //�ʿ��������
        if (actor.is_faint)
        {
            //����
        }
        else
        {


        }


        if (actor.can_action)
        {
            if (actor.can_use_skill)
            {
                //Ÿ�� ���� �ʿ�?
                if (actor.skill_info.target_check)
                {
                    //��ų ���� Ÿ�� ���� ��� ä��
                    actor.skillTarget = setAttackTarget(actor.skillTarget, actor.skill_info.cast_range, atkType);
                    if (actor.can_use_skill && actor.skillTarget != null && actor.skillTarget.GetComponent<Actor>().isDie == false)
                    {
                        actor.can_action = false;
                        actor.can_use_skill = false;
                        actor.animator.SetTrigger("Skill");
                    }
                }
                else
                {
                    actor.can_action = false;
                    actor.animator.SetTrigger("Skill");
                }
            }
            else
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
