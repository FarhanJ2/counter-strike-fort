using System.Collections;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class C4 : Weapon
{
    [SerializeField] private BoxCollider _physicsCollider, _triggerCollider;
    [FormerlySerializedAs("_beepingSounds")] [SerializeField] private AudioClip[] _c4Sounds;
    [SerializeField] private AudioSource _source;
    [SerializeField] private GameObject _model;
    [SerializeField] private Rigidbody _rb;

    public enum C4Sounds
    {
        C4_BEEP_01,
        C4_BEEP_02,
        C4_DISARM_START,
        C4_DISARM_FINISH,
        C4_PLANTING,
        C4_FUSE_ON,
        C4_RAMP,
        C4_EXPLOSION
    }
    public bool BombDown { get; set; }
    private PlayerBridge _bridge;

    private readonly float _plantingTime = 3f;
    private readonly float _timeToExplosion = 45f;
    private float _bombTimer;

    private Coroutine _plantingCoroutine, _soundCoroutine;
    
    public override void Fire(PlayerBridge bridge)
    {
        if (BombDown || _bridge == null || !bridge.player.ownedWeapons.HasBomb || !bridge.player.InBombZone) return;
        PlantBombServer(_bridge);
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlantBombServer(PlayerBridge _bridge)
    {
        PlantBombObserver(_bridge);
    }

    [ObserversRpc]
    private void PlantBombObserver(PlayerBridge _bridge)
    {
        Debug.Log("Planting");
        _bridge.playerMovement.canMove = false;
        if (_plantingCoroutine != null)
        {
            StopCoroutine(_plantingCoroutine);
        }

        _bridge.playerSounds.PlayTVo(PlayerSounds.T_VO.PLANTING_02);
        _plantingCoroutine = StartCoroutine(PlantBomb(_bridge));
    }

    IEnumerator PlantBomb(PlayerBridge _bridge)
    {
        PlaySound(C4Sounds.C4_PLANTING);
        yield return new WaitForSeconds(_plantingTime);
        BombDown = true;
        
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        _physicsCollider.enabled = true;
        _rb.isKinematic = true;
        Vector3 playerPosition = _bridge.transform.position;
        float offsetY = 0.952f;
        SetBombOnPlayerServer(gameObject, new Vector3(playerPosition.x, playerPosition.y - offsetY, playerPosition.z),
            Quaternion.Euler(0f, _bridge.playerCamera.transform.eulerAngles.y + 90f, 90f));
        
        _bridge.playerMovement.canMove = true;
        AudioManager.Instance.PlaySound(AudioManager.Sound.BOMB_PL);
        GameManager.Instance.C4Planted = true;
        _bombTimer = _timeToExplosion;
        _soundCoroutine = StartCoroutine(PlayBeepingSounds());
    }
    
    public void StopPlanting()
    {
        if (_plantingCoroutine != null)
        {
            StopCoroutine(_plantingCoroutine);
            _bridge.playerMovement.canMove = true;
        }
    }

    IEnumerator PlayBeepingSounds()
    {
        yield return new WaitForSeconds(.25f);
        while (!GameManager.Instance.C4Exploded)
        {
            PlaySound(C4Sounds.C4_BEEP_02);
            float waitTime = Mathf.Clamp(_bombTimer / _timeToExplosion, .1f, _timeToExplosion);
            Debug.Log(waitTime);
            yield return new WaitForSeconds(waitTime);
            if (_bombTimer <= 2f)
            {
                break;
            }
        }
        PlaySound(C4Sounds.C4_FUSE_ON);
        yield return new WaitForSeconds(1f);
        PlaySound(C4Sounds.C4_RAMP);
        yield return new WaitForSeconds(.75f);
        PlaySound(C4Sounds.C4_EXPLOSION);
    }

    public void DefuseBomb(PlayerBridge _bridge)
    {
        DefuseBombServer(_bridge);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DefuseBombServer(PlayerBridge _bridge)
    {
        DefuseBombObserver(_bridge);
    }

    [ObserversRpc]
    private void DefuseBombObserver(PlayerBridge _bridge)
    {
        _bridge.playerMovement.canMove = false;
        
    }

    private void Start()
    {
        // _bombDown.OnChange += ChangeBombDown;
        BombDown = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.C4Planted) return;
        
        if (other.CompareTag("Player"))
        {
            _bridge = other.GetComponent<PlayerBridge>();
            if (_bridge.player.PlayerTeam == Player.PlayerTeams.T)
            {
                // _bridge.InputManager.PlayerControls.Attack.Fire.started += _ => Fire(_bridge);
                // _bridge.InputManager.PlayerControls.Attack.Fire.canceled += _ =>
                // {
                //     if (_plantingCoroutine != null)
                //     {
                //         StopCoroutine(_plantingCoroutine);
                //         _bridge.playerMovement.canMove = true;
                //     }
                // };
                _bridge.player.ownedWeapons.HasBomb = true;
                _physicsCollider.enabled = false;
                BombDown = false;
            }
        }
    }

    public void PlaySound(C4Sounds sound)
    {
        AudioClip clip = _c4Sounds[(int)sound];
        _source.PlayOneShot(clip);
    }

    private void Update()
    {
        if (!GameManager.Instance.C4Planted)
        {
            _rb.isKinematic = !BombDown; // need to find a way to sync this across the network
        }
        
        if (!BombDown)
        {
            SetBombOnPlayerServer(gameObject, _bridge.PlayerInventory.BombHolder.transform.position, _bridge.PlayerInventory.BombHolder.transform.rotation);
        }

        if (GameManager.Instance.C4Planted && !GameManager.Instance.C4Exploded)
        {
            if (_bombTimer >= 0)
            {
                _bombTimer -= Time.deltaTime;
                // Debug.Log(_bombTimer);
            }
            else
            {
                GameManager.Instance.C4Exploded = true;
                GameManager.InvokeMajorEvent();
                _bombTimer = 0f;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetBombOnPlayerServer(GameObject bomb, Vector3 position, Quaternion rotation)
    {
        SetBombOnPlayerObserver(bomb, position, rotation);
    }

    [ObserversRpc]
    private void SetBombOnPlayerObserver(GameObject bomb, Vector3 position, Quaternion rotation)
    {
        _rb.isKinematic = true;
        bomb.transform.position = position;
        bomb.transform.rotation = rotation;
    }
    
    public void DropBomb()
    {
        DropBombServer(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropBombServer(GameObject obj)
    {
        Debug.Log("Drop bomb logic ran");
        DropBombObserver(obj);
    }

    [ObserversRpc]
    private void DropBombObserver(GameObject obj)
    {
        _bridge = null;
        obj.GetComponentInChildren<MeshRenderer>().enabled = true;
        _physicsCollider.enabled = true;
        StartCoroutine(ToggleWithDelayBombDown());
        _rb.isKinematic = false;
    }

    private IEnumerator ToggleWithDelayBombDown()
    {
        BombDown = true;
        _triggerCollider.enabled = false;
        yield return new WaitForSeconds(3f);
        _triggerCollider.enabled = true;
    }
}
