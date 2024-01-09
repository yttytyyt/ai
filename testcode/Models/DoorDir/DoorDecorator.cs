using Models.ItemDir;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DoorDir
{
    public abstract class DoorDecorator : DoorInteractable
    {
        protected bool IsOpen;
        public DoorDecorator(DoorInteractable component)
        {
            this.Component = component;
        }

        public override bool Interact(Player player)
        {
            return Component.Interact(player);
        }
    }
}
