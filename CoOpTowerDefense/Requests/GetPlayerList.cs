using Nancy;
using Newtonsoft.Json;
using DisruptoLib;
using DisruptoLib.Factories;

namespace CoOpTowerDefense.Requests
{
    public class GetPlayerList : NancyModule
    {
        public GetPlayerList()
        {
            Get["/GetPlayerList"] = ReturnPlayerList;
        }

        private object ReturnPlayerList(dynamic parameters)
        {
            return JsonConvert.SerializeObject(CharacterFactory.Players);
        }
    }
}
