using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuyMenu : MonoBehaviour
{
    [SerializeField] private GameObject _ctWeaponHolder, _tWeaponHolder;
    [SerializeField] private Player _player;

    private void Start()
    {
    }

    private void OnEnable()
    {
        ShowTeamWeapons();
    }

    private void ShowTeamWeapons()
    {
        if (_player.PlayerTeam == Player.PlayerTeams.CT)
        {
            _ctWeaponHolder.SetActive(true);
            _tWeaponHolder.SetActive(false);
        }
        else
        {
            _ctWeaponHolder.SetActive(false);
            _tWeaponHolder.SetActive(true);
        }
    }

    private void PurchaseWeapon(int weaponId)
    {
        
    }

    private void Update()
    {
        
    }
}
