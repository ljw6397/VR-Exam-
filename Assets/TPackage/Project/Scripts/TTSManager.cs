using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class TTS
{
    public string name;
    public AudioClip clip;

    //[Range(0f, 1f)]                      //인스펙터에서 범위 설정
    //public float volume = 1.0f;          //사운드 볼륨

    //[Range(0.1f, 3f)]
    //public float pitch = 1.0f;           //사운드 피치
    public bool loop;
    public bool isSkip;                    //재생 중인 TTS가 있을 때 스킵 여부

    [HideInInspector] 
    public AudioSource source;
}

public class TTSManager : MonoBehaviour
{
    public static TTSManager instance;

    public List<TTS> sounds = new List<TTS>();

    private Queue<AudioSource> playList = new Queue<AudioSource>();
    private AudioSource currentTTS;

    private bool isChangingTTS = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (TTS sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = 1;
            sound.source.pitch = 1;
            sound.source.loop = sound.loop;
            //sound.source.outputAudioMixerGroup = sound.mixerGroup;  //오디오 믹서 그룹 설정
        }
    }

    private void Start()
    {
        //테스트용 
        PlaySound("시작");
        PlaySound("움직임제한");
        //PlaySound("구속");
    }

    private void Update()
    {
        if(playList.Count > 0)
        {
            if (playList.TryPeek(out var result))  //현재 재생 중인 TTS가 재생 중일 경우 리턴
            {
                if(isChangingTTS)
                {
                    result.Play();
                    currentTTS = result;
                    isChangingTTS = false;

                    Debug.Log("교체한 TTS: " + currentTTS.clip.name);
                }

                if (!result.isPlaying)
                {
                    //해당 TTS 재생 종료
                    playList.Dequeue();
                    isChangingTTS = true;
                }
            }
        }
    }

    // 사운드를 재생하는 매서드
    public void PlaySound(string name)
    {
        TTS soundToPlay = sounds.Find(sound => sound.name == name);

        if (soundToPlay != null)
        {
            if (soundToPlay.isSkip)
            {
                currentTTS.Stop();      //현재 재생 중이던 TTS 종료
                playList.Clear();       //이전의 TTS 대기열 초기화
                playList.Enqueue(soundToPlay.source);
            }
            else
            {
                playList.Enqueue(soundToPlay.source);
            }
        }
        else
        {
            Debug.LogWarning("사운드 : " + name + " 없습니다.");
        }
    }
}
