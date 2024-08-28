using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class UIBuyMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerMoneyText;
    [SerializeField] private GameObject _ctWeaponHolder, _tWeaponHolder;
    [SerializeField] private PlayerBridge _playerBridge;

    private void OnEnable()
    {
        ShowTeamWeapons();
    }

    private void ShowTeamWeapons()
    {
        if (_playerBridge.player.PlayerTeam == Player.PlayerTeams.CT)
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

    public void PurchaseWeapon(int weaponId)
    {
        Weapon.WeaponName weaponName = Weapon.WeaponName.NONE;
        switch (weaponId)
        {
            case 0:
                weaponName = Weapon.WeaponName.ARMOR;
                break;
            case 1:
                weaponName = Weapon.WeaponName.ARMOR_HELM;
                break;
            case 2:
                weaponName = Weapon.WeaponName.USPS;
                break;
            case 3:
                weaponName = Weapon.WeaponName.AK47;
                break;
        }
    
        if (weaponName == Weapon.WeaponName.NONE) return;
        
        if (_playerBridge.player.PlayerMoney < GetPrice(weaponName))
        {
            Debug.Log("Player does not have enough money");
            return;
        }
        
        _playerBridge.player.PlayerMoney -= GetPrice(weaponName);

        if (weaponName == Weapon.WeaponName.ARMOR || weaponName == Weapon.WeaponName.ARMOR_HELM)
        {
            _playerBridge.player.ownedWeapons.HasArmor = true;
            if (weaponName == Weapon.WeaponName.ARMOR_HELM)
            {
                _playerBridge.player.ownedWeapons.HasHelmet = true;
            }
        }
        
        if (GetIsPrimary(weaponName))
        {
            _playerBridge.player.ownedWeapons.CurrentPrimary = weaponName;
        }
        else
        {
            _playerBridge.player.ownedWeapons.CurrentSecondary = weaponName;
        }
    }

    private bool GetIsPrimary(Weapon.WeaponName weapon)
    {
        Type type = weapon.GetType();
        FieldInfo fieldInfo = type.GetField(weapon.ToString());
        WeaponAttr attribute = (WeaponAttr)fieldInfo.GetCustomAttribute(typeof(WeaponAttr));
        return attribute?.IsPrimary ?? false;
    }
    
    private int GetPrice(Weapon.WeaponName weapon)
    {
        Type type = weapon.GetType();
        FieldInfo fieldInfo = type.GetField(weapon.ToString());
        WeaponAttr attribute = (WeaponAttr)fieldInfo.GetCustomAttribute(typeof(WeaponAttr));

        return attribute?.WeaponPrice ?? 0;
    }

    private void Update()
    {
        _playerMoneyText.text = "$" +_playerBridge.player.PlayerMoney;
    }
}