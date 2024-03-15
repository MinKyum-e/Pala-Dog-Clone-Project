using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public Player player;
    public PoolManager pool;
    public Parser parser;

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);//�� ��ȯ�ǵ� ���� �ȵ�
        }
        else
        {//�� �̵��ƴµ� ���� �Ŵ����� ������ ��� �ڽ��� ����
            Destroy(this.gameObject );
        }
    }

    public static GameManager Instance //���ӸŴ��� �ν��Ͻ� ����
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void InitGame()
    {

    }
    public void PauseGame()
    {

    }
    public void ContinueGame()
    {

    }
    public void RestartGame()
    {

    }
    public void StopGame()
    {
      
    }
}
