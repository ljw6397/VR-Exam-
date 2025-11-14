using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;            //Audio 관련 기능을 사용하기 위해 추가

[System.Serializable]  //Serializable 직렬화 (클래스 데이터를 형식을 인스펙터에서 보여주게 함)

public class Sound                       //사운드 클래스 선언
{
    public string name;                  //사운드 이름
    public AudioClip clip;               //사운드 클립

    //[Range(0f, 1f)]                      //인스펙터에서 범위 설정
    //public float volume = 1.0f;          //사운드 볼륨

    //[Range(0.1f, 3f)]
    //public float pitch = 1.0f;           //사운드 피치
    public bool loop;                    //반복 재생 여부
    //public AudioMixerGroup mixerGroup;   //오디오 믹서 그룹

    [HideInInspector]            //인스펙터 창에서 안보이게 가린다.
    public AudioSource source;   //오디오 소스
}

public class SoundManager : MonoBehaviour
{
    //static 전역으로 가져와서 사용 할 수 있게 해준다.  싱글톤패턴: 어디서든 전역으로 존재하고 접근할 수 있는 장점이 있다.
    public static SoundManager instance;               //싱글톤 인스턴스 화 시틴다.

    public List<Sound> sounds = new List<Sound>();
    //public AudioMixer audioMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;                  //싱글톤 패턴 적용
            DontDestroyOnLoad(gameObject);    //Scene이 변경되어도 이 오브젝트는 파괴 NO
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();  //소스 하나당 1개씩 컴포넌트를 더해준다.
            sound.source.clip = sound.clip;
            sound.source.volume = 1;
            sound.source.pitch = 1;
            sound.source.loop = sound.loop;
            //sound.source.outputAudioMixerGroup = sound.mixerGroup;  //오디오 믹서 그룹 설정
        }
    }
    // 사운드를 재생하는 매서드
    public void PlaySound(string name)                                  //인수 Name 받아서
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);   //List 안에 있는 name이 같은 것을 검색 후 soundToPlay 에 선언

        if (soundToPlay != null)
        {
            soundToPlay.source.Play();
        }
        else
        {
            Debug.LogWarning("사운드 : " + name + " 없습니다.");
        }
    }
}
