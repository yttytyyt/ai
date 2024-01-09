using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Models.ItemDir;

namespace Models.DoorDir
{
    public class ClosingGate : DoorDecorator
    {
        public ClosingGate(DoorInteractable component) : base(component)
        {
            IsOpen = true;
        }

        public override bool Interact(Player player)
        {
            if (base.Interact(player) && AddedBehavior())
            {
                return true;
            }
            return false;
        }

        private bool AddedBehavior()
        {
            if (IsOpen)
            {
                IsOpen = false;
                return true;
            }

            return false;
        }
    }
}
