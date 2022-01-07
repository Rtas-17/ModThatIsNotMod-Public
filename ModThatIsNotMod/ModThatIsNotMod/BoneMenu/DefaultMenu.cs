﻿using ModThatIsNotMod.Internals;
using ModThatIsNotMod.MiniMods;
using StressLevelZero.Combat;
using StressLevelZero.Data;
using StressLevelZero.Player;
using StressLevelZero.Props.Weapons;
using System;
using UnityEngine;

namespace ModThatIsNotMod.BoneMenu
{
    internal static class DefaultMenu
    {
        private static PlayerInventory inventory;
        private static Weight ammoWeight = Weight.LIGHT;
        private static int ammoAmount = 1000;

        private static MenuCategory rootCategory;
        private static MenuCategory itemSpawningCategory;
        private static MenuCategory ammoMenuCategory;
        private static MenuCategory prefsCategory;
        private static MenuCategory menuOffsetCategory;


        /// <summary>
        /// Add some default options to the menu that will be useful to most people
        /// </summary>
        public static void CreateDefaultElements()
        {
            // Categories
            rootCategory = MenuManager.CreateCategory("ModThatIsNotMod", Color.grey);
            itemSpawningCategory = rootCategory.CreateSubCategory("Item Spawning", Color.grey);
            ammoMenuCategory = rootCategory.CreateSubCategory("Ammo Menu", Color.grey);
            prefsCategory = rootCategory.CreateSubCategory("Preferences", Color.grey);
            menuOffsetCategory = prefsCategory.CreateSubCategory("Menu Offset", Color.grey);

            // Item spawning
            itemSpawningCategory.CreateFunctionElement("Spawn Utility Gun", Color.white, new Action(() => RadialMenuEverywhere.SpawnUtilGun(1.5f)));
            itemSpawningCategory.CreateFunctionElement("Spawn Nimbus Gun", Color.white, new Action(() => RadialMenuEverywhere.SpawnNimbusGun(1.5f)));
            itemSpawningCategory.CreateFunctionElement("Spawn Random Custom Item", Color.white, SpawnRandomCustomItem);

            // Ammo menu
            ammoMenuCategory.CreateEnumElement("Type", Color.white, ammoWeight, (value) => { ammoWeight = (Weight)value; });
            ammoMenuCategory.CreateIntElement("Amount", Color.white, ammoAmount, (value) => { ammoAmount = value; }, increment: 500, invokeOnValueChanged: true);
            ammoMenuCategory.CreateFunctionElement("Add Ammo", Color.white, AddAmmo);

            // Preferences
            prefsCategory.CreateBoolElement("Mag Eject Button", Color.white, Preferences.enableMagEjectButton.value, new Action<bool>((value) => Preferences.enableMagEjectButton.SetValue(value)));
            prefsCategory.CreateIntElement("Presses To Eject", Color.white, Preferences.pressesToEjectMag.value, new Action<int>((value) => Preferences.pressesToEjectMag.SetValue(value)), minValue: 1, invokeOnValueChanged: true);
            prefsCategory.CreateBoolElement("Auto Eject Mags", Color.white, Preferences.autoEjectEmptyMags.value, new Action<bool>((value) => Preferences.autoEjectEmptyMags.SetValue(value)));
            prefsCategory.CreateBoolElement("Override Mag Eject Settings", Color.white, Preferences.overrideMagEjectSettings.value, new Action<bool>((value) => Preferences.overrideMagEjectSettings.SetValue(value)));
            prefsCategory.CreateBoolElement("Reload Items On Level Change", Color.white, Preferences.reloadItemsOnLevelChange.value, new Action<bool>((value) => Preferences.reloadItemsOnLevelChange.SetValue(value)));

            // Menu offset
            menuOffsetCategory.CreateFloatElement("X Offset", Color.white, Preferences.menuOffsetX.value, new Action<float>((value) => { Preferences.menuOffsetX.SetValue(value); MenuManager.menuOffset.x = value; }), 0.05f);
            menuOffsetCategory.CreateFloatElement("Y Offset", Color.white, Preferences.menuOffsetY.value, new Action<float>((value) => { Preferences.menuOffsetY.SetValue(value); MenuManager.menuOffset.y = value; }), 0.05f);
            menuOffsetCategory.CreateFloatElement("Z Offset", Color.white, Preferences.menuOffsetZ.value, new Action<float>((value) => { Preferences.menuOffsetZ.SetValue(value); MenuManager.menuOffset.z = value; }), 0.05f);
        }

        /// <summary>
        /// Gets a random non-magazine custom item and spawns it in front of the player
        /// </summary>
        private static void SpawnRandomCustomItem()
        {
            Vector3 spawnPos = Player.GetPlayerHead().transform.position + Player.GetPlayerHead().transform.forward * 1.5f;
            Quaternion spawnRot = Player.GetPlayerHead().transform.rotation;
            SpawnableObject spawnable = null;
            while (spawnable == null || spawnable.title.Contains(".bcm") || spawnable.prefab.GetComponent<Magazine>() != null)
                spawnable = CustomItems.GetRandomCustomSpawnable();
            CustomItems.SpawnFromPool(spawnable.title, spawnPos, spawnRot);
        }

        private static void AddAmmo()
        {
            if (inventory == null)
                inventory = GameObject.FindObjectOfType<PlayerInventory>();
            if (inventory != null)
                inventory.AddAmmo(ammoWeight, ammoAmount);
        }
    }
}