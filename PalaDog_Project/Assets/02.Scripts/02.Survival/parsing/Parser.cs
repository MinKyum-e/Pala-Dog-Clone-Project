using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    private static Parser instance = null;
    public static List<Dictionary<string, object>> data_MinionTable = null;
    public static List<Dictionary<string, object>> data_EnemyTable = null;
    public static List<Dictionary<string, object>> data_WaveTable = null;

    public static Dictionary<int, EnemyStatus> enemy_status_dict = null;
    public static Dictionary<int, WaveInfo> wave_info_dict = null;
    public static Dictionary<int, MinionStatus> minion_status_dict = null;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            data_MinionTable = CSVReader.Read("DT_ChrTable", 20);
            data_EnemyTable = CSVReader.Read("DT_MonsterTable", 19);
            data_WaveTable = CSVReader.Read("DT_WaveTable", 10);
            
            enemy_status_dict = new Dictionary<int, EnemyStatus>();
            wave_info_dict = new Dictionary<int, WaveInfo>();
            minion_status_dict = new Dictionary<int, MinionStatus>();

            //���� ���� ��������ִ� class �����
            foreach (var d in data_EnemyTable)
            {
                int idx = (int)d["Monster_Index"];
                EnemyStatus e = new EnemyStatus();
                e.common.skill = new int[3];
                e.common.name = d["Monster_GameName"].ToString();
                //enemy_info_dict[idx].grade = (int)d["Monster_Grade"];
                e.gold = (int)d["Monster_Gold"];
                e.common.HP = (int)d["Monster_HP"];
                e.common.atk = (int)d["Monster_Atk"];
                e.common.atkSpeed = float.Parse(d["Monster_AtkSpeed"].ToString());
                e.common.atkRange = float.Parse(d["Monster_AtkRange"].ToString());
                e.common.moveSpeed = float.Parse(d["Monster_MoveSpeed"].ToString());
                e.common.skill[0] = (int)d["Monster_Skill1"];
                e.common.skill[1] = (int)d["Monster_Skill2"];
                e.common.skill[2] = (int)d["Monster_Skill3"];
                e.common.moveDir = Vector2.left;

                enemy_status_dict[idx] = e;
            }

            foreach (var d in data_MinionTable)
            {
                
                int idx = (int)d["Chr_Index"];
                MinionStatus s = new MinionStatus();
                s.common.skill = new int[3];
                s.common.skill[0] = (int)d["Chr_Skill"];
                s.common.name = d["Chr_GameName"].ToString();
                //enemy_info_dict[idx].grade = (int)d["Monster_Grade"];
                s.cost = (int)d["Chr_Cost"];
                s.common.HP = (int)d["Chr_HP"];
                s.common.atk = (int)d["Chr_Atk"];
                s.common.atkSpeed = float.Parse(d["Chr_AtkSpeed"].ToString());
                s.common.atkRange = float.Parse(d["Chr_AtkRange"].ToString());
                //jobȮ��
                if (d["Chr_Job"].ToString() == "melee")
                    s.common.job = Chr_job.melee;
                else if (d["Chr_Job"].ToString() == "projectile")
                    s.common.job = Chr_job.projectile;
                else if (d["Chr_Job"].ToString() == "magic")
                    s.common.job = Chr_job.magic;

                s.common.moveSpeed = float.Parse(d["Chr_MoveSpeed"].ToString());
               s.common.moveDir = Vector2.right;
                minion_status_dict[idx] = s;
            }
            //���̺� ����
            foreach (var wave in data_WaveTable)
            {
                int idx = (int)wave["Wave_Index"];
                wave_info_dict[idx] = new WaveInfo();
                wave_info_dict[idx].Wave_Index = (int)wave["Wave_Index"];
                wave_info_dict[idx].Wave_Name = wave["Wave_Name"].ToString();
                wave_info_dict[idx].Wave_DevName = wave["Wave_DevName"].ToString();
                wave_info_dict[idx].Wave_Group = (int)wave["Wave_Group"];
                wave_info_dict[idx].Wave_StageNum = (int)wave["Wave_StageNum"];
                wave_info_dict[idx].Wave_WaveNum = (int)wave["Wave_WaveNum"];

                if (wave["Wave_WaveType"].ToString() == "Normal")
                    wave_info_dict[idx].Wave_WaveType = WaveType.Normal;
                else
                    wave_info_dict[idx].Wave_WaveType = WaveType.Boss;

                wave_info_dict[idx].Wave_SpawnTime = float.Parse(wave["Wave_SpawnTime"].ToString());
                wave_info_dict[idx].Wave_MonsterIndex = (int)wave["Wave_MonsterIndex"];
                wave_info_dict[idx].Wave_MonsterNum = (int)wave["Wave_MonsterNum"];
            }


            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public static Parser Instance //���ӸŴ��� �ν��Ͻ� ����
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
}
