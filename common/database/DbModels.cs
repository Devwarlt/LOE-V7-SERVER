#region

using BookSleeve;
using LoESoft.Core.config;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace LoESoft.Core
{
    #region RedisObject

    public abstract class RedisObject
    {
        private ConcurrentDictionary<string, KeyValuePair<byte[], bool>> fields;

        protected void Init(Database db, string key)
        {
            Key = key;
            Database = db;
            fields =
                new ConcurrentDictionary<string, KeyValuePair<byte[], bool>>(
                    db
                    .Hashes
                    .GetAll(0, key)
                    .Exec()
                    .ToDictionary(
                        x => x.Key,
                        x => new KeyValuePair<byte[], bool>(x.Value, false)
                    )
                );
        }

        public Database Database { get; private set; }
        public string Key { get; private set; }
        public IEnumerable<string> AllKeys => fields.Keys;
        public bool IsNull => fields.Count == 0;

        protected T GetValue<T>(string key, T def = default(T))
        {
            if (!fields.TryGetValue(key, out KeyValuePair<byte[], bool> val))
                return def;
            if (typeof(T) == typeof(int))
                return (T)(object)int.Parse(Encoding.UTF8.GetString(val.Key));
            else if (typeof(T) == typeof(ushort))
                return (T)(object)ushort.Parse(Encoding.UTF8.GetString(val.Key));
            else if (typeof(T) == typeof(bool))
                return (T)(object)(val.Key[0] != 0);
            else if (typeof(T) == typeof(DateTime))
                return (T)(object)DateTime.FromBinary(BitConverter.ToInt64(val.Key, 0));
            else if (typeof(T) == typeof(byte[]))
                return (T)(object)val.Key;
            else if (typeof(T) == typeof(ushort[]))
            {
                ushort[] ret = new ushort[val.Key.Length / 2];
                Buffer.BlockCopy(val.Key, 0, ret, 0, val.Key.Length);
                return (T)(object)ret;
            }
            else if (typeof(T) == typeof(int[]))
            {
                int[] ret = new int[val.Key.Length / 4];
                Buffer.BlockCopy(val.Key, 0, ret, 0, val.Key.Length);
                return (T)(object)ret;
            }
            else if (typeof(T) == typeof(string))
                return (T)(object)Encoding.UTF8.GetString(val.Key);
            else
                throw new NotSupportedException();
        }

        protected void SetValue<T>(string key, T val)
        {
            byte[] buff;
            if (typeof(T) == typeof(int) || typeof(T) == typeof(ushort) ||
                typeof(T) == typeof(string))
                buff = Encoding.UTF8.GetBytes(val.ToString());
            else if (typeof(T) == typeof(bool))
                buff = new byte[] { (byte)((bool)(object)val ? 1 : 0) };
            else if (typeof(T) == typeof(DateTime))
                buff = BitConverter.GetBytes(((DateTime)(object)val).ToBinary());
            else if (typeof(T) == typeof(byte[]))
                buff = (byte[])(object)val;
            else if (typeof(T) == typeof(ushort[]))
            {
                var v = (ushort[])(object)val;
                buff = new byte[v.Length * 2];
                Buffer.BlockCopy(v, 0, buff, 0, buff.Length);
            }
            else if (typeof(T) == typeof(int[]))
            {
                var v = (int[])(object)val;
                buff = new byte[v.Length * 4];
                Buffer.BlockCopy(v, 0, buff, 0, buff.Length);
            }
            else
                throw new NotSupportedException();
            fields[key] = new KeyValuePair<byte[], bool>(buff, true);
        }

        private Dictionary<string, byte[]> update;

        public void Flush()
        {
            if (update == null)
                update = new Dictionary<string, byte[]>();
            else
                update.Clear();

            foreach (var i in fields)
                if (i.Value.Value)
                    update.Add(i.Key, i.Value.Key);

            Database.Hashes.Set(0, Key, update);
        }

        public void Flush(RedisConnection conn = null)
        {
            if (update == null) update = new Dictionary<string, byte[]>();
            else update.Clear();
            foreach (var i in fields)
                if (i.Value.Value)
                    update.Add(i.Key, i.Value.Key);
            (conn ?? Database).Hashes.Set(0, Key, update);
        }

        public void Reload()    //Discard all updates
        {
            if (update != null)
                update.Clear();

            fields =
                new ConcurrentDictionary<string, KeyValuePair<byte[], bool>>(
                    Database
                    .Hashes
                    .GetAll(0, Key)
                    .Exec()
                    .ToDictionary(
                        x => x.Key,
                        x => new KeyValuePair<byte[], bool>(x.Value, false)
                    )
                );
        }
    }

    #endregion

    public class DbLoginInfo
    {
        private Database Db { get; set; }

        internal DbLoginInfo(Database db, string uuid)
        {
            Db = db;
            UUID = uuid;
            var json = db.Hashes.GetString(0, "logins", uuid.ToUpperInvariant()).Exec();
            if (json == null)
                IsNull = true;
            else
                JsonConvert.PopulateObject(json, this);
        }

        [JsonIgnore]
        public string UUID { get; private set; }

        [JsonIgnore]
        public bool IsNull { get; private set; }

        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public string AccountId { get; set; }

        public void Flush()
        {
            Db.Hashes.Set(0, "logins", UUID.ToUpperInvariant(), JsonConvert.SerializeObject(this));
        }
    }

    public class DbAccount : RedisObject
    {
        internal DbAccount(Database db, string accId)
        {
            AccountId = accId;
            Init(db, "account." + accId);
        }

        public DateTime AccountLifetime
        {
            get { return GetValue("accountLifetime", DateTime.MinValue); }
            set { SetValue("accountLifetime", value); }
        }

        public int AccountType
        {
            get { return GetValue("accountType", (int)config.AccountType.FREE_ACCOUNT); }
            set { SetValue("accountType", value); }
        }

        public string AccountId { get; private set; }

        internal string LockToken { get; set; }

        public string UUID
        {
            get { return GetValue<string>("uuid"); }
            set { SetValue("uuid", value); }
        }

        public string Name
        {
            get { return GetValue<string>("name"); }
            set { SetValue("name", value); }
        }

        public bool Admin
        {
            get { return GetValue("admin", false); }
            set { SetValue("admin", value); }
        }

        public bool MapEditor
        {
            get { return GetValue("mapEditor", false); }
            set { SetValue("mapEditor", value); }
        }

        public bool NameChosen
        {
            get { return GetValue("nameChosen", false); }
            set { SetValue("nameChosen", value); }
        }

        public bool Verified
        {
            get { return GetValue("verified", Settings.STARTUP.VERIFIED); }
            set { SetValue("verified", value); }
        }

        public bool Converted
        {
            get { return GetValue("converted", false); }
            set { SetValue("converted", value); }
        }

        public string GuildId
        {
            get { return GetValue("guildId", "-1"); }
            set { SetValue("guildId", value); }
        }

        public int GuildRank
        {
            get { return GetValue<int>("guildRank"); }
            set { SetValue("guildRank", value); }
        }

        public int VaultCount
        {
            get { return GetValue<int>("vaultCount"); }
            set { SetValue("vaultCount", value); }
        }

        public int MaxCharSlot
        {
            get { return GetValue("maxCharSlot", Settings.STARTUP.MAX_CHAR_SLOTS); }
            set { SetValue("maxCharSlot", value); }
        }

        public DateTime RegTime
        {
            get { return GetValue<DateTime>("regTime"); }
            set { SetValue("regTime", value); }
        }

        public bool Guest
        {
            get { return GetValue("guest", false); }
            set { SetValue("guest", value); }
        }

        public int Credits
        {
            get { return GetValue("credits", Settings.STARTUP.GOLD); }
            set { SetValue("credits", value); }
        }

        public int EmpiresCoin
        {
            get { return GetValue("empiresCoin", Settings.STARTUP.EMPIRES_COIN); }
            set { SetValue("empiresCoin", value); }
        }

        public int NextCharId
        {
            get { return GetValue<int>("nextCharId"); }
            set { SetValue("nextCharId", value); }
        }

        public int[] Gifts
        {
            get { return GetValue<int[]>("gifts"); }
            set { SetValue("gifts", value); }
        }

        public int IsAgeVerified
        {
            get { return GetValue("isAgeVerified", Settings.STARTUP.IS_AGE_VERIFIED); }
            set { SetValue("isAgeVerified", value); }
        }

        public int[] PurchasedPackages
        {
            get { return GetValue<int[]>("purchasedPackages"); }
            set { SetValue("purchasedPackages", value); }
        }

        public int[] PurchasedBoxes
        {
            get { return GetValue<int[]>("PurchasedBoxes"); }
            set { SetValue("PurchasedBoxes", value); }
        }

        public string AuthToken
        {
            get { return GetValue<string>("authToken"); }
            set { SetValue("authToken", value); }
        }

        public bool Muted
        {
            get { return GetValue("muted", false); }
            set { SetValue("muted", value); }
        }

        public bool Banned
        {
            get { return GetValue("banned", false); }
            set { SetValue("banned", value); }
        }

        public int[] Locked
        {
            get { return GetValue<int[]>("locked"); }
            set { SetValue("locked", value); }
        }

        public int[] Ignored
        {
            get { return GetValue<int[]>("ignored"); }
            set { SetValue("ignored", value); }
        }
    }

    public class DbChar : RedisObject
    {
        public DbAccount Account
        { get; private set; }

        public int CharId
        { get; private set; }

        internal DbChar(DbAccount acc, int charId)
        {
            Account = acc;
            CharId = charId;
            Init(acc.Database, "char." + acc.AccountId + "." + charId);
        }

        public int Vocation
        {
            get { return GetValue("vocation", Settings.NewCharacter.VocationType); }
            set { SetValue("vocation", value); }
        }

        public int Level
        {
            get { return GetValue("level", Settings.NewCharacter.InitialLevel); }
            set { SetValue("level", value); }
        }

        public double Experience
        {
            get { return GetValue("experience", Settings.NewCharacter.InitialExperience); }
            set { SetValue("experience", value); }
        }

        public int MaxHP
        {
            get { return GetValue("max_hp", Settings.NewCharacter.InitialHealthPoints); }
            set { SetValue("max_hp", value); }
        }

        public int MaxMP
        {
            get { return GetValue("max_mp", Settings.NewCharacter.InitialMagicPoints); }
            set { SetValue("max_mp", value); }
        }

        public int HP
        {
            get { return GetValue("hp", Settings.NewCharacter.InitialHealthPoints); }
            set { SetValue("hp", value); }
        }

        public int MP
        {
            get { return GetValue("mp", Settings.NewCharacter.InitialMagicPoints); }
            set { SetValue("mp", value); }
        }

        public int Speed
        {
            get { return GetValue("speed", Settings.NewCharacter.InitialSpeedBase); }
            set { SetValue("speed", value); }
        }

        public int Attack
        {
            get { return GetValue("attack", Settings.NewCharacter.InitialAttackLevel); }
            set { SetValue("attack", value); }
        }

        public int Defense
        {
            get { return GetValue("defense", Settings.NewCharacter.InitialDefenseLevel); }
            set { SetValue("defense", value); }
        }

        public double AttackExp
        {
            get { return GetValue("attack_exp", Settings.NewCharacter.InitialAttackExperience); }
            set { SetValue("attack_exp", value); }
        }

        public double DefenseExp
        {
            get { return GetValue("defense_exp", Settings.NewCharacter.InitialDefenseExperience); }
            set { SetValue("defense_exp", value); }
        }

        public int Outfit
        {
            get { return GetValue("outfit", Settings.NewCharacter.InitialOutfit); }
            set { SetValue("outfit", value); }
        }

        public int[] Equipments
        {
            get { return GetValue<int[]>("items"); }
            set { SetValue("items", value); }
        }
    }

    public class DbGuild : RedisObject
    {
        public DbAccount AccountId { get; private set; }
        public int Id { get; private set; }

        internal DbGuild(Database db, int id)
        {

            Id = id;
            Init(db, "guild." + id);
        }

        public DbGuild(DbAccount acc)
        {
            Id = Convert.ToInt32(acc.GuildId);
            Init(acc.Database, "guild." + Id);
        }

        public string Name
        {
            get { return GetValue<string>("name"); }
            set { SetValue("name", value); }
        }

        public int Level
        {
            get { return GetValue<int>("level"); }
            set { SetValue("level", value); }
        }

        public int Fame
        {
            get { return GetValue<int>("fame"); }
            set { SetValue("fame", value); }
        }

        public int TotalFame
        {
            get { return GetValue<int>("totalFame"); }
            set { SetValue("totalFame", value); }
        }

        public int[] Members
        {
            get { return GetValue<int[]>("members"); }
            set { SetValue("members", value); }
        }

        public string Board
        {
            get { return GetValue<string>("board") ?? ""; }
            set { SetValue("board", value); }
        }
    }

    public class DbVault : RedisObject
    {
        public DbAccount Account { get; private set; }

        public DbVault(DbAccount acc)
        {
            Account = acc;
            Init(acc.Database, "vault." + acc.AccountId);
        }

        public int[] this[int index]
        {
            get { return GetValue<int[]>("vault." + index); }
            set { SetValue("vault." + index, value); }
        }
    }
}