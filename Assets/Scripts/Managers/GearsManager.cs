using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.Components;
using static GameObjectUtils;
using static GameFoundationUtils;

public class GearsManager : Singleton<GearsManager>
{
    /*===== PRIVATE UI =====*/
    [Header("GEARS MENU")]
    [Header("UI prefabs")]
    [SerializeField] private GameObject _rune_viewer_prefab;
    [SerializeField] private GameObject _empty_rune_viewer_prefab;

    [Space(10)]

    [Header("Gears menu elements")]
    [SerializeField] private ItemView _equiped_weapon_viewer;
    [SerializeField] private Transform _equiped_runes_container;

    [Space(10)]

    [Header("RUNES MENU")]
    [Header("UI prefabs")]
    [SerializeField] private ItemView _available_rune_prefab;

    [Header("Runes menu elements")]
    [SerializeField] private GameObject _runes_menu;
    [SerializeField] private Transform _available_runes_container;

    /*===== PRIVATE =====*/
    private InventoryItemIdentifier rune_to_exchange;

    private void Start()
    {
        display_equiped_gear();
    }

    private void display_equiped_gear()
    {
        // DISPLAY WEAPON
        InventoryItem equiped_weapon = PlayerInfosManager.Instance.equiped_weapon;

        display_item_in_viewer(equiped_weapon, _equiped_weapon_viewer);

        // DISPLAY RUNES
        int nb_rune_slots = equiped_weapon.GetMutableProperty("nb_rune_slots");
        List <InventoryItem> equiped_runes = PlayerInfosManager.Instance.equiped_runes;

        // Clear viewer
        _equiped_runes_container.destroy_children();

        // Display rune slots and equiped runes
        for (int rune_slot=0; rune_slot < nb_rune_slots; rune_slot++)
        {
            if(equiped_runes[rune_slot] != null)
            {
                ItemView rune_viewer = Instantiate(
                    _rune_viewer_prefab,
                    Vector3.zero,
                    Quaternion.identity,
                    _equiped_runes_container
                ).GetComponent<ItemView>();

                InventoryItemIdentifier identifier = rune_viewer.transform.GetComponent<InventoryItemIdentifier>();
                identifier.id = equiped_runes[rune_slot].id;
                identifier.slot = rune_slot;
                display_item_in_viewer(equiped_runes[rune_slot], rune_viewer);
            }
            else
            {
                InventoryItemIdentifier empty_rune_slot = Instantiate(
                    _empty_rune_viewer_prefab,
                    Vector3.zero,
                    Quaternion.identity,
                    _equiped_runes_container
                ).GetComponent<InventoryItemIdentifier>();

                empty_rune_slot.slot = rune_slot;
            }
        }
    }

    public void start_rune_exchange(InventoryItemIdentifier rune_to_exchange_id)
    {
        rune_to_exchange = rune_to_exchange_id;
        List<InventoryItem> runes = get_inventory_items_from_tag("RUNE");

        _available_runes_container.destroy_children();

        foreach (InventoryItem rune in runes)
        {
            if(rune.GetMutableProperty("equiped") == false)
            {
                ItemView rune_viewer = Instantiate(
                    _available_rune_prefab,
                    Vector3.zero,
                    Quaternion.identity,
                    _available_runes_container
                ).GetComponent<ItemView>();

                InventoryItemIdentifier identifier = rune_viewer.transform.GetComponent<InventoryItemIdentifier>();
                identifier.id = rune.id;
                display_item_in_viewer(rune, rune_viewer);
            }
        }

        _runes_menu.SetActive(true);
    }

    public void exchange_rune(InventoryItemIdentifier rune_to_exhange_with_id)
    {
        InventoryItem rune_to_exchange_with = GameFoundationSdk.inventory.FindItem(rune_to_exhange_with_id.id);
        Debug.Log($"{rune_to_exchange_with.definition.displayName}");
    }
}