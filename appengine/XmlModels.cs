#region

using LoESoft.Core;
using LoESoft.Core.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using LoESoft.AppEngine.account;

#endregion

namespace LoESoft.AppEngine
{
    class GuildMember
    {
        public string name;
        public int rank;
#pragma warning disable CS0649 // Field 'GuildMember.lastSeen' is never assigned to, and will always have its default value 0
        public int lastSeen;
#pragma warning restore CS0649 // Field 'GuildMember.lastSeen' is never assigned to, and will always have its default value 0

        public static GuildMember FromDb(DbAccount acc)
        {
            return new GuildMember()
            {
                name = acc.Name,
                rank = acc.GuildRank
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("Member",
                    new XElement("Name", name),
                    new XElement("Rank", rank)
                    );
        }
    }

    class Guild
    {
        public int id;
        public string name;
        public int currentFame;
        public int totalFame;
        public string hallType;
        public List<GuildMember> members;

        public static Guild FromDb(Database db, DbGuild guild)
        {
            var members = (from member in guild.Members
                           select db.GetAccountById(Convert.ToString(member)) into acc
                           where acc != null
                           orderby acc.GuildRank descending,
                                   acc.Name ascending
                           select GuildMember.FromDb(acc)).ToList();

            return new Guild()
            {
                id = guild.Id,
                name = guild.Name,
                currentFame = guild.Fame,
                totalFame = guild.TotalFame,
                hallType = "Guild Hall " + guild.Level,
                members = members
            };
        }

        public XElement ToXml()
        {
            var guild = new XElement("Guild");
            guild.Add(new XAttribute("id", id));
            guild.Add(new XAttribute("name", name));
            guild.Add(new XElement("TotalFame", totalFame));
            guild.Add(new XElement("CurrentFame", currentFame));
            guild.Add(new XElement("HallType", hallType));
            foreach (var member in members)
                guild.Add(member.ToXml());

            return guild;
        }
    }

    class GuildIdentity
    {
        public int id;
        public string name;
        public int rank;

        public static GuildIdentity FromDb(DbAccount acc, DbGuild guild)
        {
            return new GuildIdentity()
            {
                id = guild.Id,
                name = guild.Name,
                rank = acc.GuildRank
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("Guild",
                    new XAttribute("id", id),
                    new XElement("Name", name),
                    new XElement("Rank", rank)
                );
        }
    }

    internal class Vault
    {
        private int[][] Chests { get; set; }

        public int[] this[int index] => Chests[index];

        public static Vault FromDb(DbAccount acc, DbVault vault)
        {
            return new Vault()
            {
                Chests = Enumerable.Range(1, acc.VaultCount).
                            Select(x => vault[x] ??
                                Enumerable.Repeat(-1, 8).ToArray()).ToArray()
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("Vault",
                    Chests.Select(x => new XElement("Chest", x.ToCommaSepString()))
                );
        }
    }

    internal class Account
    {
        public int EmpiresCoin { get; private set; }
        public string AccountId { get; private set; }
        public int AccountType { get; private set; }
        public DateTime AccountLifetime { get; private set; }
        public string Name { get; set; }

        public bool NameChosen { get; private set; }
        public bool Converted { get; private set; }
        public bool Admin { get; private set; }
        public bool MapEditor { get; private set; }
        public bool VerifiedEmail { get; private set; }

        public int Credits { get; private set; }
        public int NextCharSlotPrice { get; private set; }
        public uint BeginnerPackageTimeLeft { get; private set; }

        public int[] Gifts { get; private set; }
        public int IsAgeVerified { get; private set; }

        public Vault Vault { get; private set; }
        public Guild Guild { get; private set; }

        public static Account FromDb(DbAccount acc)
        {
            return new Account()
            {
                EmpiresCoin = acc.EmpiresCoin,
                AccountId = acc.AccountId,
                AccountType = acc.AccountType,
                AccountLifetime = acc.AccountLifetime,
                Name = acc.Name,
                NameChosen = acc.NameChosen,
                Converted = acc.Converted,
                Admin = acc.AccountType == (int)Core.config.AccountType.LOESOFT_ACCOUNT,
                MapEditor = acc.AccountType == (int)Core.config.AccountType.TUTOR_ACCOUNT,
                VerifiedEmail = acc.Verified,
                Credits = acc.Credits,
                NextCharSlotPrice = 100, // need adjusts
                BeginnerPackageTimeLeft = 604800,
                Gifts = acc.Gifts,
                IsAgeVerified = acc.IsAgeVerified,
                Vault = Vault.FromDb(acc, new DbVault(acc)),
                Guild = new Guild()
            };
        }

