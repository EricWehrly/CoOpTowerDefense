using System;
using System.Collections.Generic;
using System.Linq;
using DisruptoLib.Entities;
using SharpKit.JavaScript;

namespace DisruptoLib.Factories
{
    [JsType(JsMode.Clr, Filename = "../res/Factories.js")]
    public static class CharacterFactory
    {
        private static Dictionary<string, Character> _playerCharacters = new Dictionary<string, Character>();

        private static Dictionary<Guid, Character> _projectiles = new Dictionary<Guid, Character>();

        public static Character[] Players { get { return _playerCharacters.Values.ToArray(); } }

        public static Character[] Projectiles { get { return _projectiles.Values.ToArray(); } }

        public static Character[] Characters { get { return Players.Concat(Projectiles).ToArray(); } }

        public static DateTime LastUpdate { get; private set; }

        private static int _nextPlayerId;

        public static int NextPlayerId { get { return _nextPlayerId++; } }

        public static bool PlayerExists(string playerId)
        {
            return _playerCharacters.Keys.Contains(playerId);
        }

        public static Character GetLocalPlayerCharacter()
        {
            return Players[0];
        }

        public static Character GetPlayerCharacter(string playerId)
        {
            return _playerCharacters[playerId];
        }

        public static string AddPlayer()
        {
            var playerGuid = Guid.NewGuid().ToString();

            var newCharacter = new Character();

            newCharacter.Id = Guid.NewGuid();

            _playerCharacters.Add(playerGuid, newCharacter);

            LastUpdate = DateTime.Now;

            return playerGuid;
        }

        public static void ClearPlayers()
        {
            _playerCharacters = new Dictionary<string, Character>();

            LastUpdate = DateTime.Now;
        }

        public static void ClearProjectiles()
        {
            _projectiles = new Dictionary<Guid, Character>();

            LastUpdate = DateTime.Now;
        }

        public static Guid CreateProjectile(Character owner, float speed)
        {
            var projectile = new Character
            {
                Position = new Point(owner.Position.X, owner.Position.Y),
                LookTarget = new Point(owner.LookTarget.X, owner.LookTarget.Y),
                Destination = new Point(owner.LookTarget.X, owner.LookTarget.Y),
                Speed = speed,
                Health = 10
            };
            projectile.Id = Guid.NewGuid();
            //projectile.Health = 10;

            _projectiles.Add(owner.Id, projectile);

            LastUpdate = DateTime.Now;

            return projectile.Id;
        }

        public static void KillCharacter(Character character)
        {
            foreach (KeyValuePair<Guid, Character> projectile in _projectiles)
            {
                if (projectile.Value == character)
                {
                    _projectiles.Remove(projectile.Key);

                    return;
                }
            }

            foreach (KeyValuePair<string, Character> player in _playerCharacters)
            {
                if (player.Value == character)
                {
                    _playerCharacters.Remove(player.Key);

                    return;
                }
            }

            LastUpdate = DateTime.Now;
        }
    }
}