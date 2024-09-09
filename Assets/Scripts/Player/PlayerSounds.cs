using Random = UnityEngine.Random;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] private AudioClip[] concreteSounds, woodSounds, dirtSounds, sandSounds;
    [Header("Voice Lines")]
    [SerializeField] private AudioClip[] ctVo, tVo;
    [SerializeField] private AudioClip[] fallingSounds, landingSounds;

    [Header("UI")] [SerializeField] private AudioClip[] buyMenu; 
    [SerializeField] private AudioSource _source, _sourcePain;

    public bool IsPlaying => _source.isPlaying;

    public enum ConcreteFootsteps
    {
        CONCRETE_01,
        CONCRETE_02,
        CONCRETE_03,
        CONCRETE_04,
        CONCRETE_05,
        CONCRETE_06,
        CONCRETE_07,
        CONCRETE_08,
        CONCRETE_09,
        CONCRETE_10,
        CONCRETE_11,
        CONCRETE_12,
        CONCRETE_13,
        CONCRETE_14,
        CONCRETE_15,
        CONCRETE_16,
        CONCRETE_17,
    }

    public enum WoodFootsteps
    {
        WOOD_01,
        WOOD_02,
        WOOD_03,
        WOOD_04,
        WOOD_05,
        WOOD_06,
        WOOD_07,
        WOOD_08,
        WOOD_09,
        WOOD_10,
        WOOD_11,
        WOOD_12,
        WOOD_13,
        WOOD_14,
        WOOD_15,
    }

    public enum GrenadeVO
    {
        THROWING_FLASH_01,
        THROWING_FLASH_02
    }

    public enum CT_VO
    {
        DEFUSING_01,
        DEFUSING_02
    }

    public enum T_VO
    {
        PLANTING_01,
        PLANTING_02
    }
    
    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void StopSound()
    {
        _sourcePain.Stop();
    }

    public void PlayFootstep(string floorType)
    {
        switch (floorType)
        {
            case "Concrete":
                _source.PlayOneShot(concreteSounds[Random.Range(0, concreteSounds.Length)]);
                break;
            case "Wood":
                _source.PlayOneShot(woodSounds[Random.Range(0, woodSounds.Length)]);
                break;
            case "Dirt":
                _source.PlayOneShot(dirtSounds[Random.Range(0, dirtSounds.Length)]);
                break;
            case "Sand":
                _source.PlayOneShot(sandSounds[Random.Range(0, sandSounds.Length)]);
                break;
        }
    }

    public void PlayTVo(T_VO vo)
    {
        AudioClip clip = tVo[(int)vo];
        _source.PlayOneShot(clip);
    }

    public void PlayBuySound()
    {
        AudioClip clip = buyMenu[Random.Range(0, buyMenu.Length)];
        _source.PlayOneShot(clip);
    }

    public void PlayFallingSound()
    {
        AudioClip clip = fallingSounds[Random.Range(0, fallingSounds.Length)];
        _sourcePain.PlayOneShot(clip);
    }

    public void PlayLandingSound()
    {
        AudioClip clip = landingSounds[Random.Range(0, landingSounds.Length)];
        _sourcePain.PlayOneShot(clip);
    }
}
