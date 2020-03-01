using Domain;
using Domain.Pieces;
using Domain.Players;
using Services.GameValidationService;
using Shouldly;
using Xunit;

namespace IntegrationTests
{
    public class GameValidationServiceIntegrationTests
    {
        private readonly IGameValidationService gameValidationService;

        public GameValidationServiceIntegrationTests()
        {
            gameValidationService = new GameValidationService();
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 2)]
        [InlineData(-1, 8)]
        [InlineData(2, -1)]
        [InlineData(8, -1)]
        [InlineData(8, 5)]
        [InlineData(8, 8)]
        [InlineData(5, 8)]
        public void IsMoveValid_DestinationIsOutsideTheBoard_ShouldReturnFalse(int x, int y)
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);
            var piece = new WhitePawn(new Position(5, 5));
            var dest = new Position(x, y);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 2)]
        [InlineData(-1, 8)]
        [InlineData(2, -1)]
        [InlineData(8, -1)]
        [InlineData(8, 5)]
        [InlineData(8, 8)]
        [InlineData(5, 8)]
        public void IsMoveValid_PieceIsOutsideTheBoard_ShouldReturnFalse(int x, int y)
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var piece = new WhitePawn(new Position(x, y));

            var dest = new Position(5, 5);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_MovingPlayerTriesToMoveOpponentsPiece_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var piece = new WhitePawn(new Position(1, 1));
            waitingPlayer.Pieces.Add(piece);

            var dest = new Position(2, 2);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_DestinationIsOccupiedByFriendlyPiece_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var pieceSource = new Queen(new Position(1, 1));
            var pieceDestination = new WhitePawn(new Position(2, 2));

            movingPlayer.Pieces.Add(pieceSource);
            movingPlayer.Pieces.Add(pieceDestination);

            var dest = new Position(2, 2);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_DestinationIsNotOnAPossiblePathFor_Queen_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var pieceSource = new Queen(new Position(1, 1));
            var pieceDestination = new WhitePawn(new Position(4, 2));

            movingPlayer.Pieces.Add(pieceSource);
            waitingPlayer.Pieces.Add(pieceDestination);

            var dest = new Position(4, 2);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_DestinationIsNotOnAPossiblePathFor_Knight_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var pieceSource = new Knight(new Position(1, 1));
            var pieceDestination = new WhitePawn(new Position(4, 2));

            movingPlayer.Pieces.Add(pieceSource);
            waitingPlayer.Pieces.Add(pieceDestination);

            var dest = new Position(4, 2);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_DestinationIsNotOnAPossiblePathFor_Pawn_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var pieceSource = new WhitePawn(new Position(1, 1));
            var pieceDestination = new WhitePawn(new Position(4, 2));

            movingPlayer.Pieces.Add(pieceSource);
            waitingPlayer.Pieces.Add(pieceDestination);

            var dest = new Position(4, 2);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_PathIsBlockedByFriendlyPiece_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var pieceSource = new Queen(new Position(1, 1));
            var blockingPiece = new WhitePawn(new Position(2, 2));

            movingPlayer.Pieces.Add(pieceSource);
            movingPlayer.Pieces.Add(blockingPiece);

            var dest = new Position(3, 3);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_PathIsBlockedByEnemyPiece_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var pieceSource = new Queen(new Position(1, 1));
            var blockingPiece = new WhitePawn(new Position(2, 2));

            movingPlayer.Pieces.Add(pieceSource);
            waitingPlayer.Pieces.Add(blockingPiece);

            var dest = new Position(3, 3);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_UniDirectionalBlackPieceMovesUpTheBoard_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.Black);
            var waitingPlayer = new Player(PlayerColor.White);

            var piece = new WhitePawn(new Position(1, 1));
            movingPlayer.Pieces.Add(piece);

            var dest = new Position(1, 0);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_DestinationIsOccupiedByEnemyKing_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var piece = new Queen(new Position(1, 1));
            var king = new King(new Position(0, 0));

            movingPlayer.Pieces.Add(piece);
            waitingPlayer.Pieces.Add(king);

            var dest = new Position(0, 0);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_PlayerTriesToMoveOtherPieceThanTheKingWhileInCheck_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var king = new King(new Position(0, 0));
            var movingPiece = new Queen(new Position(1, 1));
            var checkingPiece = new Queen(new Position(7, 0));

            movingPlayer.Pieces.Add(king);
            movingPlayer.Pieces.Add(movingPiece);
            waitingPlayer.Pieces.Add(checkingPiece);

            var dest = new Position(2, 2);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_KingMovesIntoCheck_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var king = new King(new Position(0, 0));
            var checkingPiece = new Queen(new Position(7, 0));

            movingPlayer.Pieces.Add(king);
            waitingPlayer.Pieces.Add(checkingPiece);

            var dest = new Position(1, 0);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_PieceMovesAndPutsOwnKingInCheck_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var king = new King(new Position(0, 0));
            var traitorPiece = new WhitePawn(new Position(1, 1));
            var checkingPiece = new Queen(new Position(2, 2));

            movingPlayer.Pieces.Add(king);
            movingPlayer.Pieces.Add(traitorPiece);
            waitingPlayer.Pieces.Add(checkingPiece);

            var dest = new Position(0, 1);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsMoveValid_PieceMovesAndBlocksCheckToOwnKing_ShouldReturnTrue()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var king = new King(new Position(0, 0));
            var saviorPiece = new WhitePawn(new Position(2, 1));
            var checkingPiece = new Queen(new Position(2, 2));

            movingPlayer.Pieces.Add(king);
            movingPlayer.Pieces.Add(saviorPiece);
            waitingPlayer.Pieces.Add(checkingPiece);

            var dest = new Position(1, 1);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(true);
        }

        [Fact]
        public void IsMoveValid_PieceTakesOpponentPieceThatCheckOwnKing_ShouldReturnTrue()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var king = new King(new Position(0, 0));
            var saviorPiece = new Queen(new Position(2, 1));
            var checkingPiece = new Queen(new Position(2, 2));

            movingPlayer.Pieces.Add(king);
            movingPlayer.Pieces.Add(saviorPiece);
            waitingPlayer.Pieces.Add(checkingPiece);

            var dest = new Position(2, 2);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(true);
        }

        [Fact]
        public void IsMoveValid_PawnTriesToMoveDiagonallyButNoOpponentPieceIsThereToTake_ShouldReturnFalse()
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var pawn = new WhitePawn(new Position(1, 1));
            var king = new King(new Position(7, 7));

            movingPlayer.Pieces.Add(pawn);
            movingPlayer.Pieces.Add(king);

            var dest = new Position(0, 0);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(0, 0, 1, 0)]
        [InlineData(1, 0, 1, 0)]
        public void IsMoveValid_PawnTriesToMoveForwardButOpponentPieceBlocksIt_ShouldReturnFalse(int destX, int destY, int enemyPawnX, int enemyPawnY)
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var enemyPiece = new BlackPawn(new Position(enemyPawnX, enemyPawnY));
            var movingPawn = new WhitePawn(new Position(2, 0));
            var king = new King(new Position(7, 7));

            movingPlayer.Pieces.Add(movingPawn);
            movingPlayer.Pieces.Add(king);

            waitingPlayer.Pieces.Add(enemyPiece);

            var dest = new Position(destX, destY);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(0, 0, 1, 0)]
        [InlineData(1, 0, 1, 0)]
        public void IsMoveValid_PawnTriesToMoveForwardButFriendlyPieceBlocksIt_ShouldReturnFalse(int destX, int destY, int friendlyPawnX, int friendlyPawnY)
        {
            // Arrange
            var movingPlayer = new Player(PlayerColor.White);
            var waitingPlayer = new Player(PlayerColor.Black);

            var friendlyPiece = new BlackPawn(new Position(friendlyPawnX, friendlyPawnY));
            var movingPawn = new WhitePawn(new Position(2, 0));
            var king = new King(new Position(7, 7));

            movingPlayer.Pieces.Add(movingPawn);
            movingPlayer.Pieces.Add(king);
            movingPlayer.Pieces.Add(friendlyPiece);

            var dest = new Position(destX, destY);

            // Act
            var result = gameValidationService.ValidateMove(TODO, TODO);

            // Assert
            result.ShouldBe(false);
        }
    }
}
