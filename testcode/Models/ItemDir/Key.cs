namespace Models.ItemDir
{
    public class Key : Item
    {
        public bool PickedUp { get; set; }
        public Color Color { get; set; }


        public Key() {
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
