
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// WaveTable 
///public int Wave_Group;                 //���� ���̺� �� ���� �׷�ȭ�� ���� �׷� �ε���
///public int Wave_StageNum;              //���̺갡 �����ϴ� �������� ��ȣ
///public int Wave_WaveNum;               //�������� ���� ���̺� ���� ��ȣ
///public WaveType Wave_WaveType;
///public float Wave_SpawnTime;           //���̺��� ������ �ֱ�
///public int Wave_MonsterIndex;          //���̺꿡 ������ ������ ���� �ε���
///public int Wave_MonsterNum;            //���̺꿡 ������ ���� ���� �� ��ü ��*/
/// 
/// </summary>

public class WaveMonster
{
    public int monster_index;
    public int monster_num;
    public float monster_spawnTime;
    public float last_spawnTime;

    public WaveMonster(int wave_MonsterIndex, int wave_MonsterNum, float wave_SpawnTime)
    {
        this.monster_index = wave_MonsterIndex;
        this.monster_num = wave_MonsterNum;
        this.monster_spawnTime = wave_SpawnTime;
        this.last_spawnTime = -99999999;
    }
};

public class WaveManager : MonoBehaviour
{
    private static WaveManager instance; 
    public int cur_stageNum;
    public int cur_waveNum;
    public List<WaveMonster> monster_list;
    
    public List<Coroutine> coroutine_list ;
    private PoolManager monsterPool;
    public WaveType wave_type = WaveType.Normal;
    int sort_num;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
        cur_stageNum = 0;
        cur_waveNum = 0;
        monster_list = new List<WaveMonster>();
        coroutine_list = new List<Coroutine>();
        monsterPool = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<PoolManager>();

    }
    public static WaveManager Instance
    {
        get{
            if (instance == null)
                return null;
            return instance;
        }
    }



    private void Update()
    {
        if(GameManager.Instance.stage != cur_stageNum || GameManager.Instance.wave != cur_waveNum) //monsterlist ����
        {
            //����Ʈ �ʱ�ȭ
            monster_list.Clear();
            cur_stageNum = GameManager.Instance.stage;
            cur_waveNum = GameManager.Instance.wave;
            foreach(Coroutine c in coroutine_list) { StopCoroutine(c); }

            //monsterlist ����
            foreach(KeyValuePair<int, WaveInfo>  waveTable in Parser.wave_info_dict)
            {
                if(waveTable.Value.Wave_StageNum == cur_stageNum && waveTable.Value.Wave_WaveNum == cur_waveNum)
                {
                    monster_list.Add(new WaveMonster(waveTable.Value.Wave_MonsterIndex, waveTable.Value.Wave_MonsterNum, waveTable.Value.Wave_SpawnTime));
                    wave_type = waveTable.Value.Wave_WaveType;

                }
            }
        }
        else 
        {
            //spawn time, last spawn time �� �� ��ȯ
            foreach(WaveMonster monster in monster_list)
            {
                if((Time.time - monster.last_spawnTime) > monster.monster_spawnTime )
                {
                    coroutine_list.Add(StartCoroutine(SpawnMonster(monster.monster_index, monster.monster_num)));
                    monster.last_spawnTime = Time.time;
                }
            }
        }
    }

    IEnumerator SpawnMonster(int idx, int num)
    {
        for(int i=0;i<num;i++)
        {
            GameObject clone = monsterPool.Get(idx, transform.position);
            Color c = clone.GetComponent<SpriteRenderer>().color;
            //clone.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g,  c.b, 1);
            clone.tag = "Enemy";
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }


    private void ClearMonsterList()
    {
        monster_list.Clear();
    }


    public void ClearMonsterObjectOnStage()
    {
        GameObject[] alive_enemys = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] alive_minions = GameObject.FindGameObjectsWithTag("Minion");
        foreach(GameObject e in alive_enemys)
        {
            if(e.activeSelf)
             e.GetComponent<Enemy>().Die();
        }
        foreach (GameObject m in alive_minions)
        {
            if (m.activeSelf)
                m.GetComponent<Minion>().Die();
        }
    }
}
