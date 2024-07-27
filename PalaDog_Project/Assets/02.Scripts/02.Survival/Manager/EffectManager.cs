using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_Instance;
    public ParticleSystem spawnCloud; 
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) return null;
            else
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (null == m_Instance) //���� ����ó�������Ҷ���
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {//�� �̵��ƴµ� ���� �Ŵ����� ������ ��� �ڽ��� ����
            Destroy(this.gameObject);
        }
    }
}