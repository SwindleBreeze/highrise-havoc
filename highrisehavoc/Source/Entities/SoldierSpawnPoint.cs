using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace highrisehavoc.Source.Entities
{
    public class SoldierSpawnPoint
    {
        public Vector2 Position { get; set; }
        public bool IsOccupied { get; set; }

        public SoldierSpawnPoint(Vector2 position)
        {
            Position = position;
            IsOccupied = false;
        }
    }
}
