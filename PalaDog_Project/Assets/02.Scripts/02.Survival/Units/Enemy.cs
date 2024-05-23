using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy: Unit
{
/*    Actor actor;
  *//*  Actions action;*//*
    PoolManager poolManager;*/
    GameObject player;
    public int grade;
    public int gold;

    private void Awake()
    {
/*        actor = GetComponent<Actor>();
        poolManager = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();*/
        player = GameObject.FindGameObjectWithTag("Player");
/*        action = GetComponent<Actions>();*/
    }
/*    private void OnEnable()
    {
        setStatus();
        actor.cur_status.HP = actor.status.HP;
        actor.atkTarget = null;

        actor.is_faint = false;
        actor.can_use_skill = false;
        actor.isDie = false;
        actor.can_action = true;

    }*/
/*    private void Update()
    {

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
        }*/

        /*        if (actor.atkTarget == null)
                {
                    actor.isWalk = true;
                }*//*
                if (actor.cur_status.HP <= 0)
                {
                    actor.isDie = true;
                    actor.can_action = false;
                    actor.atkTarget = null;
                    actor.animator.Play("Die");
                }

        *//*        if(actor.is_faint)
                {
                }
                else
                {

                }*//*

                if(actor.can_action)
                {
                    *//*            if (actor.can_search)
                                {

                                }

                                if (actor.can_use_skill && actor.atkTarget != null)
                                {
                                    if (actor.skillTarget.activeSelf && actor.skillTarget.GetComponent<Actor>().isDie == false)
                                    {
                                        actor.can_action = false;
                                        actor.animator.SetTrigger("Skill");
                                    }
                                }*//*
                    actor.atkTarget = setAttackTarget();
                    if (actor.atkTarget != null &&actor.atkTarget.activeSelf && actor.atkTarget.GetComponent<Actor>().isDie == false)
                    {
                        actor.can_action = false;
                        actor.animator.SetTrigger("Attack");
                    }
                }
            }
            void FixedUpdate()
            {
                //�ȱ�
                if (actor.can_action)
                {

                    action.Move();action.SetMoveDir("Player");

                }
            }*/

        public override void setStatus()
    {
        try
        {
            actor.status = Parser.enemy_status_dict[actor.ID].common;
            actor.cur_status = Parser.enemy_status_dict[actor.ID].common;
            gold = Parser.enemy_status_dict[actor.ID].gold;
        }
        catch { Debug.Log("status Setting Error"); }
    }



    public override GameObject setAttackTarget(GameObject cur_target, float range, UnitType unitType)
    {
       
        GameObject target = null;
        //���ΰ� ������ �� ������ ����
        float dist;
        try
        {
            dist = Utils.DistanceToTarget(player.transform.position, transform.position);
            if (dist <= actor.cur_status.atkRange)
            {
                return player;
            }
                
        }
        catch
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            dist = 9999999;
        }

        if (actor.atkTarget != null && Utils.DistanceToTarget(actor.atkTarget.transform.position, transform.position) <= actor.cur_status.atkRange)
        {
            return actor.atkTarget;
        }

        PoolManager targetPool = ((unitType == UnitType.Enemy) ? actor.enemy_poolManager : actor.minion_poolManager);
        foreach (List<GameObject> units in targetPool.pools)
        {
            foreach (GameObject u in units)
            {
                if (!u.activeSelf || u.GetComponent<Actor>().isDie) { continue; }
                float tmp_dist = Utils.DistanceToTarget(u.transform.position, transform.position);
                if (tmp_dist < dist && tmp_dist <= actor.cur_status.atkRange)
                {
                    dist = tmp_dist;
                    target = u;
                }
            }
        }

        if (target != null)
        {
            actor.atkTarget = target;
            return actor.atkTarget;
        }
        else { return null; }
    }
    public override void Die()
    {
        actor.spriteRenderer.color = Color.white;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
        GameManager.Instance.UpdateGold(gold);
    }

}
