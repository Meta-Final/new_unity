using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundMgr_KJS : MonoBehaviour
{
    [SerializeField] private AudioClip onEnableSound;  // 활성화될 때 재생할 사운드
    [SerializeField] private AudioClip onDisableSound; // 비활성화될 때 재생할 사운드
    [SerializeField] private AudioSource audioSource;  // 사운드를 재생할 AudioSource

    private void OnEnable()
    {
        PlaySound(onEnableSound);
    }

    private void OnDisable()
    {
        PlaySound(onDisableSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
