using System;
using System.Collections.Generic;
using Domain.ClientModels;
using Domain.Commands;
using Domain.Players;

namespace Domain
{
    public class GameModel
    {
        public Guid Id { get; }
        public Player WhitePlayer { get; set; }
        public Player BlackPlayer { get; set; }
        public GameType GameType { get; }
        public Player MovingPlayer { get; set; }
        public Player WaitingPlayer { get; set; }
        public GameStatusModel GameStatusModel { get; set; }
        public List<MoveCommand> MovesHistory { get; set; }

        public GameModel(GameType gameType)
        {
            GameType = gameType;

            Id            = Guid.NewGuid();
            WhitePlayer   = new Player(PlayerColor.White);
            BlackPlayer   = new Player(PlayerColor.Black);
            MovingPlayer  = WhitePlayer;
            WaitingPlayer = BlackPlayer;
            GameStatusModel  = null;

            MovesHistory = new List<MoveCommand>();
        }

        public void SwitchTurns()
        {
            var aux = MovingPlayer;
            MovingPlayer = WaitingPlayer;
            WaitingPlayer = aux;
        }

        public bool SwitchPlayerPerspective(Func<bool> func)
        {
            SwitchTurns();
            var result = func();
            SwitchTurns();

            return result;
        }

        public void SwitchPlayerPerspective(Action func)
        {
            SwitchTurns();
            func();
            SwitchTurns();
        }
    }
}
