using UnityEngine;
using TMPro;
using System.Collections;

public class Btn_TextChanger_KJS : MonoBehaviour
{
    public TMP_Text targetText; // TMP_Text 컴포넌트 참조

    // 버튼 1, 2, 3, 4에 출력할 텍스트를 각각 설정
    public string textForButton1;
    public string textForButton2;
    public string textForButton3;
    public string textForButton4; // 버튼 4용 텍스트

    public GameObject uiToEnableForButton1; // 버튼 1에서 활성화할 UI
    public GameObject uiToEnableForButton2; // 버튼 2에서 활성화할 UI
    public GameObject uiToEnableAfterButton3; // 버튼 3의 코루틴이 끝난 후 활성화할 UI
    public GameObject uiToEnableForButton4; // 버튼 4에서 활성화할 UI

    public float typingSpeed = 0.1f; // 텍스트가 출력되는 속도 (초)
    public float delayAfterButton3 = 0.5f; // 버튼 3 이후 UI 활성화 딜레이 (초)

    public AudioClip bgmClip; // 코루틴 동안 재생할 오디오 클립
    private GameObject bgmObject; // PlayClipAtPoint로 생성된 임시 오디오 객체

    private Coroutine typingCoroutine; // 현재 실행 중인 코루틴을 추적

    // 랜덤 출력 텍스트 목록
    private readonly string[] randomTexts = {
        "무슨일이야 삐약!",
        "심심해 삐약!",
        "무슨 기사를 적어볼까? 삐약!",
        "많은 기사를 서로 교환하자 삐약!"
    };

    private void OnEnable()
    {
        StartTyping(GetRandomText(), false); // 활성화될 때 랜덤 텍스트를 한 글자씩 출력
    }

    // 랜덤으로 텍스트 반환
    private string GetRandomText()
    {
        int randomIndex = Random.Range(0, randomTexts.Length); // 랜덤 인덱스 생성
        return randomTexts[randomIndex]; // 랜덤 텍스트 반환
    }

    // 버튼 1에서 호출될 메서드
    public void ChangeTextToButton1Text()
    {
        StartTyping(textForButton1, false); // 버튼 1은 UI를 비활성화하지 않음
        if (uiToEnableForButton1 != null)
        {
            uiToEnableForButton1.SetActive(true); // 버튼 1의 UI 활성화
        }
    }

    // 버튼 2에서 호출될 메서드
    public void ChangeTextToButton2Text()
    {
        StartTyping(textForButton2, false); // 버튼 2는 UI를 비활성화하지 않음
        if (uiToEnableForButton2 != null)
        {
            uiToEnableForButton2.SetActive(true); // 버튼 2의 UI 활성화
        }
    }

    // 버튼 3에서 호출될 메서드
    public void ChangeTextToButton3Text()
    {
        StartTyping(textForButton3, true); // 버튼 3은 UI를 비활성화함
    }

    // 버튼 4에서 호출될 메서드
    public void ChangeTextToButton4Text()
    {
        StartTyping(textForButton4, false); // 버튼 4는 UI를 비활성화하지 않음
        if (uiToEnableForButton4 != null)
        {
            uiToEnableForButton4.SetActive(true); // 버튼 4의 UI 활성화
        }
    }

    // 텍스트 출력 시작 (기존 코루틴 중단 후 새로운 코루틴 실행)
    private void StartTyping(string textToDisplay, bool deactivateUIAfterTyping)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // 기존 코루틴이 있으면 중지
        }

        typingCoroutine = StartCoroutine(TypeTextCoroutine(textToDisplay, deactivateUIAfterTyping));
    }

    // 코루틴으로 텍스트를 한 글자씩 출력
    private IEnumerator TypeTextCoroutine(string textToDisplay, bool deactivateUIAfterTyping)
    {
        // BGM 재생 시작 (bgmClip이 설정되어 있으면)
        if (bgmClip != null)
        {
            bgmObject = PlayBGM(bgmClip);
        }

        targetText.text = ""; // 기존 텍스트 초기화

        foreach (char letter in textToDisplay)
        {
            targetText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed); // 설정된 속도로 대기
        }

        typingCoroutine = null; // 코루틴 완료 후 초기화

        // BGM 정지
        StopBGM();

        // 버튼 3에 의한 호출이고, 특정 UI가 설정되어 있다면 활성화 (0.5초 딜레이 추가)
        if (deactivateUIAfterTyping)
        {
            if (uiToEnableAfterButton3 != null)
            {
                yield return new WaitForSeconds(delayAfterButton3); // 0.5초 대기
                uiToEnableAfterButton3.SetActive(true); // 특정 UI 활성화
            }

            // 이 스크립트가 포함된 UI 비활성화
            gameObject.SetActive(false);
        }
    }

    // Play BGM using PlayClipAtPoint
    private GameObject PlayBGM(AudioClip clip)
    {
        GameObject audioObject = new GameObject("BGM_Audio"); // 임시 오디오 객체 생성
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = true; // BGM이 반복되도록 설정
        audioSource.playOnAwake = false;
        audioSource.Play();

        return audioObject; // 오디오 객체 반환
    }

    // Stop BGM and destroy the audio object
    private void StopBGM()
    {
        if (bgmObject != null)
        {
            Destroy(bgmObject); // 임시 오디오 객체 제거
        }
    }
}