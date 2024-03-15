using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    GAME_OVER, 
    GAME_PLAY, 
    GAME_PAUSE,
    GAME_STAGE_CLEAR
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public Player_fix player;
    public PoolManager enemy_pool;
    public PoolManager minion_pool;
    public Parser parser;
    public UIManager uiManager;


    public GameState state;
    

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
        }
        else
        {//�� �̵��ƴµ� ���� �Ŵ����� ������ ��� �ڽ��� ����
            Destroy(this.gameObject );
        }
    }
    private void Start()
    {
        InitGame();
    }
    private void Update()
    {
        //���� ����
        switch(state)
        {
            case GameState.GAME_PLAY:

                break;
            case GameState.GAME_OVER:
                GameOver();

                break;
            case GameState.GAME_PAUSE:
                PauseGame();
                break;
            case GameState.GAME_STAGE_CLEAR:
                StageClear();

                break;

            default: 
                break;
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
        Time.timeScale = 1.0f;
        player.transform.position = Vector3.zero;
        state = GameState.GAME_PLAY;
        uiManager.SetCurrentPage(UIPageInfo.GamePlay);
        
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ContinueGame()
    {

    }
    public void RestartGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver()
    {
        uiManager.SetCurrentPage(UIPageInfo.GameOver);
        Time.timeScale = 0;
        
    }

    public void StageClear()
    {
        uiManager.SetCurrentPage(UIPageInfo.GameStageClear);
        Time.timeScale = 0;
    }
}
