﻿using LoESoft.GameServer.realm;
using Mono.Game;
using LoESoft.GameServer.realm.entity.player;
using LoESoft.GameServer.networking.outgoing;

namespace LoESoft.GameServer.logic.skills.Pets
{
    internal class PetFollow : CycleBehavior
    {
        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            FollowState s;
            if (state == null)
                s = new FollowState();
            else
                s = (FollowState)state;

            Status = CycleStatus.NotStarted;

            Player player = host.GetPlayerOwner();
            Entity pet = player.Pet;
            bool hatchling = player.HatchlingPet;

            if (hatchling)
            {
                NOTIFICATION _notification = new NOTIFICATION();

                _notification.Color = new ARGB(0xFFFFFF);
                _notification.ObjectId = host.Id;
                _notification.Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"New Pet!\"}}";

                host.Owner.BroadcastPacket(_notification, null);
                return;
            }

            if (player.Owner == null || pet == null || host == null)
            {
                pet.Owner.LeaveWorld(host);
                return;
            }

            Vector2 vect = new Vector2(player.X - pet.X, player.Y - pet.Y);

            switch (s.State)
            {
                case F.DontKnowWhere:
                    if (s.RemainingTime > 0)
                        s.RemainingTime -= time.ElapsedMsDelta;
                    else
                        s.State = F.Acquired;
                    break;
                case F.Acquired:
                    if (player == null)
                    {
                        s.State = F.DontKnowWhere;
                        s.RemainingTime = 0;
                        break;
                    }
                    if (s.RemainingTime > 0)
                        s.RemainingTime -= time.ElapsedMsDelta;

                    vect = new Vector2(player.X - pet.X, player.Y - pet.Y);

                    if (vect.Length > 20)
                    {
                        Position _player = new Position();
                        _player.X = player.X;
                        _player.Y = player.Y;

                        pet.Move(player.X, player.Y);

                        GOTO _goto = new GOTO();
                        _goto.ObjectId = player.Id;
                        _goto.Position = _player;

                        pet.Owner.BroadcastPacket(_goto, null);
                        pet.UpdateCount++;
                    }
                    else if (vect.Length > 1 && vect.Length <= 20)
                    {
                        float dist = host.EntitySpeed(player.Stats[4] / 10, time);

                        if (vect.Length > 2 && vect.Length <= 3.5)
                            dist *= 1.75f;
                        else if (vect.Length > 3.5 && vect.Length <= 5)
                            dist *= 2f;
                        else if (vect.Length > 5 && vect.Length <= 6)
                            dist *= 2.25f;
                        else if (vect.Length > 6 && vect.Length <= 7)
                            dist *= 2.75f;
                        else if (vect.Length > 7 && vect.Length <= 10)
                            dist *= 3f;
                        else if (vect.Length > 10 && vect.Length <= 20)
                            dist *= 3.25f;

                        Status = CycleStatus.InProgress;
                        vect.Normalize();
                        pet.ValidateAndMove(pet.X + vect.X * dist, pet.Y + vect.Y * dist);
                        pet.UpdateCount++;
                    }

                    break;
            }

            state = s;
        }

        private enum F
        {
            DontKnowWhere,
            Acquired
        }

        private class FollowState
        {
            public int RemainingTime;
            public F State;
        }
    }
}