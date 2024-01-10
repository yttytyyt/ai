using CODE_TempleOfDoom_DownloadableContent;
using Models.game;
using Models.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.entities
{
    public class EnemyAdapter : IObserver, IEntity, ICollidable
    {
        private readonly Enemy _Adaptee;
        private readonly Room _Room;

        public EnemyAdapter(Enemy adaptee, Room room)
        {
            _Adaptee = adaptee;
            _Adaptee.OnDeath += HandleEnemyDeath;  // Subscribe to OnDeath event
            adaptee.CurrentField = new PositionAdapter(new Position(adaptee.CurrentXLocation, adaptee.CurrentYLocation));
            _Room = room;
        }


        private void HandleEnemyDeath(object sender, EventArgs e)
        {
            CurrentPosition = null;
        }

        public Position? CurrentPosition
        {
            get
            {
                if (_Adaptee.CurrentXLocation == -1 || _Adaptee.CurrentYLocation == -1)
                    return null;
                else
                    return new Position(_Adaptee.CurrentXLocation, _Adaptee.CurrentYLocation);
            }
            set
            {
                if (value == null)
                {
                    _Adaptee.CurrentXLocation = -1;
                    _Adaptee.CurrentYLocation = -1;
                }
                else
                {
                    _Adaptee.CurrentXLocation = value.CoordinateX;
                    _Adaptee.CurrentYLocation = value.CoordinateY;
                }
            }
        }

        public int Lives
        {
            get
            {
                return _Adaptee.NumberOfLives;
            }
            set
            {
                throw new InvalidOperationException("Setting this property is not allowed.");
            }
        }

        public void Collide(IEntity entity)
        {
            if (entity is Player player)
            {
                player.TakeDamage(1);
            }
        }
        public void Move(Direction direction)
        {
            if (CurrentPosition != null)
            {
                _Adaptee.Move();
                Collision.CheckCollisions(_Room.SpecialItems.OfType<ICollidable>().ToList(), this);
            }
        }

        public void TakeDamage(int damage)
        {
            _Adaptee.DoDamage(damage);
        }

        public void Update()
        {
            Move(Direction.NORTH);
        }
    }

    public class PositionAdapter : IField

    {
        private readonly Position _Adaptee;

        public PositionAdapter(Position adaptee)
        {
            _Adaptee = adaptee;
        }

        public bool CanEnter => true;

        public IPlacable Item { get; set; }

        public IField GetNeighbour(int direction)
        {
            return new PositionAdapter(_Adaptee.GetNeighbour((Direction)direction));
        }
    }
}
