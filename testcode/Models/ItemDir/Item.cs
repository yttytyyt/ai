namespace Models.ItemDir
{
    public abstract class Item : ICollidable
    {
        public Position CurrentPosition { get; set; }


        public abstract void Interact(IEntity entity);

        public void Collide(IEntity entity)
        {
            Interact(entity);
        }
    }
}
