using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ItemDir
{
    public class Ladder : Item
    {

        public Dictionary<Room, Position> LadderPosition { get; set; }


        public void SetPosition(Room room)
        {
            foreach (var key in LadderPosition.Keys)
            {
                if (key.Id == room.Id)
                {
                    CurrentPosition = LadderPosition[key];
                }
            }
        }
        public Position GetPosition(Room room)
        {
            var test = LadderPosition.Keys.SingleOrDefault(r => r.Id == room.Id);
            var tes2 = LadderPosition[test];
            return tes2;
        }

        public override void Interact(IEntity entitity)
        {
            if (entitity is Player player && LadderPosition.ContainsKey(player.CurrentRoom))
            {
                Room otherRoom = LadderPosition.Keys.First(room => room != player.CurrentRoom);

                player.CurrentRoom = otherRoom;
                player.CurrentPosition = LadderPosition[otherRoom];
                CurrentPosition = LadderPosition[otherRoom];
            }
        }


        public bool ContainsRoom(Room room)
        {
            return LadderPosition.ContainsKey(room);
        }

    }
}
