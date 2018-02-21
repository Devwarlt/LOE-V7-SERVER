#region

using LoESoft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

#endregion

namespace LoESoft.AppEngine.gamestore
{
    public class GameStore
    {
        public string welcome { get; set; }
        public string offers { get; set; }
        public string offer0 { get; set; }
        public string offer1 { get; set; }
        public string offer2 { get; set; }
        public string offer3 { get; set; }
        public string offer4 { get; set; }
    }

    internal class getOffers : RequestHandler
    {
        protected override void HandleRequest()
        {
            string JSONData = File.ReadAllText("gamestore/gameStoreData.json");

            DbAccount acc;
            LoginStatus status = Database.Verify(Query["guid"], Query["password"], out acc);

            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                if (status == LoginStatus.OK)
                    wtr.Write($"{JsonConvert.DeserializeObject<List<GameStore>>(JSONData)[0].welcome}|{JsonConvert.DeserializeObject<List<GameStore>>(JSONData)[0].offers}|{JsonConvert.DeserializeObject<List<GameStore>>(JSONData)[0].offer0}|{JsonConvert.DeserializeObject<List<GameStore>>(JSONData)[0].offer1}|{JsonConvert.DeserializeObject<List<GameStore>>(JSONData)[0].offer2}|{JsonConvert.DeserializeObject<List<GameStore>>(JSONData)[0].offer3}|{JsonConvert.DeserializeObject<List<GameStore>>(JSONData)[0].offer4}");
                else
                    return;
        }
    }
}