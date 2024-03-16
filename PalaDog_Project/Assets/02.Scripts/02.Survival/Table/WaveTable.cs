using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WaveType                   //���̺��� ����/�Ϲ�
{
    Normal ,
    Boss
};
/// <summary>
/// ���̺� �ý��� ������ ���� ���̺� ���̺�
/// </summary>
public struct WaveTable
{
    public int Wave_Index;                 //���̺��� ���̺� �� ���� �ε���
    public string Wave_Name;               //���̺��� �̸�(�������� �� ���̺� ���� ���)
    public string Wave_DevName;            //���̺��� ���߸�(�������� �� ���̺� ���� ���)
    public int Wave_Group;                 //���� ���̺� �� ���� �׷�ȭ�� ���� �׷� �ε���
    public int Wave_StageNum;              //���̺갡 �����ϴ� �������� ��ȣ
    public int Wave_WaveNum;               //�������� ���� ���̺� ���� ��ȣ
    public WaveType Wave_WaveType;
    public float Wave_SpawnTime;           //���̺��� ������ �ֱ�
    public int Wave_MonsterIndex;          //���̺꿡 ������ ������ ���� �ε���
    public int Wave_MonsterNum;            //���̺꿡 ������ ���� ���� �� ��ü ��
    
}
