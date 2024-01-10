using Models.entities;

namespace Models.ItemDir
{
    public class DisappearingBoobytrap : Boobytrap
    {
        public bool Used { get; set; }

        public DisappearingBoobytrap()
        {
            Used = false;
        }
        public override void Interact(IEntity entity)
        {
            if (!Used && entity is Player player)
                {
                player.TakeDamage(Damage);
                player.CurrentRoom.Items.Remove(this);
                Used = true;
            }
        }
    }
}
