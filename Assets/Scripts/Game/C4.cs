using System.Collections;
using FishNet.Object;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class C4 : Weapon
{
    [SerializeField] private BoxCollider _physicsCollider, _triggerCollider;
    [SerializeField] private GameObject _model;
    [SerializeField] private Rigidbody _rb;
    public bool BombDown { get; set; }
    private PlayerBridge _bridge;

    private float _plantingTime = 3f;

    public override void Fire()
    {
        if (BombDown || _bridge == null) return;
    }

    private void Start()
    {
        // _bombDown.OnChange += ChangeBombDown;
        BombDown = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Picked up by player");
            _bridge = other.GetComponent<PlayerBridge>();
            if (_bridge.player.PlayerTeam == Player.PlayerTeams.T)
            {
                _bridge.player.ownedWeapons.HasBomb = true;
                _physicsCollider.enabled = false;
                BombDown = false;
            }
        }
    }

    private void Update()
    {
        _rb.isKinematic = !BombDown; // need to find a way to sync this across the network
        
        if (!BombDown)
        {
            SetBombOnPlayerServer(gameObject, _bridge.PlayerInventory.BombHolder.transform.position, _bridge.PlayerInventory.BombHolder.transform.rotation);
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
