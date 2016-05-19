using FluentAssertions;
using NUnit.Framework;
using DisruptoLib;
using DisruptoLib.Entities;
using DisruptoLib.Factories;

namespace ServerTests
{
    public class PhysicsTests
    {
        private string _playerOneSession;

        private Character _playerOne;

        private Character _playerTwo;

        [SetUp]
        public void SetUp()
        {
            CharacterFactory.ClearPlayers();

            CharacterFactory.ClearProjectiles();

            SetUpTwoPlayers();
        }

        // Need a whole lot of movement tests

        [Test]
        public void PlayerPositionsAreUpdatedRegularly()
        {
            var destinationX = 5;

            var destinationY = 8;

            var loopCount = 7;

            var expectedPositionX = 2;

            var expectedPositionY = 5;

            _playerOne.Destination = new Point(destinationX, destinationY);

            for(var i = 0; i < loopCount; i++) PhysicsEngine.MainLoop();

            _playerOne.Position.X.Should().Be(expectedPositionX);

            _playerOne.Position.Y.Should().Be(expectedPositionY);
        }

        [Test]
        public void PlayersWillMoveBySpeedAmount()
        {
            var destinationX = 5;

            var expectedPositionX = 4;

            _playerOne.Speed = 4;

            _playerOne.Destination = new Point(destinationX, 0);

            PhysicsEngine.MainLoop();

            _playerOne.Position.X.Should().Be(expectedPositionX);
        }

        [Test]
        public void FastPlayersWontOvershootMovement()
        {
            var destinationX = 5;

            var expectedPositionX = 5;

            _playerOne.Speed = 6;

            _playerOne.Destination = new Point(destinationX, 0);

            PhysicsEngine.MainLoop();

            _playerOne.Position.X.Should().Be(expectedPositionX);
        }

        [Test]
        public void PlayersWontKeepMovingPastDestination()
        {
            var destinationX = 5;

            var expectedPositionX = 5;

            _playerOne.Speed = 4;

            _playerOne.Destination = new Point(destinationX, 0);

            PhysicsEngine.MainLoop();

            PhysicsEngine.MainLoop();

            _playerOne.Position.X.Should().Be(expectedPositionX);
        }

        [Ignore("Empty")]
        [Test]
        public void PlayersWillMoveTowardGreaterDisplacement()
        {
            // The whole if xDist > yDist thing. Test that.

            Assert.Fail();
        }

        [Ignore("Unfinished")]
        [Test]
        public void ProjectilesWillCollideWithOtherPlayers()
        {
            var playerTwoX = 5;

            var projectileSpeed = 1;

            ShootPlayerTwo(playerTwoX, projectileSpeed);

            var loopCount = playerTwoX / projectileSpeed;

            for (var i = 0; i < loopCount; i++) PhysicsEngine.MainLoop();

            // How do we check collision ?

            // Maybe better to do it by running the method directly, rather than the whole physics loop

            Assert.Fail();
        }

        [Ignore("Empty")]
        [Test]
        public void ProjectilesWillNotCollideWithTheirOwner()
        {
            Assert.Fail();
        }

        [Test]
        public void ProjectilesWillBeRemovedAfterColliding()
        {
            var playerTwoX = 5;

            var projectileSpeed = 1;

            ShootPlayerTwo(playerTwoX, projectileSpeed);

            var loopCount = playerTwoX / projectileSpeed;

            CharacterFactory.Projectiles.Length.Should().Be(1);

            for (var i = 0; i < loopCount; i++) PhysicsEngine.MainLoop();

            CharacterFactory.Projectiles.Length.Should().Be(0);
        }

        [Test]
        public void ProjectilesWillDealDamageToWhoTheyCollideWith()
        {
            // Gamedata character positions are wrong...

            var playerTwoX = 5;

            var projectileSpeed = 2;

            var projectileDamage = 10;

            _playerTwo.Health = 100;

            ShootPlayerTwo(playerTwoX, projectileSpeed);

            var loopCount = playerTwoX / projectileSpeed;

            for (var i = 0; i < loopCount; i++) PhysicsEngine.MainLoop();

            _playerTwo.Health.Should().Be(100 - projectileDamage);
        }

        [Test]
        public void PlayersWithNoHealthWillBeRemovedFromPlayerList()
        {
            var playerTwoX = 10;

            var projectileSpeed = 3;

            var projectileDamage = 10;

            CharacterFactory.Players.Length.Should().Be(2);

            _playerTwo.Health = projectileDamage;

            ShootPlayerTwo(playerTwoX, projectileSpeed);

            var loopCount = playerTwoX / projectileSpeed;

            CharacterFactory.Players.Length.Should().Be(2);

            for (var i = 0; i < loopCount; i++) PhysicsEngine.MainLoop();

            CharacterFactory.Players.Length.Should().Be(1);
        }

        private void SetUpTwoPlayers()
        {
            _playerOneSession = CharacterFactory.AddPlayer();

            _playerOne = CharacterFactory.GetPlayerCharacter(_playerOneSession);

            var playerTwoId = CharacterFactory.AddPlayer();

            _playerTwo = CharacterFactory.GetPlayerCharacter(playerTwoId);
        }

        private void ShootPlayerTwo(int playerTwoX, int projectileSpeed)
        {
            _playerTwo.Position = new Point(playerTwoX, 0);

            _playerTwo.Destination = new Point(playerTwoX, 0);

            _playerOne.LookTarget = _playerTwo.Position;

            CharacterFactory.CreateProjectile(_playerOne, projectileSpeed);
        }
    }
}
