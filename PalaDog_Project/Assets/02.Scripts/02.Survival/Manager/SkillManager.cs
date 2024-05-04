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

    public bool UseSkill(SkillName index, Actor actor, GameObject target)
    {
        print(target.name);
        switch (index)
        {
            case SkillName.FireWall:
                StartCoroutine(FireWall((int)index, actor, target));
                break;
            default:

                return false;
        }
        return true;
    }
    //index�� ��ų ������������ ���� actor damage ������ �ʿ��� ��츦 ����Ͽ� actor�� ������ ��ų �����ġ�� target��ġ�� ����
    public IEnumerator FireWall(int index, Actor actor, GameObject target)
    {
        SkillInfo s = Parser.skill_info_dict[index];
        if (target.activeSelf == true)
        {

            GameObject skill_clone = skillPool.Get(index);
            skill_clone.transform.position = new Vector3(target.transform.position.x,0, 0);
            skill_clone.transform.localScale = new Vector3(s.range, 1, 1);
            

            //ĳ���� ����
            actor.can_attack = false;
            actor.isWalk = false;
            yield return new WaitForSeconds(s.casting_time);
            actor.can_attack = true;
            actor.isWalk = true;
            // �����
            skill_clone.GetComponent<Actor>().cur_status.atk = s.damange;

            //������ Ÿ�̹�
            BoxCollider2D clone_collider = skill_clone.GetComponent<BoxCollider2D>();
            clone_collider.enabled = true;
            skill_clone.GetComponent<SpriteRenderer>().color = Color.gray;
            yield return new WaitForSeconds(0.1f);
            skill_clone.GetComponent<SpriteRenderer>().color = Color.white;
            clone_collider.enabled = false;
            //����

            skill_clone.SetActive(false);
        }
        yield return null;
    }


}
