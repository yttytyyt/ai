using Models.entities;

namespace Models.ItemDir
{
    public class SankaraStone : Item
    {
        public bool PickedUp { get; set; }
        public SankaraStone()
        {
            PickedUp = false;
        }
        public override void Interact(IEntity entity)
        {
            if (!PickedUp && entity is Player player)
            {
                player.AddToInventory(this);
                player.CurrentRoom.Items.Remove(this);
                PickedUp = true;
            }
        }
    }
}