        public XElement ToXml() =>
            new XElement("Account",
                new XElement("EmpiresCoin", EmpiresCoin),
                new XElement("AccountId", AccountId),
                new XElement("AccountType", AccountType),
                new XElement("AccountLifetime", AccountLifetime),
                new XElement("Name", Name),
                NameChosen ? new XElement("NameChosen", null) : null,
                Converted ? new XElement("Converted", null) : null,
                Admin ? new XElement("Admin", null) : null,
                MapEditor ? new XElement("MapEditor", null) : null,
                VerifiedEmail ? new XElement("VerifiedEmail", null) : null,
                new XElement("Credits", Credits),
                new XElement("NextCharSlotPrice", NextCharSlotPrice),
                new XElement("BeginnerPackageTimeLeft", BeginnerPackageTimeLeft),
                new XElement("Originating", "None"),
                new XElement("cleanPasswordStatus", 1),
                new XElement("Gifts", Utils.ToCommaSepString(Gifts)),
                Guild.id != 0 ? Guild.ToXml() : null,
                Vault.ToXml(),
                new XElement("IsAgeVerified", IsAgeVerified)
            );
    }

    internal class Character
    {
        public int CharId { get; private set; }
        public int VocType { get; private set; }
        public int Level { get; private set; }
        public double Exp { get; private set; }
        public int MaxHP { get; private set; }
        public int MaxMP { get; private set; }
        public int HP { get; private set; }
        public int MP { get; private set; }
        public int Spd { get; private set; }
        public int Att { get; private set; }
        public int Def { get; private set; }
        public double AttExp { get; private set; }
        public double DefExp { get; private set; }
        public int Outfit { get; private set; }
        public int AttSpd { get; private set; }
        public int[] Equipment { get; private set; }

        public static Character FromDb(DbChar character, bool dead)
        {
            return new Character()
            {
                CharId = character.CharId,
                VocType = character.Vocation,
                Level = character.Level,
                Exp = character.Experience,
                MaxHP = character.MaxHP,
                MaxMP = character.MaxMP,
                HP = character.HP,
                MP = character.MP,
                Spd = character.Speed,
                Att = character.Attack,
                Def = character.Defense,
                AttExp = character.AttackExp,
                DefExp = character.DefenseExp,
                Outfit = character.Outfit,
                AttSpd = Settings.Vocation.AttackSpeed,
                Equipment = character.Equipments,
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("CharacterData",
                    new XElement("Character",
                        new XAttribute("id", CharId),
                        new XAttribute("level", Level),
                        new XAttribute("vocation", VocType),
                        Exp
                        ),
                    new XElement("SpeedBase", Spd),
                    new XElement("HealthPoints",
                        new XAttribute("max", MaxHP),
                        HP
                        ),
                    new XElement("MagicPoints",
                        new XAttribute("max", MaxMP),
                        MP
                        ),
                    new XElement("Attack",
                        new XAttribute("level", Att),
                        AttExp
                        ),
                    new XElement("Defense",
                        new XAttribute("level", Def),
                        DefExp
                        ),
                    new XElement("Outfit", Outfit),
                    new XElement("AttackSpeed", AttSpd),
                    new XElement("Equipment", Equipment.ToCommaSepString())
                );
        }
    }

    internal class CharList
    {
        public Character[] Characters { get; private set; }
        public int NextCharId { get; private set; }
        public int MaxNumChars { get; private set; }
        public Account Account { get; private set; }
        public IEnumerable<Settings.APPENGINE.ServerItem> Servers { get; set; }
        public static CharList FromDb(Database db, DbAccount acc)
        {
            return new CharList()
            {
                Characters = db.GetAliveCharacters(acc).Select(x => Character.FromDb(db.LoadCharacter(acc, x), false)).ToArray(),
                NextCharId = acc.NextCharId,
                MaxNumChars = acc.MaxCharSlot,
                Account = Account.FromDb(acc)
            };
        }

        public XElement ToXml(EmbeddedData data, DbAccount acc)
        {
            return
                new XElement("Chars",
                    new XAttribute("nextCharId", NextCharId),
                    new XAttribute("maxNumChars", MaxNumChars),
                    Characters.Select(x => x.ToXml()),
                    Account.ToXml(),
                    new XElement("Servers",
                        Servers.Select(x => x.ToXml())
                    )
                );
        }
    }
}