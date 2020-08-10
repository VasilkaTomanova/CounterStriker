using CounterStrike.Core.Contracts;
using CounterStrike.Models;
using CounterStrike.Models.Guns.Contracts;
using CounterStrike.Models.Maps.Contracts;
using CounterStrike.Models.Players.Contracts;
using CounterStrike.Repositories;
using CounterStrike.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterStrike.Core
{
    public class Controller : IController
    {
        private GunRepository guns;
        private PlayerRepository players;
        private Map map;

        public Controller()
        {
            this.guns = new GunRepository();
            this.players = new PlayerRepository();
            this.map = new Map();
        }


        public string AddGun(string type, string name, int bulletsCount)
        {
            if (type != "Pistol" && type != "Rifle")
            {
                throw new ArgumentException(ExceptionMessages.InvalidGunType);
            }

            IGun gun;

            if (type == "Pistol")
            {
                gun = new Pistol(name, bulletsCount);
            }
            else
            {
                gun = new Rifle(name, bulletsCount);
            }

            this.guns.Add(gun);

            return string.Format(OutputMessages.SuccessfullyAddedGun, name);


        }

        public string AddPlayer(string type, string username, int health, int armor, string gunName)
        {
            IGun gun = this.guns.FindByName(gunName);

            if (gun == null)
            {
                throw new ArgumentException(ExceptionMessages.GunCannotBeFound);
            }

            if (type != "Terrorist" && type != "CounterTerrorist")
            {
                throw new ArgumentException(ExceptionMessages.InvalidPlayerType);
            }

            IPlayer player;

            if (type == "Terrorist")
            {
                player = new Terrorist(username, health, armor, gun);
            }
            else
            {
                player = new CounterTerrorist(username, health, armor, gun);
            }

            this.players.Add(player);
            return string.Format(OutputMessages.SuccessfullyAddedPlayer, username);
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var player in this.players.Models.OrderBy(x => x.GetType().Name).ThenByDescending(x => x.Health).ThenBy(x => x.Username).ToList())
            {
                sb.AppendLine(player.ToString());
            }
            return sb.ToString().TrimEnd();
        }

        public string StartGame()
        {
            ICollection<IPlayer> allivePlayers = this.players.Models.Where(x => x.IsAlive).ToList();
            return this.map.Start(allivePlayers);
        }
    }
}
