using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����, ������, ĳ���ýð�, ��Ÿ�� �˰��ִٰ� ġ��
public class SkillManager:MonoBehaviour
{
    private static SkillManager _instance;
    public static SkillManager Instance { get { return _instance; } }
    private PoolManager skillPool;

    private void Awake()
    {
        _instance = this;
        skillPool = GameObject.FindGameObjectWithTag("SkillPool").GetComponent<PoolManager>();
    }

    public bool UseSkill(SkillName index, Actor actor, GameObject target)//��ų �ε���, ������, Ÿ��
    {
        print(target.name);
        switch (index)
        {
            case SkillName.KnockBack:
                StartCoroutine(RangeSKill((int)index, actor, target));
                break;
            default:

                return false;
        }
        return true;
    }

    public void RangeTarget(int index, Actor actor, GameObject target)
    {
        SkillInfo s = Parser.skill_info_dict[index];

    }

    public IEnumerator RangeSKill(int index, Actor actor, GameObject target)
    {
        SkillInfo s = Parser.skill_info_dict[index];
        if (target.activeSelf == true)
        {
            GameObject skill_clone = skillPool.Get(index);
            skill_clone.GetComponent<Actor>().cur_status.atk = s.damange;
            skill_clone.transform.localScale = new Vector3(s.range, 5, 1);
            skill_clone.transform.position = new Vector3(actor.transform.position.x + 0.5f, actor.transform.position.y, 0);
            BoxCollider2D clone_collider = skill_clone.GetComponent<BoxCollider2D>();
            clone_collider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            clone_collider.enabled = false;
            skill_clone.transform.position = new Vector3(0, 100, 0);
            skill_clone.SetActive(false);
        }
    }



}
