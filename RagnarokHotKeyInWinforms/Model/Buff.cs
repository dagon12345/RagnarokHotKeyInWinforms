using RagnarokHotKeyInWinforms.Utilities;
using System.Drawing;
using System;
using System.Collections.Generic;
using RagnarokHotKeyInWinforms.Properties;

namespace RagnarokHotKeyInWinforms.Model
{
    internal class Buff
    {
        public String name { get; set; }
        public EffectStatusIDs effectStatusID { get; set; }
        public Bitmap icon { get; set; }

        public Buff(string name, EffectStatusIDs effectStatus, Bitmap icon)
        {
            this.name = name;
            this.effectStatusID = effectStatus;
            this.icon = icon;
        }

        #region Skills
        //For form name SkillAutoBuffForm
        #region Archer Skills
        public static List<Buff> GetArcherSkills()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Concentration", EffectStatusIDs.CONCENTRATION, Resources.ac_concentration),
                new Buff("wind walk", EffectStatusIDs.WINDWALK, Resources.sn_windwalk),
                new Buff("true sight", EffectStatusIDs.TRUESIGHT, Resources.sn_sight),
                new Buff("ilimitar", EffectStatusIDs.UNLIMIT, Resources.Ilimitar),
                new Buff("a poem of bragi", EffectStatusIDs.POEMBRAGI, Resources.poem_of_bragi),
                new Buff("windmill rush", EffectStatusIDs.RUSH_WINDMILL, Resources.windmill_rush),
                new Buff("moonlight serenade", EffectStatusIDs.MOONLIT_SERENADE, Resources.moonlight_serenade),
                new Buff("frigg's song", EffectStatusIDs.FRIGG_SONG, Resources.friggs_song)
            };

