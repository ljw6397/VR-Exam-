using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public enum TTSType
{
    chain,
    table,
    calendar,
    keypad,
    hammer,
    escapekey
}

public class TTSEvent : MonoBehaviour
{
    [SerializeField] LocomotionMediator mediator;
    [SerializeField] float detectDistance = 1f;
    [SerializeField] TTSType currentTTSType;

    static bool isTable;
    static bool isCalendar;
    static bool isKeypad;

    bool isPlayOneTime;

    private void Update()
    {
        if (mediator == null) return;


        var origin = mediator.xrOrigin;

        if (origin == null) return;

        Vector3 playerPos = origin.Camera.transform.position;

        float distance = Vector3.Distance(transform.position, playerPos);

        if (distance < detectDistance)
        {
            if (isPlayOneTime) return;    //한번 재생 했으면 반환 처리

            //특정 이벤트 재생
            switch (currentTTSType)
            {
                case TTSType.table:

                    isTable = true;
                    TTSManager.instance.PlaySound("책상");
                    isPlayOneTime = true;
                    break;
                case TTSType.calendar:
                    if (!isTable) return;

                    isCalendar = true;
                    TTSManager.instance.PlaySound("달력");
                    isPlayOneTime = true;
                    break;
                case TTSType.keypad:
                    if (!isCalendar) return;

                    isKeypad = true;
                    TTSManager.instance.PlaySound("달력키패드");
                    isPlayOneTime = true;
                    break;
            }
        }
    }

    public void PlayTTSChain()
    {
        if (isPlayOneTime) return;    //한번 재생 했으면 반환 처리

        TTSManager.instance.PlaySound("구속");
        isPlayOneTime = true;
    }

    public void PlayTTSHammer()
    {
        if (isPlayOneTime) return;    //한번 재생 했으면 반환 처리

        TTSManager.instance.PlaySound("망치");
        isPlayOneTime = true;
    }
    public void PlayTTSDoorKey()
    {
        if (isPlayOneTime) return;    //한번 재생 했으면 반환 처리

        TTSManager.instance.PlaySound("철창열쇠");
        isPlayOneTime = true;
    }
}
