using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundMgr_KJS : MonoBehaviour
{
    [SerializeField] private AudioClip onEnableSound;  // Ȱ��ȭ�� �� ����� ����
    [SerializeField] private AudioClip onDisableSound; // ��Ȱ��ȭ�� �� ����� ����
    [SerializeField] private AudioSource audioSource;  // ���带 ����� AudioSource

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
