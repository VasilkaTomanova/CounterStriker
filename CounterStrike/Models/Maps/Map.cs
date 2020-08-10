using CounterStrike.Models.Players.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace CounterStrike.Models.Maps.Contracts
{
    public class Map : IMap
    {
        private ICollection<IPlayer> terrorist;
        private ICollection<IPlayer> counterTerrorists;

        public string Start(ICollection<IPlayer> players)
        {
            this.terrorist = FullFillCollection(players, "Terrorist");
            this.counterTerrorists = FullFillCollection(players, "CounterTerrorist");

            while (this.terrorist.Any(x => x.IsAlive) && this.counterTerrorists.Any(x => x.IsAlive))
            {
                foreach (IPlayer terrorist in this.terrorist)
                {
                    if (!terrorist.IsAlive)
                    {
                        continue;
                    }
                    int currDamage = terrorist.Gun.Fire();

                    foreach (CounterTerrorist counterTerrorist in this.counterTerrorists)
                    {
                        counterTerrorist.TakeDamage(currDamage);
                    }

                }

                foreach (IPlayer counterTerrorist in this.counterTerrorists)
                {
                    if (!counterTerrorist.IsAlive)
                    {
                        continue;
                    }
                    int currDamage = counterTerrorist.Gun.Fire();

                    foreach (Terrorist terrorist in this.terrorist)
                    {
                        terrorist.TakeDamage(currDamage);
                    }
                }

            } // end while

            string result = "";
            if (this.terrorist.Any(x => x.IsAlive))
            {
                result = "Terrorist wins!";
            }
            else if(this.counterTerrorists.Any(x => x.IsAlive))
            {
                result = "Counter Terrorist wins!";
            }
            return result;
        }


        private static ICollection<IPlayer> FullFillCollection(ICollection<IPlayer> players, string type)
        {
            ICollection<IPlayer> listToReturn = new List<IPlayer>();
            foreach (IPlayer player in players)
            {
                if (player.GetType().Name == type)
                {
                    listToReturn.Add(player);
                }
            }
            return listToReturn;
        }


    }
}