            return skills;
        }

        #endregion
        #region SwordsMan
        public static List<Buff> GetSwordmanSkill()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Endure", EffectStatusIDs.ENDURE, Resources.sm_endure),
                new Buff("Auto Beserk", EffectStatusIDs.AUTOBERSERK, Resources.sm_autoberserk),
                new Buff("Guard", EffectStatusIDs.AUTOGUARD, Resources.cr_autoguard),
                new Buff("Shield Reflection", EffectStatusIDs.REFLECTSHIELD, Resources.cr_reflectshield),
                new Buff("Spear Quicken", EffectStatusIDs.SPEARQUICKEN, Resources.cr_spearquicken),
                new Buff("Defending Aura", EffectStatusIDs.DEFENDER, Resources.cr_defender),
                new Buff("Dedication", EffectStatusIDs.LKCONCENTRATION, Resources.lk_concentration),
                new Buff("Frenzy", EffectStatusIDs.BERSERK, Resources.lk_berserk),
                new Buff("Twohand Quicken", EffectStatusIDs.TWOHANDQUICKEN, Resources.mer_quicken),
                new Buff("Parry", EffectStatusIDs.PARRYING, Resources.ms_parrying),
                new Buff("Aura Blade", EffectStatusIDs.AURABLADE, Resources.lk_aurablade),
                new Buff("Enchant Blade", EffectStatusIDs.ENCHANT_BLADE, Resources.enchant_blade),
                new Buff("Shrink", EffectStatusIDs.CR_SHRINK, Resources.cr_shrink),
                new Buff("Inspiration", EffectStatusIDs.INSPIRATION, Resources.lg_inspiration),
                new Buff("Prestige", EffectStatusIDs.PRESTIGE, Resources.lg_prestige),
                new Buff("Shield Spell", EffectStatusIDs.SHIELDSPELL, Resources.lg_shieldspell),
                new Buff("Vanguard Force", EffectStatusIDs.FORCEOFVANGUARD, Resources.vanguard_force),
                new Buff("Servant Weapon", EffectStatusIDs.SERVANTWEAPON, Resources.servant_weapon),
                new Buff("Reflect Damage", EffectStatusIDs.REFLECTDAMAGE, Resources.reflect_damage),
            };

            return skills;
        }
        #endregion

        #endregion
        #region Stuffs
        //For form name StuffAutoBuffForm
        #region Potions
        public static List<Buff> GetPotionsBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Concentration Potion", EffectStatusIDs.CONCENTRATION_POTION, Resources.concentration_potiongif),
                new Buff("Awakening Potion", EffectStatusIDs.AWAKENING_POTION, Resources.awakening_potion),
                new Buff("Berserk Potion", EffectStatusIDs.BERSERK_POTION, Resources.berserk_potion),
                new Buff("Regeneration Potion", EffectStatusIDs.REGENERATION_POTION, Resources.regeneration),
                new Buff("HP Increase Potion", EffectStatusIDs.HP_INCREASE_POTION_LARGE, Resources.ghp),
                new Buff("SP Increase Potion", EffectStatusIDs.SP_INCREASE_POTION_LARGE, Resources.gsp),
                new Buff("Red Herb Activator", EffectStatusIDs.RED_HERB_ACTIVATOR, Resources.red_herb_activator),
                new Buff("Blue Herb Activator", EffectStatusIDs.BLUE_HERB_ACTIVATOR, Resources.blue_herb_activator),
                new Buff("Golden X", EffectStatusIDs.REF_T_POTION, Resources.Golden_X),
                new Buff("Energy Drink", EffectStatusIDs.ENERGY_DRINK_RESERCH, Resources.energetic_drink),
                new Buff("Mega Resist Potion", EffectStatusIDs.TARGET_BLOOD, Resources.mega_resist_potion),
                new Buff("Full SwingK Potion", EffectStatusIDs.FULL_SWINGK, Resources.swing_k),
                new Buff("Mana Plus Potion", EffectStatusIDs.MANA_PLUS, Resources.mana_plus),
                new Buff("Blessing Of Tyr", EffectStatusIDs.BASICHIT, Resources.blessing_of_tyr)
            };

            return skills;
        }
        public static List<Buff> GetElementalsBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Fire Conversor", EffectStatusIDs.PROPERTYFIRE, Resources.ele_fire_converter),
                new Buff("Wind Conversor", EffectStatusIDs.PROPERTYWIND, Resources.ele_wind_converter),
                new Buff("Earth Conversor", EffectStatusIDs.PROPERTYGROUND, Resources.ele_earth_converter),
                new Buff("Water Conversor", EffectStatusIDs.PROPERTYWATER, Resources.ele_water_converter),
                new Buff("Cursed Water", EffectStatusIDs.PROPERTYDARK, Resources.cursed_water),
                new Buff("Fireproof Potion", EffectStatusIDs.RESIST_PROPERTY_FIRE, Resources.fireproof),
                new Buff("Waterproof Potion", EffectStatusIDs.RESIST_PROPERTY_WATER, Resources.coldproof),
                new Buff("Windproof Potion", EffectStatusIDs.RESIST_PROPERTY_WIND, Resources.thunderproof),
                new Buff("Earthproof Potion", EffectStatusIDs.RESIST_PROPERTY_GROUND, Resources.earhproof)
            };

            return skills;
        }

        public static List<Buff> GetFoodBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("STR Food", EffectStatusIDs.FOOD_STR, Resources.strfood),
                new Buff("AGI Food", EffectStatusIDs.FOOD_AGI, Resources.agi_food),
                new Buff("VIT Food", EffectStatusIDs.FOOD_VIT, Resources.vit_food),
                new Buff("INT Food", EffectStatusIDs.FOOD_INT, Resources.int_food),
                new Buff("DEX Food", EffectStatusIDs.FOOD_DEX, Resources.dex_food),
                new Buff("LUK Food", EffectStatusIDs.FOOD_LUK, Resources.luk_food),

                new Buff("3RD STR Food", EffectStatusIDs.STR_3RD_FOOD, Resources.str_3rd_food),
                new Buff("3RD AGI Food", EffectStatusIDs.AGI_3RD_FOOD, Resources.agi_3rd_food),
                new Buff("3RD VIT Food", EffectStatusIDs.VIT_3RD_FOOD, Resources.vit_3rd_food),
                new Buff("3RD INT Food", EffectStatusIDs.INT_3RD_FOOD, Resources.int_3rd_food),
                new Buff("3RD DEX Food", EffectStatusIDs.DEX_3RD_FOOD, Resources.dex_3rd_food),
                new Buff("3RD LUK Food", EffectStatusIDs.LUK_3RD_FOOD, Resources.luk_3rd_food),
                new Buff("Almighty", EffectStatusIDs.RWC_2011_SCROLL, Resources.almighty),
                new Buff("CASH Food", EffectStatusIDs.FOOD_VIT_CASH, Resources.cash_food),
            };


            return skills;
        }

        public static List<Buff> GetBoxesBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Drowsiness Box", EffectStatusIDs.DROWSINESS_BOX, Resources.drowsiness),
                new Buff("Resentment Box", EffectStatusIDs.RESENTMENT_BOX, Resources.resentment),
                new Buff("Sunlight Box", EffectStatusIDs.SUNLIGHT_BOX, Resources.sunbox),
                new Buff("Box of Gloom", EffectStatusIDs.CONCENTRATION, Resources.gloom),
                new Buff("Box of Thunder", EffectStatusIDs.BOX_OF_THUNDER, Resources.speed),
                new Buff("Speed Potion / Guyak", EffectStatusIDs.SPEED_POT, Resources.speedpotion),
                new Buff("Anodyne", EffectStatusIDs.ENDURE, Resources.anodyne),
                new Buff("Aloevera", EffectStatusIDs.PROVOKE, Resources.aloevera),
                new Buff("Abrasivo", EffectStatusIDs.CRITICALPERCENT, Resources.abrasive),
                new Buff("Combat Pill", EffectStatusIDs.COMBAT_PILL, Resources.combat_pill),
                new Buff("Celermine Juice", EffectStatusIDs.ENRICH_CELERMINE_JUICE, Resources.celermine)
            };

            return skills;
        }

        public static List<Buff> GetScrollBuffs()
        {
            List<Buff> skills = new List<Buff>
            {

                new Buff("Increase Agility Scroll", EffectStatusIDs.INC_AGI, Resources.al_incagi1),
                new Buff("Bless Scroll", EffectStatusIDs.BLESSING, Resources.al_blessing1),
                new Buff("Full Chemical Protection (Scroll)", EffectStatusIDs.PROTECTARMOR, Resources.cr_fullprotection),
                new Buff("Link Scroll", EffectStatusIDs.SOULLINK, Resources.sl_soullinker),
                new Buff("Monster Transform",  EffectStatusIDs.MONSTER_TRANSFORM, Resources.mob_transform),
                new Buff("Assumptio",  EffectStatusIDs.ASSUMPTIO, Resources.assumptio),

            };

            return skills;
        }

        public static List<Buff> GetETCBuffs()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("THURISAZ Rune", EffectStatusIDs.THURISAZ, Resources.THURISAZ),
                new Buff("OTHILA Rune", EffectStatusIDs.OTHILA, Resources.OTHILA),
                new Buff("HAGALAZ Rune", EffectStatusIDs.HAGALAZ, Resources.HAGALAZ),
                new Buff("LUX AMINA Rune", EffectStatusIDs.LUX_AMINA, Resources.LUX_AMINA),
                new Buff("Cat Can", EffectStatusIDs.OVERLAPEXPUP, Resources.cat_can),
                new Buff("Bubble Gum", EffectStatusIDs.CASH_RECEIVEITEM, Resources.he_bubble_gum),
                new Buff("Battle Manual", EffectStatusIDs.CASH_PLUSEXP, Resources.combat_manual)
            };

            return skills;
        }
        #endregion
        #endregion

    }
}
