using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface ICollidable
    {
        public Position CurrentPosition { get; set; }

        public void Collide(IEntity entity);
    }
}
