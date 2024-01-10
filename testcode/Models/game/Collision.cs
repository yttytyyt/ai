using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.entities;
using Models.util;

namespace Models.game
{
    public static class Collision
    {
        public static void CheckCollisions(List<ICollidable> collidables, IEntity entity)
        {
            foreach (var collidable in collidables)
            {
                if (entity.CurrentPosition != null && entity.CurrentPosition.CompareTo(collidable.CurrentPosition))
                {
                    collidable.Collide(entity);
                }
            }
        }
    }

}
