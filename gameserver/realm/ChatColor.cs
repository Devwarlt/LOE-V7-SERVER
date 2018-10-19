using System.Collections.Generic;
using System.Linq;

namespace LoESoft.GameServer.realm
{
    /// <summary>
    /// Color-Hex:
    /// http://www.color-hex.com/
    ///
    /// FlexTool:
    /// http://www.flextool.com.br/tabela_cores.html
    /// </summary>
    public class ChatColor
    {
        private int _accountType { get; set; }
        private int _stars { get; set; }

        public ChatColor(int stars, int accountType)
        {
            _stars = stars;
            _accountType = accountType;
        }

        public int GetColor()
        {
            if (_accountType > 2)
            {
                int color = -1;
                if (specialColors.TryGetValue(_accountType, out color))
                    return color;
            }
            else
            {
                foreach (KeyValuePair<IEnumerable<int>, int> i in regularColors)
                    if (i.Key.Contains(_stars))
                        return i.Value;
            }
            return 0x123456;
        }

        private readonly Dictionary<IEnumerable<int>, int> regularColors = new Dictionary<IEnumerable<int>, int>
        {
            { Enumerable.Range(0, 13), 0x8997DD },
            { Enumerable.Range(14, 27), 0x304CDA },
            { Enumerable.Range(28, 41), 0xC0262C },
            { Enumerable.Range(42, 55), 0xF6921D },
            { Enumerable.Range(56, 69), 0xFFFF00 },
            { Enumerable.Range(70, 70), 0xFFFFFF }
        };

        private readonly Dictionary<int, int> specialColors = new Dictionary<int, int>
        {
            { 3, 0x6CFFB1 },
            { 4, 0xE52B50 }
        };
    }
}