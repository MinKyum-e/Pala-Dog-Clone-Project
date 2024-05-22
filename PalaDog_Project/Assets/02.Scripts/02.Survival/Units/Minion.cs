using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Minion: MonoBehaviour
{
    public Actor actor;
    public Actions action;
    PoolManager poolManager;
    GameObject enemyBase;
    public int cost;
    
    

    private void Awake()
    {
        actor = GetComponent<Actor>();
        poolManager = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<PoolManager>();
        enemyBase = GameObject.Find("EnemyBase");
        action = GetComponent<Actions>();
        
        
    }



    private void OnEnable()
    {
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

        actor.isWalk = true;
        actor.is_faint = false;
        actor.hit_time = false;
        actor.can_search = true;
        actor.can_attack = true;
        actor.isDie = false;
        actor.can_action = true;

/*        StartCoroutine(NormalAttack());*/
        
        //GameManager.Instance.UpdateCost(info.cost); //cost �߰�
    }


    private void Update()
    {
        /* if (actor.cur_status.HP <= 0)
         {
             Die();
         }
         else
         {
 */
        /*   if (actor.can_use_skill) 
           {
               //�Ϲ� ������ ��ų 1��


           }*/

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



        if (actor.atkTarget == null)
        {
            
            actor.isWalk = true;
        }
        if (actor.cur_status.HP <= 0)
        {
            actor.isDie = true;
            actor.can_action = false;
            actor.isWalk = false;
            actor.can_search = false;
            actor.atkTarget = null;
            actor.animator.Play("Die");
        }

        if (actor.is_faint)
        {
            actor.can_action = true;
        }

        if (actor.can_action)
        {
            if (actor.can_search)
            {
                actor.atkTarget = setAttackTarget(actor.cur_status.atkRange);
                actor.skillTarget = setAttackTarget(actor.skill_info.cast_range);
            }
            
            if (actor.can_use_skill && actor.atkTarget != null)
            {
                if(actor.skillTarget.activeSelf && actor.skillTarget.GetComponent<Actor>().isDie == false)
                {
                    actor.can_action = false;
                    actor.animator.SetTrigger("Skill");

/*                    if (actor.skillTarget != null)
                    {
                        print("��ų���!!!!!!!" + actor.cur_status.skill[0]);
                        action.PlaySkill(actor.cur_status.skill[0], actor.skillTarget);
                        
                    }*/
                }
            }
            else if (actor.can_attack && actor.atkTarget != null)
            {
                if (actor.atkTarget.activeSelf && actor.atkTarget.GetComponent<Actor>().isDie == false)
                {
                    actor.can_action = false;
                    actor.animator.SetTrigger("Attack");
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (actor.isWalk)
        {
            /*if (actor.animator != null)
                actor.animator.SetBool("isWalk", actor.isWalk); */
            action.Move();
        }

    }


    public void setStatus()
    {
        try
        {
            actor.status = Parser.minion_status_dict[actor.ID].common;
            actor.cur_status = Parser.minion_status_dict[actor.ID].common;
            cost = Parser.minion_status_dict[actor.ID].cost;
        }
        catch { Debug.Log("status Setting Error Minion"); }
    }

    public void Die()
    {
        actor.spriteRenderer.color = Color.white;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
        GameManager.Instance.UpdateCost(-cost);
    }


    public GameObject setAttackTarget(float range)
    {
        //���� Ÿ���� �����ϸ� �׳� return
        if ( actor.atkTarget != null && actor.atkTarget.gameObject.activeSelf && Utils.DistanceToTarget(actor.atkTarget.transform.position, transform.position) <= range)
        {
            return actor.atkTarget;
        }

        GameObject target = null;

        float dist;
        try
        {
            dist = Utils.DistanceToTarget(enemyBase.transform.position, transform.position);
            if (dist <= range && ! enemyBase.GetComponent<Actor>().isDie)
                target = enemyBase;
        }
        catch
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            dist = 9999999;
        }

        foreach (List<GameObject> units in poolManager.pools)
        {
            foreach (GameObject u in units)
            {
                if (!u.activeSelf || u.GetComponent<Actor>().isDie) { continue; }
                float tmp_dist = Utils.DistanceToTarget(u.transform.position, transform.position);
                if (tmp_dist < dist && tmp_dist <= range)
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

  /*  public IEnumerator NormalAttack()
    {
        while (true)
        {
            //attack
            atkTarget = setAttackTarget(actor.cur_status.atkRange);
            if (atkTarget != null && actor.can_attack && atkTarget.activeSelf)
            {

                    actor.isWalk = false;
                    Color c = atkTarget.GetComponent<SpriteRenderer>().color;
                    atkTarget.GetComponent<SpriteRenderer>().color = Color.red;//���� �ִϸ��̼� ����
                                                                               //���ݼӼ����� Ȯ��
                    if (Buff.CheckAttackIgnore(atkTarget.GetComponent<Actor>().cur_buff, actor.cur_status.job))
                        atkTarget.GetComponent<Actions>().Hit(actor.cur_status.atk);
                    yield return new WaitForSeconds(actor.cur_status.atkSpeed);
                    atkTarget.GetComponent<SpriteRenderer>().color = c;

            }
            else if(actor.can_attack)
            {
                actor.isWalk = true;
            }
            yield return null;
        }
    }*/
}
