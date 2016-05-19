using FluentAssertions;
using Nancy.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using DisruptoLib.Entities;
using DisruptoLib.Factories;
using CoOpTowerDefense.Requests;

namespace ServerTests
{
    public class ConnectionTests
    {
        [SetUp]
        public void SetUp()
        {
            CharacterFactory.ClearPlayers();
        }

        [Test]
        public void IndexSendsResponseWithScript()
        {
            var browser = new Browser(with => with.Module<Index>());
            
            var result = browser.Get("/", with => with.HttpRequest());
            
            result.Body.AsString().Should().Contain("<script");
        }

        [Test]
        public void ConnectingClientsGetEmptyPlayerListWhenAlone()
        {
            InitialConnect();

            var browser = new Browser(with => with.Module<GetPlayerList>());

            var result = browser.Get("/GetPlayerList", with => with.HttpRequest());

            var decodedResult = JsonConvert.DeserializeObject<Character[]>(result.Body.AsString());

            decodedResult.Length.Should().Be(1);
        }

        [Test]
        public void ConnectingPlayerIsAddedToPlayerPool()
        {
            // var browser = new Browser(with => with.Module<InitialConnect>());

            // browser.Get("/InitialConnect", with => with.HttpRequest());

            InitialConnect();

            CharacterFactory.Players.Length.Should().Be(1);
        }

        [Test]
        public void ConnectingClientsGetPopulatedPlayerListWhenThereAreUsers()
        {
            var initialPlayerCount = 5;

            SetPlayerCount(initialPlayerCount);

            InitialConnect();

            var browser = new Browser(with => with.Module<GetPlayerList>());

            var result = browser.Get("/GetPlayerList", with => with.HttpRequest());

            var decodedResult = JsonConvert.DeserializeObject<Character[]>(result.Body.AsString());

            decodedResult.Length.Should().Be(initialPlayerCount + 1);
        }

        [Ignore("Empty")]
        [Test]
        public void ClientConnectionTimesOut()
        {

        }

        [Ignore("Empty")]
        [Test]
        public void ClientCanManuallyDisconnect()
        {

        }
        
        private void SetPlayerCount(int newCount)
        {
            CharacterFactory.ClearPlayers();

            for (var i = 0; i < newCount; i++) CharacterFactory.AddPlayer();
        }

        private void InitialConnect()
        {
            var browser = new Browser(with => with.Module<InitialConnect>());

            browser.Get("/InitialConnect", with => with.HttpRequest());

            //return JsonConvert.DeserializeObject<ConnectPacket>(result.Body.AsString());
        }
    }
}
