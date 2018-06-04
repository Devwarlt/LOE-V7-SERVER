using LoESoft.Core.models;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LoESoft.GameServer.networking
{
    /// <summary>
    /// CTTManager
    /// Author: DV
    /// </summary>
    public class CTTManager
    {
        public ConcurrentDictionary<string, CTT> CTTData { get; private set; }

        private readonly string Path = "C:/LoESoft/Github/LOE-V7-SERVER/ctt_data.xml";

        private XmlDocument CTTLoadData { get; set; }
        private XDocument CTTSaveData { get; set; }

        public CTTManager()
        {
            CTTLoadData = new XmlDocument();
            CTTSaveData = new XDocument();
            CTTData = new ConcurrentDictionary<string, CTT>();
        }

        public void Init()
        {
            Log.Info("Initializing CTTManager...");

            Load();
        }

        private void Load()
        {
            CTTLoadData.LoadXml(File.ReadAllText(Path));

            CTTData = Serialization(CTTLoadData.GetElementsByTagName("Token"));

            int tokens = CTTData.Count;
            int usedTokens = CTTData.Values.Select(i => i.Use).Where(j => j).Count();

            Log.Info($"Initializing CTTManager... OK!" +
                $"\n\t\t- Tokens: {tokens} token{(tokens > 1 ? "s" : "")}" +
                $"\n\t\t- Used Tokens: {usedTokens} token{(usedTokens > 1 ? "s" : "")}"
                );
        }

        public void Save()
        {
            Log.Info("Reload CTTManager...");

            CTTSaveData = new XDocument(new XElement("CTTData", CTTData.Values.Select(j => j.Export)));
            CTTSaveData.Save(Path);

            int tokens = CTTData.Count;
            int usedTokens = CTTData.Values.Select(i => i.Use).Where(j => j).Count();

            Log.Info($"Reload CTTManager... OK!" +
                $"\n\t\t- Tokens: {tokens} token{(tokens > 1 ? "s" : "")}" +
                $"\n\t\t- Used Tokens: {usedTokens} token{(usedTokens > 1 ? "s" : "")}"
                );
        }

        private ConcurrentDictionary<string, CTT> Serialization(XmlNodeList doc)
        {
            ConcurrentDictionary<string, CTT> cache = new ConcurrentDictionary<string, CTT>();

            int j = doc.Count;
            while (j > 0)
            {
                for (int i = 0; i < doc.Count; i++)
                {
                    cache.TryAdd(
                        doc[i].Attributes["auth"].Value,
                        new CTT(
                            Auth: doc[i].Attributes["auth"].Value,
                            Use: Convert.ToBoolean(doc[i].Attributes["use"].Value),
                            Assing: doc[i].Attributes["assing"].Value
                            )
                        );
                    j--;
                }
            }

            return cache;
        }
    }

    public class CTT
    {
        public string Auth { get; private set; }
        public bool Use { get; private set; }
        public string Assing { get; private set; }

        public CTT(string Auth, bool Use, string Assing)
        {
            this.Auth = Auth;
            this.Use = Use;
            this.Assing = Assing;
        }

        public void UpdateUse()
            => Use = true;

        public XElement Export
            => new XElement("Token",
                new XAttribute("auth", Auth),
                new XAttribute("use", Use),
                new XAttribute("assing", Assing)
                );
    }
}
