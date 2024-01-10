using Models.DoorDir;
using Models.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public abstract class DoorInteractable
    {
        public DoorInteractable? Component; 
        public abstract bool Interact(Player Player);


        public int GetDoorPriority()
        {
            switch (this)
            {
                case ColoredDoor _:
                    return 1;
                case OpenOnOdd _:
                    return 2;
                case OpenOnStonesInRoom _:
                    return 2;
                case ToggleDoor _:
                    return 3;
                case ClosingGate _:
                    return 4;
                default:
                    return 0;
            }
        }
    }


}
