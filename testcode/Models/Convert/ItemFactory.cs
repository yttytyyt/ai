using Models.ItemDir;
using Models.util;

namespace Models.Convert
{
    public class ItemFactory
    {
        public Models.ItemDir.Item CreateItem(DataLayer.Item item)
        {
            switch (item.type)
            {
                case "boobytrap":
                    return new Boobytrap()
                    {
                        Damage = item.damage,
                        CurrentPosition = new Position(item.x, item.y)

                    };
                case "disappearing boobytrap":
                    return new DisappearingBoobytrap()
                    {
                        Damage = item.damage,
                        CurrentPosition = new Position(item.x, item.y)

                    };
                case "sankara stone":
                    return new SankaraStone()
                    {
                        CurrentPosition = new Position(item.x, item.y)
 
                    };
                case "key":
                    return new Models.ItemDir.Key()
                    {
                        Color = (Color)Enum.Parse(typeof(Color), item.color.ToUpper()),
                        CurrentPosition = new Position(item.x, item.y)

                    };
                case "pressure plate":
                    return new PressurePlate()
                    {
                        CurrentPosition = new Position(item.x, item.y)
                    };
                default:
                    throw new ArgumentException("Invalid type");
            }
        }
        public Models.ItemDir.Item CreateItem(DataLayer.Specialfloortile item)
        {
            switch(item.type)
            {
                case "ice":
                    return new Models.ItemDir.Ice()
                    {
                        CurrentPosition = new Position(item.x, item.y)
                    };
                default:
                    throw new ArgumentException("Invalid type");
            }
        }
    }
}
