using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteraction : MonoBehaviour
{
    [Header("Button Config")]
    public Transform buttonTop;
    public float pressDepth = 0.01f;
    public UnityEvent onPressed;

    private Vector3 initialPos;
    private bool isPressed = false;

    private void Start()
    {
        initialPos = buttonTop.localPosition;
    }

    private void Update()
    {
        float pushedDistance = initialPos.y - buttonTop.localPosition.y;

        // 콘솔에 현재 눌린 거리 출력
        Debug.Log("Button pushed distance: " + pushedDistance);

        if (!isPressed && pushedDistance >= pressDepth)
        {
            isPressed = true;
            Debug.Log("<color=green>✅ BUTTON PRESSED!</color>");
            onPressed?.Invoke();
        }

        // 버튼이 다시 올라갔을 때 재활성화
        if (pushedDistance < pressDepth * 0.3f)
        {
            isPressed = false;
        }
    }
}
