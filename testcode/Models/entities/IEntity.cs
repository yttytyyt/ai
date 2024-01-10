using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.util;

namespace Models.entities
{
    public interface IEntity
    {
        public Position CurrentPosition { get; set; }
        public int Lives { get; set; }


        public abstract void Move(Direction direction);

        public abstract void TakeDamage(int damage);

    }
}
/// <summary>
/// so an Enity is an object in game, an item is an object in game,
/// an entity is basic viewed just an extension of item.
/// an item has a action, so does entity, but entity is an extistance 
/// and can activly interact with the game.
/// 
/// </summary>

