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
        #region Mage
        public static List<Buff> GetMageSkills()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Energy Coat", EffectStatusIDs.ENERGYCOAT, Resources.mg_energycoat),
                new Buff("Sight Blaster", EffectStatusIDs.SIGHTBLASTER, Resources.wz_sightblaster),
                new Buff("Autospell", EffectStatusIDs.AUTOSPELL, Resources.sa_autospell),
                new Buff("Double Casting", EffectStatusIDs.DOUBLECASTING, Resources.pf_doublecasting),
                new Buff("Memorize", EffectStatusIDs.MEMORIZE, Resources.pf_memorize),
                new Buff("Telekinesis Intense", EffectStatusIDs.TELEKINESIS_INTENSE, Resources.telecinese),
                new Buff("Amplification", EffectStatusIDs.MYST_AMPLIFY, Resources.amplify),
                new Buff("Recognized Spell", EffectStatusIDs.RECOGNIZEDSPELL, Resources.recognized_spell)
            };

            return skills;
        }
        #endregion
        #region Merchant
        public static List<Buff> GetMerchantSkills()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Crazy Uproar", EffectStatusIDs.CRAZY_UPROAR, Resources.mc_loud),
                new Buff("Power-Thrust", EffectStatusIDs.OVERTHRUST, Resources.bs_overthrust),
                new Buff("Adrenaline Rush", EffectStatusIDs.ADRENALINE, Resources.bs_adrenaline),
                new Buff("Advanced Adrenaline Rush", EffectStatusIDs.ADRENALINE2, Resources.bs_adrenaline2),
                new Buff("Maximum Power-Thrust", EffectStatusIDs.OVERTHRUSTMAX, Resources.ws_overthrustmax),
                new Buff("Weapon Perfection", EffectStatusIDs.WEAPONPERFECT, Resources.bs_weaponperfect),
                new Buff("Power Maximize", EffectStatusIDs.MAXIMIZE, Resources.bs_maximize),
                new Buff("Cart Boost", EffectStatusIDs.CARTBOOST, Resources.ws_cartboost),
                new Buff("Meltdown", EffectStatusIDs.MELTDOWN, Resources.ws_meltdown),
                new Buff("Acceleration", EffectStatusIDs.ACCELERATION, Resources.mec_acceleration),
                new Buff("Cart Boost", EffectStatusIDs.GN_CARTBOOST, Resources.cart_boost),
                new Buff("Research Report", EffectStatusIDs.RESEARCHREPORT, Resources.researchreport)
            };

            return skills;
        }
        #endregion
        #region Thief
        public static List<Buff> GetThiefSkills()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Poison React", EffectStatusIDs.POISONREACT, Resources.as_poisonreact),
                new Buff("Reject Sword", EffectStatusIDs.SWORDREJECT, Resources.st_rejectsword),
                new Buff("Preserve", EffectStatusIDs.PRESERVE, Resources.st_preserve),
                new Buff("Enchant Deadly Poison", EffectStatusIDs.EDP, Resources.asc_edp),
                new Buff("Weapon Blocking", EffectStatusIDs.WEAPONBLOCKING, Resources.weapon_blocking)
            };

            return skills;
        }
        #endregion
        #region Acolyte
        public static List<Buff> GetAcolyteSkills()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Gloria", EffectStatusIDs.GLORIA, Resources.pr_gloria),
                new Buff("Magnificat", EffectStatusIDs.MAGNIFICAT, Resources.pr_magnificat),
                new Buff("Angelus", EffectStatusIDs.ANGELUS, Resources.al_angelus),
                new Buff("Rising Dragon", EffectStatusIDs.RAISINGDRAGON, Resources.rising_dragon),
                new Buff("Firm Faith", EffectStatusIDs.FIRM_FAITH, Resources.firm_faith),
                new Buff("Powerful Faith", EffectStatusIDs.POWERFUL_FAITH, Resources.powerful_faith),
                new Buff("Gentle Touch-Revitalize", EffectStatusIDs.GENTLETOUCH_REVITALIZE, Resources.gentle_touch_revitalize),
                new Buff("Gentle Touch-Convert", EffectStatusIDs.GENTLETOUCH_CHANGE, Resources.gentle_touch_convert),
                new Buff("Fury ", EffectStatusIDs.FURY, Resources.fury),
                new Buff("Impositio Manus",  EffectStatusIDs.IMPOSITIO, Resources.impositio_manus),
            };

            return skills;
        }
        #endregion

        #region Ninja
        public static List<Buff> GetNinjaSkills()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Cicada Skin Shed", EffectStatusIDs.PEEL_CHANGE, Resources.nj_utsusemi),
                new Buff("Ninja Aura", EffectStatusIDs.AURA_NINJA, Resources.nj_nen),
                new Buff("Izayoi", EffectStatusIDs.IZAYOI, Resources.izayoi)
            };

            return skills;
        }
        #endregion
        #region Takewondo
        public static List<Buff> GetTaekwonSkills()
        {
            List<Buff> skills = new List<Buff>
            {
                new Buff("Mild Wind (Earth)", EffectStatusIDs.PROPERTYGROUND, Resources.tk_mild_earth),
                new Buff("Mild Wind (Fire)", EffectStatusIDs.PROPERTYFIRE, Resources.tk_mild_fire),
                new Buff("Mild Wind (Water)", EffectStatusIDs.PROPERTYWATER, Resources.tk_mild_water),
                new Buff("Mild Wind (Wind)", EffectStatusIDs.PROPERTYWIND, Resources.tk_mild_wind),
                new Buff("Mild Wind (Ghost)", EffectStatusIDs.PROPERTYTELEKINESIS, Resources.tk_mild_ghost),
                new Buff("Mild Wind (Holy)", EffectStatusIDs.ASPERSIO, Resources.tk_mild_holy),
                new Buff("Mild Wind (Shadow)", EffectStatusIDs.PROPERTYDARK, Resources.tk_mild_shadow),
                new Buff("Tumbling", EffectStatusIDs.DODGE_ON, Resources.tumbling)
            };

            return skills;
        }
        #endregion
        #region Gun
        public static List<Buff> GetGunsSkills()
        {
            List<Buff> skills = new List<Buff>();

            skills.Add(new Buff("Gatling Fever", EffectStatusIDs.GATLINGFEVER, Resources.gatling_fever));
            skills.Add(new Buff("Madness Canceller", EffectStatusIDs.MADNESSCANCEL, Resources.madnesscancel));
            skills.Add(new Buff("Adjustment", EffectStatusIDs.ADJUSTMENT, Resources.adjustment));
            skills.Add(new Buff("Increase Accuracy", EffectStatusIDs.ACCURACY, Resources.increase_accuracy));

            return skills;
        }
        #endregion

        //Stuffs form below
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
