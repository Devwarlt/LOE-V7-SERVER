#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using log4net;
using LoESoft.Core.models;

#endregion

namespace LoESoft.Core
{
    public class EmbeddedData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmbeddedData));

        private readonly XElement addition;

        private int LoEObjectAmount = 0;
        private int EggAmount = 0;
        private int EnemyAmount = 0;
        private int ProjectileAmount = 0;
        private int GroundAmount = 0;
        private int EquipmentAmount = 0;
        private int PetAmount = 0;
        private int SkinAmount = 0;

        private readonly Dictionary<string, int> id2type_obj;
        private readonly Dictionary<string, ushort> id2type_tile;
        private readonly Dictionary<int, Item> items;
        private readonly Dictionary<int, ObjectDesc> objDescs;
        private readonly Dictionary<int, PortalDesc> portals;
        private readonly Dictionary<ushort, TileDesc> tiles;
        private readonly Dictionary<int, XElement> type2elem_obj;
        private readonly Dictionary<ushort, XElement> type2elem_tile;
        private readonly Dictionary<string, PetSkin> id2pet_skin;
        private readonly Dictionary<ushort, PetStruct> type2pet;
        private readonly Dictionary<ushort, string> type2id_obj;
        private readonly Dictionary<ushort, string> type2id_tile;
        private readonly Dictionary<ushort, SetTypeSkin> setTypeSkins;

        public IDictionary<int, XElement> ObjectTypeToElement
        { get; private set; }

        public IDictionary<ushort, string> ObjectTypeToId
        { get; private set; }

        public IDictionary<string, int> IdToObjectType
        { get; private set; }

        public IDictionary<ushort, XElement> TileTypeToElement
        { get; private set; }

        public IDictionary<ushort, string> TileTypeToId
        { get; private set; }

        public IDictionary<string, ushort> IdToTileType
        { get; private set; }

        public IDictionary<ushort, TileDesc> Tiles
        { get; private set; }

        public IDictionary<int, Item> Items
        { get; private set; }

        public IDictionary<int, ObjectDesc> ObjectDescs
        { get; private set; }

        public IDictionary<int, PortalDesc> Portals
        { get; private set; }

        public IDictionary<ushort, PetStruct> TypeToPet
        { get; private set; }

        public IDictionary<string, PetSkin> IdToPetSkin
        { get; private set; }

        public IDictionary<ushort, SetTypeSkin> SetTypeSkins
        { get; private set; }

        private string[] addXml;
        private int prevUpdateCount = -1;
        private int updateCount;

        public EmbeddedData(string path = "assets/xmls")
        {
            bool loaded = false;

            ObjectTypeToElement = new ReadOnlyDictionary<int, XElement>(type2elem_obj = new Dictionary<int, XElement>());
            ObjectTypeToId = new ReadOnlyDictionary<ushort, string>(type2id_obj = new Dictionary<ushort, string>());
            IdToObjectType = new ReadOnlyDictionary<string, int>(id2type_obj = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase));
            TileTypeToElement = new ReadOnlyDictionary<ushort, XElement>(type2elem_tile = new Dictionary<ushort, XElement>());
            TileTypeToId = new ReadOnlyDictionary<ushort, string>(type2id_tile = new Dictionary<ushort, string>());
            IdToTileType = new ReadOnlyDictionary<string, ushort>(id2type_tile = new Dictionary<string, ushort>(StringComparer.InvariantCultureIgnoreCase));
            Tiles = new ReadOnlyDictionary<ushort, TileDesc>(tiles = new Dictionary<ushort, TileDesc>());
            Items = new artic.ReadOnlyDictionary<int, Item>(items = new Dictionary<int, Item>());
            ObjectDescs = new ReadOnlyDictionary<int, ObjectDesc>(objDescs = new Dictionary<int, ObjectDesc>());
            Portals = new ReadOnlyDictionary<int, PortalDesc>(portals = new Dictionary<int, PortalDesc>());
            TypeToPet = new ReadOnlyDictionary<ushort, PetStruct>(type2pet = new Dictionary<ushort, PetStruct>());
            IdToPetSkin = new ReadOnlyDictionary<string, PetSkin>(id2pet_skin = new Dictionary<string, PetSkin>());
            SetTypeSkins = new ReadOnlyDictionary<ushort, SetTypeSkin>(setTypeSkins = new Dictionary<ushort, SetTypeSkin>());

            addition = new XElement("Additions");

            string basePath = Path.Combine(AssemblyDirectory, path);
            string[] xmls = Directory.EnumerateFiles(basePath, "*.xml", SearchOption.AllDirectories).ToArray();
            try
            {
                for (int i = 0; i < xmls.Length; i++)
                {
                    using (Stream stream = File.OpenRead(xmls[i]))
                        ProcessXml(XElement.Load(stream));
                    if (i + 1 == xmls.Length)
                        loaded = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            if (!loaded)
                return;

            Log._("assets", xmls.Length, false, "Loaded ", " ", ".");
            Log._($"custom object{(LoEObjectAmount > 1 ? "s" : "")} of {objDescs.Count} object{(objDescs.Count > 1 ? "s" : "")}", LoEObjectAmount, false);
            Log._($"custom egg", EggAmount);
            Log._($"custom enem{(EnemyAmount > 1 ? "ies" : "y")}", EnemyAmount, false);
            Log._($"custom projectile", ProjectileAmount);
            Log._($"custom ground", GroundAmount);
            Log._($"custom equipment", EquipmentAmount);
            Log._($"custom pet", PetAmount);
            Log._($"custom skin", SkinAmount);
            Log._($"portal", portals.Count);
            Log._($"special themed item", setTypeSkins.Count);
        }

        private static string AssemblyDirectory
        { get { return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); } }

        public string[] AdditionXml
        {
            get
            {
                UpdateXml();
                return addXml;
            }
        }

        public void AddSetTypes(XElement root)
        {
            foreach (XElement elem in root.XPathSelectElements("//EquipmentSet"))
            {
                string id = elem.Attribute("id").Value;
                ushort type;
                XAttribute typeAttr = elem.Attribute("type");
                type = (ushort)Utils.FromString(typeAttr.Value);
                setTypeSkins[type] = new SetTypeSkin(elem, type);
                XAttribute extAttr = elem.Attribute("ext");
                if (extAttr != null && bool.TryParse(extAttr.Value, out bool ext) && ext)
                {
                    addition.Add(elem);
                    updateCount++;
                }
            }
        }

        /* LoE Object:
         * - element 'File' of element 'AnimatedTexture' contains: TUK or
         * element 'File' of element 'Texture' contains: TUK
         */
        private bool IsLoEObject(XElement e)
        {
            if (e.Element("AnimatedTexture") != null)
                return e.Element("AnimatedTexture").Element("File").Value.ToLower().StartsWith("tuk");
            if (e.Element("Texture") != null)
                return e.Element("Texture").Element("File").Value.ToLower().StartsWith("tuk");
            return false;
        }

        /* Eggs:
         * - is LoE Object
         * - attribute 'id' contains: lcp egg
         * - attribute 'successChance'
         * - attribute 'minStars'
         * - element 'SlotType' is: 9000
         * - element 'Activate' is: SpecialPet
         */
        private bool IsEgg(XElement e) =>
            IsLoEObject(e)
            && e.Attribute("id").Value.Contains("lcp egg")
            && e.Attribute("successChance") != null
            && e.Attribute("minStars") != null
            && Utils.FromString(e.Element("SlotType").Value) == 9000
            && e.Element("Activate").Value == "SpecialPet";

        /* Enemies:
         * - is LoE Object
         * - element 'Enemy'
         * - element 'Class' is: Character
         */
        private bool IsEnemy(XElement e) =>
           IsLoEObject(e)
           && e.Element("Enemy") != null
           && e.Element("Class").Value == "Character";

        /* Projectiles:
         * - is LoE Object
         * - element 'Class' is: Projectile
         */
        private bool IsProjectile(XElement e) =>
           IsLoEObject(e)
           && e.Element("Class").Value == "Projectile";

        /* Grounds:
         * - is LoE Object
         * - element 'Ground'
         */
        private bool IsGround(XElement e) =>
           IsLoEObject(e);

        /* Equipments:
         * - is LoE Object
         * - element 'Class' is: Equipment
         * - element 'Item'
         */
        private bool IsEquipment(XElement e) =>
           IsLoEObject(e)
           && e.Element("Class").Value == "Equipment"
           && e.Element("Item") != null;

        /* Pets:
         * - is LoE Object
         * - element 'NewPet'
         * - element 'Class' is: Character
         */
        private bool IsPet(XElement e) =>
           IsLoEObject(e)
           && e.Element("NewPet") != null
           && e.Element("Class").Value == "Character";

        /* Skins:
         * - is LoE Object
         * - element 'Skin'
         */
        private bool IsSkin(XElement e) =>
           IsLoEObject(e)
           && e.Element("Skin") != null;

        public void AddObjects(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements("//Object"))
            {
                if (e.Element("Class") == null) continue;
                string cls = e.Element("Class").Value;
                string id = e.Attribute("id").Value;

                if (IsEgg(e))
                {
                    LoEObjectAmount++;
                    EggAmount++;
                }

                if (IsEnemy(e))
                {
                    LoEObjectAmount++;
                    EnemyAmount++;
                }

                if (IsProjectile(e))
                {
                    LoEObjectAmount++;
                    ProjectileAmount++;
                }

                if (IsEquipment(e))
                {
                    LoEObjectAmount++;
                    EquipmentAmount++;
                }

                if (IsPet(e))
                {
                    LoEObjectAmount++;
                    PetAmount++;
                }

                if (IsSkin(e))
                {
                    LoEObjectAmount++;
                    SkinAmount++;
                }

                ushort type;
                XAttribute typeAttr = e.Attribute("type");
                type = (ushort)Utils.FromString(typeAttr.Value);

                if (cls == "PetBehavior" || cls == "PetAbility") continue;

                if (type2id_obj.ContainsKey(type))
                    Log.Warn($"'{id}' and '{type2id_obj[type]}' has the same ID of {type}!");
                if (id2type_obj.ContainsKey(id))
                    Log.Warn($"{type} and {id2type_obj[id]} has the same name of {id}!");

                type2id_obj[type] = id;
                id2type_obj[id] = type;
                type2elem_obj[type] = e;

                switch (cls)
                {
                    case "Equipment":
                    case "Dye":
                        items[type] = new Item(type, e);
                        break;
                    case "Portal":
                    case "GuildHallPortal":
                        try
                        {
                            portals[type] = new PortalDesc(type, e);
                        }
                        catch
                        {
                            Console.WriteLine("Error for portal: " + type + " id: " + id);
                            /*3392,1792,1795,1796,1805,1806,1810,1825 -- no location, assume nexus?* 
        *  Tomb Portal of Cowardice,  Dungeon Portal,  Portal of Cowardice,  Realm Portal,  Glowing Portal of Cowardice,  Glowing Realm Portal,  Nexus Portal,  Locked Wine Cellar Portal*/
                        }
                        break;
                    case "Pet":
                        type2pet[type] = new PetStruct(type, e);
                        break;
                    case "PetSkin":
                        id2pet_skin[id] = new PetSkin(type, e);
                        break;
                    case "PetBehavior":
                    case "PetAbility":
                        break;
                    default:
                        objDescs[type] = new ObjectDesc(type, e);
                        break;
                }

                XAttribute extAttr = e.Attribute("ext");
                if (extAttr != null && bool.TryParse(extAttr.Value, out bool ext) && ext)
                {
                    if (e.Attribute("type") == null)
                        e.Add(new XAttribute("type", type));
                    addition.Add(e);
                    updateCount++;
                }
            }
        }

        public void AddGrounds(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements("//Ground"))
            {
                if (IsGround(e))
                {
                    LoEObjectAmount++;
                    GroundAmount++;
                }

                string id = e.Attribute("id").Value;

                ushort type;
                XAttribute typeAttr = e.Attribute("type");
                type = (ushort)Utils.FromString(typeAttr.Value);

                if (type2id_tile.ContainsKey(type))
                    Log.Warn($"'{id}' and '{type2id_tile[type]}' has the same ID of {type}!");
                if (id2type_tile.ContainsKey(id))
                    Log.Warn($"{type} and {id2type_tile[id]} has the same name of {id}!");

                type2id_tile[type] = id;
                id2type_tile[id] = type;
                type2elem_tile[type] = e;

                tiles[type] = new TileDesc(type, e);

                XAttribute extAttr = e.Attribute("ext");
                if (extAttr != null && bool.TryParse(extAttr.Value, out bool ext) && ext)
                {
                    addition.Add(e);
                    updateCount++;
                }
            }
        }

        private void ProcessXml(XElement root)
        {
            AddObjects(root);
            AddGrounds(root);
            AddSetTypes(root);
        }

        private void UpdateXml()
        {
            if (prevUpdateCount != updateCount)
            {
                addXml = new[] { addition.ToString() };
                prevUpdateCount = updateCount;
            }
        }

        private class AutoAssign : LoESoft.Core.AutoAssign
        {
            private EmbeddedData dat;
            private int nextFullId;
            private int nextSignedId;

            internal AutoAssign(EmbeddedData dat)
                : base("autoId")
            {
                this.dat = dat;
                nextSignedId = GetValue<int>("nextSigned", "50000"); //0xC350
                nextFullId = GetValue<int>("nextFull", "58000"); //0xE290
            }

            public int Assign(string id, XElement elem)
            {
                int type = GetValue<int>(id, "0");
                if (type == 0)
                {
                    XElement cls = elem.Element("Class");
                    bool isFull = cls.Value == "Dye" || (cls.Value == "Equipment" && !elem.Elements("Projectile").Any());
                    if (isFull)
                    {
                        type = nextFullId++;
                        SetValue("nextFull", nextFullId.ToString());
                    }
                    else
                    {
                        type = nextSignedId++;
                        SetValue("nextSigned", nextSignedId.ToString());
                    }
                    SetValue(id, type.ToString());

                    Log.Info($"Auto assigned '{id}' to 0x{type:x4}");
                }
                return type;
            }
        }
    }
}