using Models.entities;

namespace Models.ItemDir
{
    public class Boobytrap : Item
    {
        public int Damage { get; set; }

        public override void Interact(IEntity entity)
        {
            if(entity is Player player)
                player.TakeDamage(Damage);
        }
    }
}
