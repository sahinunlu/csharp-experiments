using System;
using System.Collections.Generic;
using System.Drawing;

namespace snakegame
{
    public class Snake
    {
        public EventHandler Collision;

        private Direction _CurrentDirection = Direction.Left;

        public Direction CurrentDirection
        {
            get { return _CurrentDirection; }
            set { _CurrentDirection = value; }
        }

        private LinkedList<Node> _Nodes;
        public LinkedList<Node> Nodes
        {
            get { return _Nodes; }
            set { _Nodes = value; }
        }

        private LinkedListNode<Node> _Head;
        public LinkedListNode<Node> Head
        {
            get { return _Head; }
            set { _Head = value; }
        }

        public Snake()
        {
            Initialize();
        }

        public void Initialize()
        {
            _Nodes = new LinkedList<Node>();

            _Head = _Nodes.AddFirst(
                new Node
                {
                    Symbol = new Rectangle(new Point(Constants.InitialPos, Constants.InitialPos), new Size(Constants.NodeSize, Constants.NodeSize)),
                    OldLocation = new Point(Constants.InitialPos, Constants.InitialPos),
                    NewLocation = new Point(Constants.InitialPos, Constants.InitialPos)
                });

            for (int i = 1; i < Constants.InitialNodesCounts; i++)
            {
                AddNode();
            }
        }

        public void Move(Direction direction)
        {
            var headPos = _Head.Value.NewLocation;

            switch (direction)
            {
                case Direction.Up:
                    if (CurrentDirection == Direction.Down)
                    {
                        return;
                    }

                    Head.Value.OldLocation = Head.Value.NewLocation;
                    Head.Value.NewLocation = new Point(headPos.X, headPos.Y - Constants.NodeSize);
                    GetIn(_Head);
                    CurrentDirection = Direction.Up;
                    break;
                case Direction.Down:
                    if (CurrentDirection == Direction.Up)
                    {
                        return;
                    }

                    Head.Value.OldLocation = Head.Value.NewLocation;
                    Head.Value.NewLocation = new Point(headPos.X, headPos.Y + Constants.NodeSize);
                    GetIn(_Head);
                    CurrentDirection = Direction.Down;
                    break;
                case Direction.Left:
                    if (CurrentDirection == Direction.Rigth)
                    {
                        return;
                    }

                    Head.Value.OldLocation = Head.Value.NewLocation;
                    Head.Value.NewLocation = new Point(headPos.X - Constants.NodeSize, headPos.Y);
                    GetIn(_Head);
                    CurrentDirection = Direction.Left;
                    break;
                case Direction.Rigth:
                    if (CurrentDirection == Direction.Left)
                    {
                        return;
                    }

                    Head.Value.OldLocation = Head.Value.NewLocation;
                    Head.Value.NewLocation = new Point(headPos.X + Constants.NodeSize, headPos.Y);
                    GetIn(_Head);
                    CurrentDirection = Direction.Rigth;
                    break;
                default:
                    break;
            }

            GenerateNewPositions();
        }

        public void EatFood()
        {
            AddNode();
        }

        private void AddNode()
        {
            _Nodes.AddLast(new Node
            {
                Symbol = new Rectangle(Nodes.Last.Value.NewLocation, new Size(Constants.NodeSize, Constants.NodeSize)),
                OldLocation = Nodes.Last.Value.NewLocation,
                NewLocation = Nodes.Last.Value.NewLocation
            });
        }

        private void GenerateNewPositions()
        {
            foreach (var item in _Nodes)
            {
                var node = _Nodes.Find(item);

                if (node.Previous != null)
                {
                    if (Head.Value.NewLocation.X == node.Value.NewLocation.X && Head.Value.NewLocation.Y == node.Value.NewLocation.Y)
                    {
                        Collision?.Invoke(this, EventArgs.Empty);
                    }

                    node.Value.OldLocation = node.Value.NewLocation;
                    node.Value.NewLocation = node.Previous.Value.OldLocation;
                    GetIn(node);
                }
            }
        }

        private void GetIn(LinkedListNode<Node> node)
        {
            if (node.Value.NewLocation.X <= 0)
            {
                node.Value.NewLocation.X += Constants.Size;
            }

            if (node.Value.NewLocation.Y <= 0)
            {
                node.Value.NewLocation.Y += Constants.Size;
            }

            if (node.Value.NewLocation.Y >= Constants.Size)
            {
                node.Value.NewLocation.Y -= Constants.Size;
            }

            if (node.Value.NewLocation.X >= Constants.Size)
            {
                node.Value.NewLocation.X -= Constants.Size;
            }
        }
    }

}
