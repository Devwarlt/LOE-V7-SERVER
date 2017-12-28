using common;
using common.config;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace appengine.gamestore
{
    public static class CONSTANTS
    {
        public static readonly string appengine = Settings.NETWORKING.APPENGINE_URL;
        public static WebClient client = new WebClient();
        public static readonly string file = "/app/packages/gameStoreData.json";
    }

    public class GameStore
    {
        public string welcome;
        public string offers;
        public string offer0;
        public string offer1;
        public string offer2;
        public string offer3;
        public string offer4;
    }

    internal class getOffers : RequestHandler
    {
        protected override void HandleRequest()
        {
            string JSONData = CONSTANTS.client.DownloadString(CONSTANTS.appengine + CONSTANTS.file);
            
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