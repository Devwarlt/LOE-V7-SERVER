﻿#region

using System;
using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.realm
{
    public enum CurrencyType
    {
        Gold = 0,
        Fame = 1,
        GuildFame = 2,
        FortuneTokens = 3
    }

    public struct StatsType
    {
        public readonly static StatsType MAX_HP_STAT = 0;
        public readonly static StatsType HP_STAT = 1;
        public readonly static StatsType SIZE_STAT = 2;
        public readonly static StatsType MAX_MP_STAT = 3;
        public readonly static StatsType MP_STAT = 4;
        public readonly static StatsType NEXT_LEVEL_EXP_STAT = 5;
        public readonly static StatsType EXP_STAT = 6;
        public readonly static StatsType LEVEL_STAT = 7;
        public readonly static StatsType ATTACK_STAT = 20;
        public readonly static StatsType DEFENSE_STAT = 21;
        public readonly static StatsType SPEED_STAT = 22;
        public readonly static StatsType VITALITY_STAT = 26;
        public readonly static StatsType WISDOM_STAT = 27;
        public readonly static StatsType DEXTERITY_STAT = 28;
        public readonly static StatsType CONDITION_STAT = 29;
        public readonly static StatsType NUM_STARS_STAT = 30;
        public readonly static StatsType NAME_STAT = 31; //Is UTF
        public readonly static StatsType TEX1_STAT = 32;
        public readonly static StatsType TEX2_STAT = 33;
        public readonly static StatsType MERCHANDISE_TYPE_STAT = 34;
        public readonly static StatsType CREDITS_STAT = 35;
        public readonly static StatsType MERCHANDISE_PRICE_STAT = 36;
        public readonly static StatsType ACTIVE_STAT = 37;
        public readonly static StatsType ACCOUNT_ID_STAT = 38; //Is UTF
        public readonly static StatsType FAME_STAT = 39;
        public readonly static StatsType MERCHANDISE_CURRENCY_STAT = 40;
        public readonly static StatsType CONNECT_STAT = 41;
        public readonly static StatsType MERCHANDISE_COUNT_STAT = 42;
        public readonly static StatsType MERCHANDISE_MINS_LEFT_STAT = 43;
        public readonly static StatsType MERCHANDISE_DISCOUNT_STAT = 44;
        public readonly static StatsType MERCHANDISE_RANK_REQ_STAT = 45;
        public readonly static StatsType MAX_HP_BOOST_STAT = 46;
        public readonly static StatsType MAX_MP_BOOST_STAT = 47;
        public readonly static StatsType ATTACK_BOOST_STAT = 48;
        public readonly static StatsType DEFENSE_BOOST_STAT = 49;
        public readonly static StatsType SPEED_BOOST_STAT = 50;
        public readonly static StatsType VITALITY_BOOST_STAT = 51;
        public readonly static StatsType WISDOM_BOOST_STAT = 52;
        public readonly static StatsType DEXTERITY_BOOST_STAT = 53;
        public readonly static StatsType OWNER_ACCOUNT_ID_STAT = 54; //Is UTF
        public readonly static StatsType RANK_REQUIRED_STAT = 55;
        public readonly static StatsType NAME_CHOSEN_STAT = 56;
        public readonly static StatsType CURR_FAME_STAT = 57;
        public readonly static StatsType NEXT_CLASS_QUEST_FAME_STAT = 58;
        public readonly static StatsType GLOW_COLOR_STAT = 59;
        public readonly static StatsType SINK_LEVEL_STAT = 60;
        public readonly static StatsType ALT_TEXTURE_STAT = 61;
        public readonly static StatsType GUILD_NAME_STAT = 62; //Is UTF
        public readonly static StatsType GUILD_RANK_STAT = 63;
        public readonly static StatsType BREATH_STAT = 64;
        public readonly static StatsType XP_BOOSTED_STAT = 65;
        public readonly static StatsType XP_TIMER_STAT = 66;
        public readonly static StatsType LD_TIMER_STAT = 67;
        public readonly static StatsType LT_TIMER_STAT = 68;
        public readonly static StatsType HEALTH_POTION_STACK_STAT = 69;
        public readonly static StatsType MAGIC_POTION_STACK_STAT = 70;
        public readonly static StatsType BACKPACK_0_STAT = 71;
        public readonly static StatsType BACKPACK_1_STAT = 72;
        public readonly static StatsType BACKPACK_2_STAT = 73;
        public readonly static StatsType BACKPACK_3_STAT = 74;
        public readonly static StatsType BACKPACK_4_STAT = 75;
        public readonly static StatsType BACKPACK_5_STAT = 76;
        public readonly static StatsType BACKPACK_6_STAT = 77;
        public readonly static StatsType BACKPACK_7_STAT = 78;
        public readonly static StatsType HASBACKPACK_STAT = 79;
        public readonly static StatsType TEXTURE_STAT = 80;
        public readonly static StatsType PET_INSTANCEID_STAT = 81;
        public readonly static StatsType PET_NAME_STAT = 82; //Is UTF
        public readonly static StatsType PET_TYPE_STAT = 83;
        public readonly static StatsType PET_RARITY_STAT = 84;
        public readonly static StatsType PET_MAXABILITYPOWER_STAT = 85;
        public readonly static StatsType PET_FAMILY_STAT = 86; //This does do nothing in the client
        public readonly static StatsType PET_FIRSTABILITY_POINT_STAT = 87;
        public readonly static StatsType PET_SECONDABILITY_POINT_STAT = 88;
        public readonly static StatsType PET_THIRDABILITY_POINT_STAT = 89;
        public readonly static StatsType PET_FIRSTABILITY_POWER_STAT = 90;
        public readonly static StatsType PET_SECONDABILITY_POWER_STAT = 91;
        public readonly static StatsType PET_THIRDABILITY_POWER_STAT = 92;
        public readonly static StatsType PET_FIRSTABILITY_TYPE_STAT = 93;
        public readonly static StatsType PET_SECONDABILITY_TYPE_STAT = 94;
        public readonly static StatsType PET_THIRDABILITY_TYPE_STAT = 95;
        public readonly static StatsType NEW_CON_STAT = 96;
        public readonly static StatsType FORTUNE_TOKEN_STAT = 97;
        public readonly static StatsType ACCOUNT_TYPE = 98;
        public readonly static StatsType ADMIN = 99;
        public readonly static StatsType PET_OBJECT_ID = 100;
        public readonly static StatsType PET_HP_HEALING_AVERAGE_MIN = 101;
        public readonly static StatsType PET_HP_HEALING_AVERAGE_MAX = 102;
        public readonly static StatsType PET_HP_HEALING_AVERAGE_BONUS = 103;
        public readonly static StatsType PET_MP_HEALING_AVERAGE_MIN = 104;
        public readonly static StatsType PET_MP_HEALING_AVERAGE_MAX = 105;
        public readonly static StatsType PET_MP_HEALING_AVERAGE_BONUS = 106;
        public readonly static StatsType PET_ATTACK_COOLDOWN = 107;
        public readonly static StatsType PET_ATTACK_CHANCE = 108;
        public readonly static StatsType PET_ATTACK_DAMAGE_MIN = 109;
        public readonly static StatsType PET_ATTACK_DAMAGE_MAX = 110;
        public readonly static StatsType CHAR_LEVEL = 111;
        public readonly static StatsType CHAR_EXPERIENCE = 112;
        public readonly static StatsType CHAR_HEALTH_POINTS = 113;
        public readonly static StatsType CHAR_MAGIC_POINTS = 114;
        public readonly static StatsType CHAR_ATTACK_LEVEL = 115;
        public readonly static StatsType CHAR_ATTACK_EXPERIENCE = 116;
        public readonly static StatsType CHAR_DEFENSE_LEVEL = 117;
        public readonly static StatsType CHAR_DEFENSE_EXPERIENCE = 118;
        public readonly static StatsType CHAR_SPEED = 119;
        public readonly static StatsType CHAR_MAX_HEALTH_POINTS = 120;
        public readonly static StatsType CHAR_MAX_MAGIC_POINTS = 121;
        public readonly static StatsType CHAR_NEXT_EXPERIENCE = 122;
        public readonly static StatsType CHAR_NEXT_ATTACK_EXPERIENCE = 123;
        public readonly static StatsType CHAR_NEXT_DEFENSE_EXPERIENCE = 124;
        public readonly static StatsType CHAR_ARM = 125;
        public readonly static StatsType CHAR_ATTACK = 126;
        public readonly static StatsType CHAR_DEFENSE = 127;
        public readonly static StatsType INVENTORY_0_STAT = 128;
        public readonly static StatsType INVENTORY_1_STAT = 129;
        public readonly static StatsType INVENTORY_2_STAT = 130;
        public readonly static StatsType INVENTORY_3_STAT = 131;
        public readonly static StatsType INVENTORY_4_STAT = 132;
        public readonly static StatsType INVENTORY_5_STAT = 133;
        public readonly static StatsType INVENTORY_6_STAT = 134;
        public readonly static StatsType INVENTORY_7_STAT = 135;
        public readonly static StatsType INVENTORY_8_STAT = 136;
        public readonly static StatsType INVENTORY_9_STAT = 137;
        public readonly static StatsType INVENTORY_10_STAT = 138;
        public readonly static StatsType INVENTORY_11_STAT = 139;
        public readonly static StatsType INVENTORY_12_STAT = 140;
        public readonly static StatsType INVENTORY_13_STAT = 141;
        public readonly static StatsType INVENTORY_14_STAT = 142;
        public readonly static StatsType INVENTORY_15_STAT = 143;
        public readonly static StatsType INVENTORY_16_STAT = 144;
        public readonly static StatsType INVENTORY_17_STAT = 145;
        public readonly static StatsType INVENTORY_18_STAT = 146;
        public readonly static StatsType INVENTORY_19_STAT = 147;
        public readonly static StatsType INVENTORY_20_STAT = 148;
        public readonly static StatsType INVENTORY_21_STAT = 149;
        public readonly static StatsType INVENTORY_22_STAT = 150;
        public readonly static StatsType INVENTORY_23_STAT = 151;
        public readonly static StatsType INVENTORY_24_STAT = 152;
        public readonly static StatsType INVENTORY_25_STAT = 153;
        public readonly static StatsType INVENTORY_26_STAT = 154;

        // TODO.
        /*public readonly static StatsType CONTAINER_ITEM_DATA = 125; // utf (json)
        public readonly static StatsType CONTAINER_PRIORITY = 126;
        public readonly static StatsType CONTAINER_OWNER = 127;*/

        private byte _type;

        private StatsType(byte type)
        {
            _type = type;
        }

        internal static List<StatsType> UTF = new List<StatsType>
        {
            NAME_STAT,
            ACCOUNT_ID_STAT,
            OWNER_ACCOUNT_ID_STAT,
            GUILD_NAME_STAT,
            PET_NAME_STAT,
            CHAR_EXPERIENCE,
            CHAR_ATTACK_EXPERIENCE,
            CHAR_DEFENSE_EXPERIENCE,
            CHAR_NEXT_EXPERIENCE,
            CHAR_NEXT_ATTACK_EXPERIENCE,
            CHAR_NEXT_DEFENSE_EXPERIENCE
        };

        public bool IsUTF
            => UTF.Contains(this);

        public static string GetType<Any>(Any any)
        {
            string value = null;
            Type type = any.GetType();

            if (type == typeof(int))
                value = "Int";
            else if (type == typeof(string))
                value = "String";
            else if (type == typeof(float))
                value = "Float";
            else if (type == typeof(double))
                value = "Double";
            else if (type == typeof(bool))
                value = "Boolean";
            else
                value = "Unknown";

            return $"{value}{(value != null ? "\t(Not Null)" : "\t(Null)")}";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator StatsType(int type)
        {
            if (type > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return new StatsType((byte)type);
        }

        public static implicit operator StatsType(byte type) => new StatsType(type);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static bool operator ==(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return type._type == (byte)id;
        }

        public static bool operator ==(StatsType type, byte id) => type._type == id;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static bool operator !=(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return type._type != (byte)id;
        }

        public static bool operator !=(StatsType type, byte id) => type._type != id;

        public static bool operator ==(StatsType type, StatsType id) => type._type == id._type;

        public static bool operator !=(StatsType type, StatsType id) => type._type != id._type;

        public static implicit operator int(StatsType type) => type._type;

        public static implicit operator byte(StatsType type) => type._type;

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj)
        {
            if (!(obj is StatsType)) return false;
            return this == (StatsType)obj;
        }

        public override string ToString() => _type.ToString();
    }
}