using UnityEngine;

public class TouchAnywhere : MonoBehaviour
{
    public GameObject obj;
    void Update()
    {
        // ����� ��ġ �Է� ó��
        if (Input.touchCount > 0)
        {
            // ù ��° ��ġ�� ���� ���� ��������
            Touch touch = Input.GetTouch(0);

            // ��ġ ���� �� �ߵ� (��ġ�ϴ� ����)
            if (touch.phase == TouchPhase.Began)
            {
                OnTouch();
            }
        }

        // PC������ ���콺 Ŭ�� �Է� ó��
        if (Input.GetMouseButtonDown(0))
        {
            OnTouch();
        }
    }

    // ��ġ Ȥ�� Ŭ�� �� �ߵ��� �̺�Ʈ
    void OnTouch()
    {
        obj.SetActive(true);
    }
}