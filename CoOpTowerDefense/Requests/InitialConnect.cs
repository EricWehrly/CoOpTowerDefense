using System.Text;
using Nancy;
using Newtonsoft.Json;
using DisruptoLib;
using DisruptoLib.Factories;

namespace CoOpTowerDefense.Requests
{
    public class InitialConnect : NancyModule
    {
        public InitialConnect()
        {
            Get["/InitialConnect"] = InitialConnectResponse;
        }

        private Response InitialConnectResponse(dynamic parameters)
        {
            var newPlayer = CharacterFactory.AddPlayer();

            var returnVal = new ConnectPacket
            {
                SessionId = newPlayer,
                CharacterId = CharacterFactory.GetPlayerCharacter(newPlayer).Id
            };

            //return JsonConvert.SerializeObject(new Tuple<Guid, Character[]>(newPlayer, GameData.Players));
            var jsonBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(returnVal));

            var returnResponse = new Response();

            returnResponse.StatusCode = HttpStatusCode.OK;
            returnResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            returnResponse.ContentType = "application/json";
            returnResponse.Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length);

            return returnResponse;

            // return JsonConvert.SerializeObject(returnVal);
        } 
		
    }
}
