using System.Collections;
using UnityEngine;

public abstract class Monster : Unit
{
    protected MonsterInfo monster_info;
    protected Unit atkTarget;

    public abstract Unit setAttackTarget(string main_target_tag, string target_tag);

    public float DistanceToTarget(Vector3 a,  Vector3 b)
    {
        return Mathf.Abs(a.x - b.x);
    }
    public IEnumerator NormalAttack(string main_target_tag, string target_tag)
    {
        while (true)
        {
            //Ÿ������
            atkTarget = setAttackTarget(main_target_tag, target_tag);

            //attack
            if (atkTarget != null)
            {
                isWalk = false;
                atkTarget.GetComponent<SpriteRenderer>().color = Color.red ;//���� �ִϸ��̼� ����
                atkTarget.Hit(monster_info.atk);
                yield return new WaitForSeconds(monster_info.atkSpeed);
                atkTarget.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                isWalk = true;
            }
            yield return null;
        }
    }
    
    public void SetMoveDir(string main_target_tag)
    {
        GameObject main_target = GameObject.FindGameObjectWithTag(main_target_tag);
        try
        {
            moveDir = new Vector3((main_target.transform.position.x - transform.position.x), 0, 0).normalized;
        }
        catch
        {

            print("SetAttackTarget: maintarget missing");
            
        }
    }


}
