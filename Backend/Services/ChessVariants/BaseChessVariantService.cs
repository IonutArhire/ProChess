using Domain;
using Domain.ClientModels;
using Domain.Commands;
using Domain.Pieces;
using Services.GameValidationService;
using Services.MoveProcessorService;
using Services.PiecesManagerService;

namespace Services.ChessVariants
{
    public abstract class BaseChessVariantService : IBaseChessVariantService
    {
        protected IPiecesManagerService PiecesManagerService { get; }
        protected IGameValidationService GameValidationService { get; }
        public GameType GameType { get; set; }

        protected BaseChessVariantService(IPiecesManagerService piecesManagerService, IGameValidationService gameValidationService)
        {
            PiecesManagerService = piecesManagerService;
            GameValidationService = gameValidationService;
        }

        private void UpdateGameMovesHistory(GameModel game, MoveCommand moveCommand)
        {
            game.MovesHistory.Add(moveCommand);
        }

        protected virtual void SetGameStatus(GameModel game)
        {
            var gameStatus = GameStatusType.Ongoing;

            game.SwitchPlayerPerspective(() =>
            {
                if (PiecesManagerService.IsKingCaptured(game))
                {
                    gameStatus = GameStatusType.Win;
                }
                else if (PiecesManagerService.IsPlayerUnableToMove(game))
                {
                    gameStatus = GameStatusType.Draw;
                }
            });

            if (gameStatus != GameStatusType.Ongoing)
            {
                game.GameStatusModel = new GameStatusModel(gameStatus);

                if (gameStatus is GameStatusType.Win)
                {
                    game.GameStatusModel.Winner = game.MovingPlayer;
                }
            }
        }

        public virtual void InitializeBoard(GameModel game)
        {
            game.WhitePlayer.Pieces.Add(new Rook(GameConstants.BackRowWhite, 0));
            game.WhitePlayer.Pieces.Add(new Knight(GameConstants.BackRowWhite, 1));
            game.WhitePlayer.Pieces.Add(new Bishop(GameConstants.BackRowWhite, 2));
            game.WhitePlayer.Pieces.Add(new Queen(GameConstants.BackRowWhite, 3));
            game.WhitePlayer.Pieces.Add(new King(GameConstants.BackRowWhite, 4));
            game.WhitePlayer.Pieces.Add(new Bishop(GameConstants.BackRowWhite, 5));
            game.WhitePlayer.Pieces.Add(new Knight(GameConstants.BackRowWhite, 6));
            game.WhitePlayer.Pieces.Add(new Rook(GameConstants.BackRowWhite, 7));

            game.WhitePlayer.Pieces.Add(new WhitePawn(GameConstants.PawnRowWhite, 0));
            game.WhitePlayer.Pieces.Add(new WhitePawn(GameConstants.PawnRowWhite, 1));
            game.WhitePlayer.Pieces.Add(new WhitePawn(GameConstants.PawnRowWhite, 2));
            game.WhitePlayer.Pieces.Add(new WhitePawn(GameConstants.PawnRowWhite, 3));
            game.WhitePlayer.Pieces.Add(new WhitePawn(GameConstants.PawnRowWhite, 4));
            game.WhitePlayer.Pieces.Add(new WhitePawn(GameConstants.PawnRowWhite, 5));
            game.WhitePlayer.Pieces.Add(new WhitePawn(GameConstants.PawnRowWhite, 6));
            game.WhitePlayer.Pieces.Add(new WhitePawn(GameConstants.PawnRowWhite, 7));


            game.BlackPlayer.Pieces.Add(new Rook(GameConstants.BackRowBlack, 0));
            game.BlackPlayer.Pieces.Add(new Knight(GameConstants.BackRowBlack, 1));
            game.BlackPlayer.Pieces.Add(new Bishop(GameConstants.BackRowBlack, 2));
            game.BlackPlayer.Pieces.Add(new Queen(GameConstants.BackRowBlack, 3));
            game.BlackPlayer.Pieces.Add(new King(GameConstants.BackRowBlack, 4));
            game.BlackPlayer.Pieces.Add(new Bishop(GameConstants.BackRowBlack, 5));
            game.BlackPlayer.Pieces.Add(new Knight(GameConstants.BackRowBlack, 6));
            game.BlackPlayer.Pieces.Add(new Rook(GameConstants.BackRowBlack, 7));

            game.BlackPlayer.Pieces.Add(new BlackPawn(GameConstants.PawnRowBlack, 0));
            game.BlackPlayer.Pieces.Add(new BlackPawn(GameConstants.PawnRowBlack, 1));
            game.BlackPlayer.Pieces.Add(new BlackPawn(GameConstants.PawnRowBlack, 2));
            game.BlackPlayer.Pieces.Add(new BlackPawn(GameConstants.PawnRowBlack, 3));
            game.BlackPlayer.Pieces.Add(new BlackPawn(GameConstants.PawnRowBlack, 4));
            game.BlackPlayer.Pieces.Add(new BlackPawn(GameConstants.PawnRowBlack, 5));
            game.BlackPlayer.Pieces.Add(new BlackPawn(GameConstants.PawnRowBlack, 6));
            game.BlackPlayer.Pieces.Add(new BlackPawn(GameConstants.PawnRowBlack, 7));

            //game.WhitePlayer.Pieces.Add(new King(2, 0));
            //game.WhitePlayer.Pieces.Add(new WhitePawn(2, 2));

            //game.BlackPlayer.Pieces.Add(new King(0, 0));
        }

        public MoveCommand MakeMove(GameModel game, Move move, string promotionType = null)
        {
            if (game.GameStatusModel != null)
            {
                return null;
            }

            var validatedMoveCommand = GameValidationService.ValidateMove(game, move);

            if (validatedMoveCommand == null)
            {
                return null;
            }

            if (validatedMoveCommand is PawnPromotionCommand)
            {
                if (promotionType == null)
                {
                    // we return without updating anything because we wait
                    // the client to tell us which piece it wants to promote to
                    return validatedMoveCommand;
                }

                ((PawnPromotionCommand) validatedMoveCommand).PromotionType = promotionType;
            }

            var validatedMoveCommandClientCopy = validatedMoveCommand.Clone();
            
            PiecesManagerService.UpdatePieces(game, validatedMoveCommand);
            SetGameStatus(game);

            UpdateGameMovesHistory(game, validatedMoveCommand);
            game.SwitchTurns();

            return validatedMoveCommandClientCopy;
        }
    }
}
