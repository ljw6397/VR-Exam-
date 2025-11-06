using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(XROrigin))]
public class XRCharacterHeightSynce : MonoBehaviour
{
    private XROrigin xrOrigin;
    private CharacterController characterController;

    void Awake()
    {
        xrOrigin = GetComponent<XROrigin>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (xrOrigin.Camera == null) return;

        // HMD 기준으로 높이 계산
        float headHeight = Mathf.Clamp(xrOrigin.CameraInOriginSpaceHeight, 1f, 2f);

        characterController.height = headHeight;
        characterController.center = new Vector3(0, headHeight / 2f, 0);
    }
}
