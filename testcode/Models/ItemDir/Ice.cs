using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.entities;
using Models.util;

namespace Models.ItemDir
{
    public class Ice : Item
    {
        public override void Interact(IEntity entity)
        {
            if(entity is Player player)
                player.Step(player.CurrentDirection);
            else if(entity is EnemyAdapter enemy)
                enemy.Move(new Direction());
        }
    }
}
