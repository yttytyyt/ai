using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.entities;
using Models.ItemDir;
using Models.util;

namespace Models.DoorDir
{
    public class ColoredDoor : DoorDecorator
    {
        public Color Color { get; set; }

        public ColoredDoor(DoorInteractable component, Color color) : base(component)
        {
            IsOpen = false;
            this.Color = color;
        }

        public override bool Interact(Player player)
        {
            if (base.Interact(player) && AddedBehavior(player))
            {
                return true;
            }
            return false;
        }

        private bool AddedBehavior(Player player)
        {
            if (player?.Items != null)
            {
                foreach (var item in player.Items)
                {
                    if (item is Key key && key.Color == Color)
                    {
                        IsOpen = true;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
