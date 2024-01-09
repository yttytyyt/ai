using CODE_TempleOfDoom_DownloadableContent;
using DataLayer;
using Models.ItemDir;
using System;

namespace Models
{
    #region context
    /*
 * as far for the ice tile, save it as item, easy for colision and drawing,
 * as for ladder, do save it in connection, drawing part is already acounted for, 
 * best way would be to change colsion method to a small extra class that can be called upon by all kinds of things
 * then just call class, say this position, these objects, is there hit? keep action also in colision class, 
 * extend all position objects from like colision class(easy to call)
 */
    #endregion
    public class Player : IObservable, IEntity
    {
        #region properties
        public List<ItemDir.Item> Items { get; set; }
        public Direction CurrentDirection { get; set; }
        public Room CurrentRoom { get; set; }
        public Game Game { get; set; }
        public Position CurrentPosition { get; set; }
        public int Lives { get; set; }
        private List<IObserver> _EnemyObserver = new List<IObserver>();
        #endregion

        #region Enity methods
        public void Move(Direction direction)
        {
            if(Step(direction))
            Notify();

        }

        public bool Step(Direction direction)
        {
            Position newPosition = CurrentPosition.GetNeighbour(direction);
            CurrentDirection = direction;
            if (CurrentRoom.IsValidMove(newPosition))
            {
                MoveToPosition(newPosition);
                return true;
            }
            else if (CurrentRoom.IsAtConnection(newPosition) && (Game.CanEnterConnection(direction) is Room newRoom))
            {
                MoveToNewRoom(newRoom);
                return true;
            }
            return false;
        }
        public void TakeDamage(int damage)
        {
            Lives -= damage;
        }

        public void DealDamage()
        {
            List<Position> NeighbourPositions = CurrentPosition.GetNeighbours();
            foreach (Position pos in NeighbourPositions)
            {
                var enemies = CurrentRoom.Enemies.OfType<IEntity>().Where(e => e.CurrentPosition != null && e.CurrentPosition.Equals(pos)).ToList();
                foreach (IEntity enemy in enemies)
                {
                    enemy.TakeDamage(1);
                }
            }
        }
        #endregion

        #region helper methods
        private void MoveToPosition(Position position)
        {
            CurrentPosition = position;
            Collision.CheckCollisions(CurrentRoom.Items.OfType<ICollidable>().ToList(), this);
            Collision.CheckCollisions(CurrentRoom.SpecialItems.OfType<ICollidable>().ToList(), this);
            Collision.CheckCollisions(CurrentRoom.Enemies.OfType<ICollidable>().ToList(), this);
            if(Game.Connections.Where(c => c.Ladder != null && c.Ladder.ContainsRoom(CurrentRoom)).ToList() is List<Connection> connections)
            {
                List<ICollidable> ladderList = new List<ICollidable>();
                foreach (Connection conn in connections)
                {
                    ladderList.Add(conn.Ladder);
                }
                Collision.CheckCollisions(ladderList, this);
            }
        }
        private void MoveToNewRoom(Room newRoom)
        {
            CurrentRoom = newRoom;
            CurrentPosition = newRoom.GetStartPosition(CurrentDirection.GetOppositeDirection());
            foreach (var enemy in CurrentRoom.Enemies.OfType<EnemyAdapter>().Cast<IObserver>().ToList())
            {
                Attach(enemy);
            }
            Game.SetLadderPosition(newRoom);

        }
        public void AddToInventory(ItemDir.Item item)
        {
            Items.Add(item);
        }

        public void PerformAction(MovementAction action)
        {
            switch (action)
            {
                case MovementAction.MoveNorth:
                    Move(Direction.NORTH);
                    break;
                case MovementAction.MoveSouth:
                    Move(Direction.SOUTH);
                    break;
                case MovementAction.MoveEast:
                    Move(Direction.EAST);
                    break;
                case MovementAction.MoveWest:
                    Move(Direction.WEST);
                    break;
                case MovementAction.DealDamage:
                    DealDamage();
                    break;
                    // Handle other actions...
            }
        }

        #endregion

        #region IObservable Methods
        public void Attach(IObserver observer)
        {
            if (!_EnemyObserver.Contains(observer))
                _EnemyObserver.Add(observer);

        }

        public void Detach(IObserver observer)
        {
            _EnemyObserver.Remove(observer);
        }

        public void Notify()
        {
            foreach (IObserver observer in _EnemyObserver)
            {
                observer.Update();
            }
            Collision.CheckCollisions(CurrentRoom.Enemies.OfType<ICollidable>().ToList(), this);

        }
        #endregion

    }
}
