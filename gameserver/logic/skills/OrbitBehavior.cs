using Mono.Game;
using System;
using System.Threading;

namespace LoESoft.GameServer.logic.behaviors
{
    public class OrbitBehavior
    {
        private static int PROCESSING_TIME_MS = 200; // '0' result in +25% CPU usage while running!

        private GameEntity ObjA { get; set; }
        private GameEntity ObjB { get; set; }
        private int Range
        {
            get { return Range; }
            set
            {
                if (value <= 0)
                    Range = 1;
                else
                    Range = value;
            }
        } // prevent negative or zero value
        private double Angle { get; set; } // range 0 to 360 degrees
        private double AngularVelocity { get; set; }

        // Event
        private event EventHandler<RangeEventArgs> RangeEvent;

        // Threads (background threads)
        private Thread RangeThread;
        private Thread AngularThread;

        public OrbitBehavior(GameEntity objA, GameEntity objB, int range, bool clockwise)
        {
            ObjA = objA;
            ObjB = objB;
            Range = range;
            Angle = 0;

            RangeEvent += OnRangeEvent;

            RangeThread =
                new Thread(() =>
                {
                    do
                    {
                        if (GetRange() != Range)
                            RangeEventHandler(new RangeEventArgs()
                            {
                                EntityA = ObjA,
                                EntityB = ObjB
                            });

                        Thread.Sleep(PROCESSING_TIME_MS);
                    } while (true);
                })
                { IsBackground = true };

            AngularThread =
                new Thread(() =>
                {
                    do
                    {
                        var phi = DegreesToRadians(Angle);

                        AngularVelocity = (((clockwise ? 1 : -1) * ObjB.Speed) / Range) + phi;

                        var aPos = ObjA.Position;
                        var bPos = ObjB.Position;
                        var deltaPos = new Vector2(aPos.X - bPos.X - (float)(Math.Cos(AngularVelocity) * Range), aPos.Y - bPos.Y - (float)(Math.Sin(AngularVelocity)) * Range);
                        deltaPos.Normalize();
                        deltaPos *= ObjA.Speed;

                        Move(ObjB, new Vector2(bPos.X - deltaPos.X, bPos.Y - deltaPos.Y));

                        Angle++;

                        Thread.Sleep(PROCESSING_TIME_MS);
                    } while (true);
                })
                { IsBackground = true };
        }

        public void Start()
        {
            RangeThread.Start();
            AngularThread.Start();
        }

        private int GetRange() => (int)Vector2.Distance(ObjA.Position, ObjB.Position);

        private double DegreesToRadians(double angle) => (angle * Math.PI) / 180;

        private void Move(GameEntity entity, Vector2 vect) => entity.Position = vect;

        // Keep range always constant.
        private void OnRangeEvent(object sender, RangeEventArgs e)
        {
            var aPos = e.EntityA.Position;
            var bPos = e.EntityB.Position;
            var deltaPos = new Vector2(aPos.X - bPos.X, aPos.Y - bPos.Y);
            deltaPos.Normalize();
            deltaPos *= e.EntityA.Speed;

            Move(e.EntityB, new Vector2(bPos.X - deltaPos.X, bPos.Y - deltaPos.Y));
        }

        private void RangeEventHandler(RangeEventArgs e) => RangeEvent?.Invoke(null, e);
    }

    public class GameEntity
    {
        public int Speed { get; set; } = 1;
        public Vector2 Position { get; set; } = new Vector2(0, 0);
    }

    public class RangeEventArgs : EventArgs
    {
        public GameEntity EntityA { get; set; }
        public GameEntity EntityB { get; set; }
    }
}