using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class XRButtonPress : MonoBehaviour
{
    public XRSimpleInteractable interactable; 
    public Transform buttonVisual;            
    public float pressDepth = 0.02f;          
    public float pressSpeed = 10f;            

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
