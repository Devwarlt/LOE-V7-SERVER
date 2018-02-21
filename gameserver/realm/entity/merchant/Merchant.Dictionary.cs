using System;
using System.Collections.Generic;

namespace LoESoft.GameServer.realm.entity.merchant
{
    partial class Merchant
    {
        internal static class BLACKLIST
        {
            internal static readonly int[] keys =
            {
                1897, 12288, 12289, 12290, 29035, 3466, 538, 887, 2996, 2998, 1601, 2355, 5705, 3285, 1544, 1563, 1584, 1576, 3311,
                3133, 8848, 28645
            };

            internal static readonly string[] eggs =
            {
                ""
            };

            internal static readonly string[] weapons =
            {
                "Bow of Eternal Frost",
                "Frostbite",
                "Present Dispensing Wand",
                "An Icicle",
                "Staff of Yuletide Carols",
                "Salju"
            };

            internal static readonly string[] small =
            {
                "Small Ivory Dragon Scale Cloth",
                "Small Green Dragon Scale Cloth",
                "Small Midnight Dragon Scale Cloth",
                "Small Blue Dragon Scale Cloth",
                "Small Red Dragon Scale Cloth",
                "Small Jester Argyle Cloth",
                "Small Alchemist Cloth",
                "Small Mosaic Cloth",
                "Small Spooky Cloth",
                "Small Flame Cloth",
                "Small Heavy Chainmail Cloth"
            };

            internal static readonly string[] large =
            {
                "Large Ivory Dragon Scale Cloth",
                "Large Green Dragon Scale Cloth",
                "Large Midnight Dragon Scale Cloth",
                "Large Blue Dragon Scale Cloth",
                "Large Red Dragon Scale Cloth",
                "Large Jester Argyle Cloth",
                "Large Alchemist Cloth",
                "Large Mosaic Cloth",
                "Large Spooky Cloth",
                "Large Flame Cloth",
                "Large Heavy Chainmail Cloth"
            };
        }

        public static readonly Dictionary<int, Tuple<int, CurrencyType>> prices = new Dictionary<int, Tuple<int, CurrencyType>>
        {
            #region "Region 1 & 2"
            { 1793, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Undead Lair Key
            { 308, new Tuple<int, CurrencyType>(250, CurrencyType.Gold) }, // Halloween Cemetery Key
            { 1797, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Pirate Cave Key
            { 1798, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Spider Den Key
            { 1802, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Abyss of Demons Key
            { 1803, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Snake Pit Key
            { 1808, new Tuple<int, CurrencyType>(200, CurrencyType.Gold) }, // Tomb of the Ancients Key
            { 1823, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Sprite World Key
            { 3089, new Tuple<int, CurrencyType>(200, CurrencyType.Gold) }, // Ocean Trench Key
            { 3097, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Totem Key
            { 29836, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Ice Cave Key
            { 3107, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Manor Key
            { 3118, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Davy's Key
            { 3119, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Lab Key
            { 3170, new Tuple<int, CurrencyType>(200, CurrencyType.Gold) }, // Candy Key
            { 3183, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Cemetery Key
            { 3284, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Draconis Key
            { 3277, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Forest Maze Key
            { 3279, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Woodland Labyrinth Key
            { 3278, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Deadwater Docks Key
            { 3290, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // The Crawling Depths Key
            { 3293, new Tuple<int, CurrencyType>(200, CurrencyType.Gold) }, // Shatters Key
            { 8852, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Shaitan's Key
            { 9042, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Theatre Key
            { 29804, new Tuple<int, CurrencyType>(200, CurrencyType.Gold) }, // Puppet Master's Encore Key
            { 573, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Toxic Sewers Key
            { 283, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // The Hive Key
            { 32695, new Tuple<int, CurrencyType>(250, CurrencyType.Gold) }, // Ice Tomb Key
            { 303, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Mountain Temple Key
            #endregion "Region 1 & 2"
            #region "Region 4"
            { 3273, new Tuple<int, CurrencyType>(20, CurrencyType.Gold) }, // Soft Drink
            { 3275, new Tuple<int, CurrencyType>(50, CurrencyType.Gold) }, // Fries
            { 3270, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Great Taco
            { 3269, new Tuple<int, CurrencyType>(150, CurrencyType.Gold) }, // Power Pizza
            { 3268, new Tuple<int, CurrencyType>(240, CurrencyType.Gold) }, // Chocolate Cream Sandwich Cookie
            { 3274, new Tuple<int, CurrencyType>(330, CurrencyType.Gold) }, // Grapes of Wrath
            { 3272, new Tuple<int, CurrencyType>(450, CurrencyType.Gold) }, // Superburger
            { 3271, new Tuple<int, CurrencyType>(700, CurrencyType.Gold) }, // Double Cheeseburger Deluxe
            { 3276, new Tuple<int, CurrencyType>(1000, CurrencyType.Gold) }, // Ambrosia
            { 3280, new Tuple<int, CurrencyType>(40, CurrencyType.Gold) }, // Cranberries
            { 3281, new Tuple<int, CurrencyType>(60, CurrencyType.Gold) }, // Ear of Corn
            { 3282, new Tuple<int, CurrencyType>(90, CurrencyType.Gold) }, // Sliced Yam
            { 3283, new Tuple<int, CurrencyType>(120, CurrencyType.Gold) }, // Pumpkin Pie
            { 3286, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) } // Thanksgiving Turkey
            #endregion "Region 4"
        };
    }
}