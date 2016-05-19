using System;
using System.Linq;
using FluentAssertions;
using Nancy.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using DisruptoLib;
using DisruptoLib.Entities;
using DisruptoLib.Factories;
using CoOpTowerDefense.Requests;

namespace ServerTests
{
    public class ServerNetworkTests
    {
        private string _localPlayerId;

        private Character _localPlayer;
        
        private Browser _browser;

        [SetUp]
        public void SetUp()
        {
            CharacterFactory.ClearPlayers();

            InitialConnect(out _localPlayerId);
        }

        [Test]
        public void ClientsCanSetDesiredDestination()
        {
            var newDestination = new Point(10, 10);

            _localPlayer.Destination = newDestination;

            SendCharacterUpdate();

            var dataPlayerDestination = CharacterFactory.Players.FirstOrDefault(player => player.Id == _localPlayer.Id).Destination;

            dataPlayerDestination.X.Should().Be(newDestination.X);

            dataPlayerDestination.Y.Should().Be(newDestination.Y);
        }

        [Test]
        public void ClientsCanSetLookTarget()
        {
            var newLookTarget = new Point(10, 10);

            _localPlayer.LookTarget = newLookTarget;

            SendCharacterUpdate();

            var dataPlayerLookTarget = CharacterFactory.Players.FirstOrDefault(player => player.Id == _localPlayer.Id).LookTarget;

            dataPlayerLookTarget.X.Should().Be(newLookTarget.X);

            dataPlayerLookTarget.Y.Should().Be(newLookTarget.Y);
        }

        [Test]
        public void PlayerDirectionChangeIsSharedWithOtherPlayers()
        {
            var expectedX = 7;

            var expectedY = 11;

            // Log two players in ... 
            var browser2 = new Browser(with => with.Module<InitialConnect>());

            var result = browser2.Get("/InitialConnect", with => with.HttpRequest());

            //var connectPacket = JsonConvert.DeserializeObject<ConnectPacket>(result.Body.AsString());

            //string player2Id = connectPacket.SessionId;

            // Change the direction of one 
            _localPlayer.Destination = new Point(expectedX, expectedY);

            SendCharacterUpdate();

            // Make sure the other player gets the update ...
            browser2 = new Browser(with => with.Module<GetPlayerList>());

            result = browser2.Get("/GetPlayerList", with => with.HttpRequest());

            var deserializedResult = JsonConvert.DeserializeObject<Character[]>(result.Body.AsString());

            var localChar = deserializedResult.FirstOrDefault(player => player.Id == _localPlayer.Id);

            localChar.Destination.X.Should().Be(expectedX);

            localChar.Destination.Y.Should().Be(expectedY);
        }

        [Test]
        public void PlayersCanCreateProjectiles()
        {
            _browser = new Browser(with => with.Module<CreateProjectile>());

            var response = _browser.Post("/CreateProjectile", (with) =>
            {
                with.HttpRequest();
                with.FormValue("SessionId", _localPlayerId);
            });

            //var projectileId = JsonConvert.DeserializeObject<Guid>(response.Body.AsString());
            var projectileId = Guid.Parse(response.Body.AsString());

            var createdProjectile = CharacterFactory.Projectiles.FirstOrDefault(projectile => projectile.Id == projectileId);

            createdProjectile.Position.X.Should().Be(_localPlayer.Position.X);

            createdProjectile.Position.Y.Should().Be(_localPlayer.Position.Y);

            createdProjectile.LookTarget.X.Should().Be(_localPlayer.LookTarget.X);

            createdProjectile.LookTarget.Y.Should().Be(_localPlayer.LookTarget.Y);
        }

        [Test]
        public void UpdatingInvalidSessionTokenWillNotUpdateCharacter()
        //public void PlayersMustUpdateCharactersWithValidIds()
        {
            var initialDestination = _localPlayer.Destination;

            var newDestination = new Point(10, 10);

            _localPlayer.Destination = newDestination;

            var browser = new Browser(with => with.Module<UpdateCharacter>());

            var encodedCharacter = JsonConvert.SerializeObject(_localPlayer);

            browser.Post("/SetCharacter", (with) =>
            {
                with.HttpRequest();
                with.FormValue("Character", encodedCharacter);
                with.FormValue("SessionId", "Not a valid session token");
            });

            var dataPlayerDestination = CharacterFactory.Players.FirstOrDefault(player => player.Id == _localPlayer.Id).Destination;

            dataPlayerDestination.X.Should().Be(initialDestination.X);

            dataPlayerDestination.Y.Should().Be(initialDestination.Y);
        }

        [Ignore("Empty")]
        [Test]
        public void UpdatingWithInvalidSessionTokenTerminatesClientConnection()
        {
            // Need to be careful that we don't terminate based on passed in session

            // At this point, the session passed in could be another player's that they're trying to boot

            // response.Should().Be("Something about informing the client to reconnect.");
        }

        [Ignore("Empty")]
        [Test]
        public void UpdatingWrongCharacterWillFail()
        {
            // Mismatch session token and character id
        }

        [Ignore("Empty")]
        [Test]
        public void ClientCommandsWillBeRateLimited()
        {
            
        }

        [Ignore("Empty")]
        [Test]
        public void AdminCanSetFakePlayerCount()
        {
            var initialPlayerCount = 7;
            SetPlayerCount(initialPlayerCount);
        }

        private void InitialConnect(out string playerId)
        {
            _browser = new Browser(with => with.Module<InitialConnect>());

            var result = _browser.Get("/InitialConnect", with => with.HttpRequest());

            var connectPacket = JsonConvert.DeserializeObject<ConnectPacket>(result.Body.AsString());

            playerId = connectPacket.SessionId;

            var playerCharacterId = connectPacket.CharacterId;

            _browser = new Browser(with => with.Module<GetPlayerList>());

            result = _browser.Get("/GetPlayerList", with => with.HttpRequest());
            
            var characterList = JsonConvert.DeserializeObject<Character[]>(result.Body.AsString());

            _localPlayer = characterList.FirstOrDefault(character => character.Id == playerCharacterId);

            //_localPlayer = decodedResult.CharacterId;

            //_localPlayer = decodedResult[decodedResult.Length - 1];
        }

        private void SendCharacterUpdate()
        {
            var encodedCharacter = JsonConvert.SerializeObject(_localPlayer);

            _browser = new Browser(with => with.Module<UpdateCharacter>());

            _browser.Post("/SetCharacter", (with) =>
            {
                with.HttpRequest();
                with.FormValue("Character", encodedCharacter);
                with.FormValue("SessionId", _localPlayerId);
            });
        }

        private void SetPlayerCount(int newCount)
        {
            CharacterFactory.ClearPlayers();

            for (var i = 0; i < newCount; i++) CharacterFactory.AddPlayer();
        }

        // Need to add tests for projectiles, health, damage, collision...
    }
}
