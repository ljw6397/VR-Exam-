using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class XRButtonPress : MonoBehaviour
{
    public XRSimpleInteractable interactable; // 눌릴 트리거
    public Transform buttonVisual;            // 실제로 움직이는 버튼 부분
    public float pressDepth = 0.02f;          // 눌릴 거리
    public float pressSpeed = 10f;            // 움직이는 속도

    private Vector3 originalPos;
    private Vector3 pressedPos;
    private bool isPressed = false;
    private bool isAnimating = false;

    void Start()
    {
        if (buttonVisual == null)
            buttonVisual = transform;

        originalPos = buttonVisual.localPosition;
        pressedPos = originalPos - new Vector3(0, pressDepth, 0);

        // XR Interaction 이벤트 연결
        interactable.selectEntered.AddListener(OnPressed);
        interactable.selectExited.AddListener(OnReleased);
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        isPressed = true;
        StopAllCoroutines();
        StartCoroutine(MoveButton(pressedPos));
    }

    void OnReleased(SelectExitEventArgs args)
    {
        isPressed = false;
        StopAllCoroutines();
        StartCoroutine(MoveButton(originalPos));
    }

    System.Collections.IEnumerator MoveButton(Vector3 target)
    {
        isAnimating = true;
        while (Vector3.Distance(buttonVisual.localPosition, target) > 0.001f)
        {
            buttonVisual.localPosition = Vector3.Lerp(
                buttonVisual.localPosition,
                target,
                Time.deltaTime * pressSpeed
            );
            yield return null;
        }
        buttonVisual.localPosition = target;
        isAnimating = false;
    }
}
