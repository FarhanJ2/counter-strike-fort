using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] gameSounds;
    [SerializeField] private AudioClip[] startRound;
    private AudioSource _globalSFX;

    public enum Sound
    {
        CT_WIN,
        T_WIN,
        BOMB_PL,
        BOMB_DEF,
        VICTORY_01,
        VICTORY_02
    }

    public enum VO
    {
        LETS_GO_CT_0,
        LETS_GO_CT_1
    }

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _globalSFX = GetComponent<AudioSource>();
    }

    public void PlaySound(Sound sound)
    {
        AudioClip clip = gameSounds[(int)sound];
        _globalSFX.PlayOneShot(clip);
    }

    public void PlaySound(VO vo)
    {
        AudioClip clip = startRound[(int)vo];
        _globalSFX.PlayOneShot(clip);
    }
}