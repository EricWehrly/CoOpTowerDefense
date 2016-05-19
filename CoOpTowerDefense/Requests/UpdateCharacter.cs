using System.Linq;
using Nancy;
using Newtonsoft.Json;
using DisruptoLib.Entities;
using DisruptoLib.Factories;

namespace CoOpTowerDefense.Requests
{
    public class UpdateCharacter : NancyModule
    {
        public UpdateCharacter()
        {
            //Get["/SetCharacter/{data}"] = SetCharacter;

            //Get["/SetCharacter"] = SetCharacter;

            Post["/SetCharacter"] = SetCharacter;

            Post["/UpdateCharacter"] = SetCharacter;



            //Get["/SetDirection"] = SetCharacterDirection;

            //Get["/SetFacing"] = SetCharacterDirection;

            //Get["/CreateProjectile"] = SetCharacterDirection;
        }

        private object SetCharacter(dynamic parameters)
        {
            var sessionId = Request.Form.SessionId;

            var characterString = Request.Form.Character;

            var character = (Character)JsonConvert.DeserializeObject<Character>(characterString);

            var dataPlayer = CharacterFactory.Players.FirstOrDefault(player => player.Id == character.Id);

            if (!CharacterFactory.PlayerExists(sessionId)) return "Invalid session id.";

            var sessionCharacter = CharacterFactory.GetPlayerCharacter(sessionId);

            if (character.Id != sessionCharacter.Id)
            {
                return null;
            }

            dataPlayer.Destination = character.Destination;

            dataPlayer.LookTarget = character.LookTarget;

            return "Updated " + character.Id;
        }
    }
}
