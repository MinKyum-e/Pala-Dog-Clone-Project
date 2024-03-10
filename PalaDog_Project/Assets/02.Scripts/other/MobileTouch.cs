using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobileTouch : MonoBehaviour
{
    [Header("Debug Test")]
    [SerializeField]
    [Range(0, 50)]private TextMeshProUGUI textTouch;


    private void Update()
    {
        //OnSingleTouch();        
        OnMultiTouch();
        OnAndriodMenu();
        OnVibrate();
    }

    private void OnSingleTouch()
    {
        if (Input.touchCount > 0) 
        {
            //ù���� �հ��� ���� ���� ������
            Touch touch = Input.GetTouch(0);

      
            //��ġ�� ���°� ��ġ �����϶�
            if(touch.phase == TouchPhase.Began) 
            {
                textTouch.text = "Touch Begin";
            }

            //��ġ�� ���°� ��ġ �����϶�
            else if(touch.phase == TouchPhase.Ended)
            {
                textTouch.text = "Touch End";
            }

        }
    }

    private void OnMultiTouch()
    {
        textTouch.text = "";
        for(int i=0;i<Input.touchCount;i++) 
        {
            Touch touch = Input.GetTouch(i);    //i��° ��ġ�� ���� ����
            int index = touch.fingerId;         //i���� ��ġ�� ID ��
            Vector2 position = touch.position;  //i��° ��ġ�� ��ġ
            TouchPhase phase = touch.phase;     //i��° ��ġ�� ����

            if(phase == TouchPhase.Began) //��ġ�ϴ� ���� 1ȸ ȣ��
            { }
            else if(phase == TouchPhase.Moved) //��ġ �� �巡�� �� �� ���
            {
                
            }
            else if(phase == TouchPhase.Stationary) //��ġ���·� ������ ���� ��
            { }
            else if(phase == TouchPhase.Ended) //��ġ�� ������ �� 1ȸ
            { }
            else if(phase==TouchPhase.Canceled) //�ý��ۿ� ���� ��ġ�� ������ ��
            { }

            textTouch.text += "Index : " + index + ", Status : " + phase + ", Position(" + position + ")\n";
        }
    }

    private void OnAndriodMenu()
    {
        if(Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            textTouch.text = "Input Escape";
        }
        if (Input.GetKeyDown(KeyCode.Home))
            {
            textTouch.text = "Input Home";
        }

        if(Input.GetKeyDown(KeyCode.Menu)) {
            textTouch.text = "Input Menu";    
        }
    }

    private void OnVibrate()
    {
        if(Input.touchCount == 3)
        {
            Handheld.Vibrate();
        }
    }




}
