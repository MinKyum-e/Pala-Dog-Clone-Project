using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����, ������, ĳ���ýð�, ��Ÿ�� �˰��ִٰ� ġ��
public class SkillManager:MonoBehaviour
{
    private static SkillManager _instance;
    public static SkillManager Instance { get { return _instance; } }
    private PoolManager skillPool;
    private PoolManager enemyPool;
    private PoolManager minionPool;

    private void Awake()
    {
        _instance = this;
        skillPool = GameObject.FindGameObjectWithTag("SkillPool").GetComponent<PoolManager>();
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<PoolManager>();
        minionPool = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
    }

    //index�� ��ų ������������ ���� actor damage ������ �ʿ��� ��츦 ����Ͽ� actor�� ������ ��ų �����ġ�� target��ġ�� ����
    public IEnumerator FireWall(int index, Actor actor, GameObject target)
    {
        SkillInfo s = Parser.skill_info_dict[index];
        if (target.activeSelf == true)
        {

            GameObject skill_clone = skillPool.Get(index);
        skill_clone.transform.position = target.transform.position;

        //ĳ���� ����
        actor.can_attack = false;
        actor.isWalk = false;
        yield return new WaitForSeconds(s.casting_time);
        actor.can_attack = true;
        actor.isWalk = true;
            //������ �ֱ�
        BoxCollider2D clone_collider = skill_clone.GetComponent<BoxCollider2D>();
            clone_collider.enabled = true;
        yield return new WaitForSeconds(0.1f);
            clone_collider.enabled = false;
        skill_clone.SetActive(false);
        }
        yield return null;
    }


}
