using Domain;
using Services;
using Shouldly;
using Xunit;

namespace UnitTests
{
    public class GameLogicExtensionsUnitTests
    {
        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 2)]
        [InlineData(-1, 8)]
        [InlineData(2, -1)]
        [InlineData(8, -1)]
        [InlineData(8, 5)]
        [InlineData(8, 8)]
        [InlineData(5, 8)]
        public void IsInBounds_PositionIsOutsideTheBoard_ShouldReturnFalse(int x, int y)
        {
            // Arrange
            var pos = new Position(x, y);

            // Act
            var result = pos.IsInBounds();

            // Assert
            result.ShouldBe(false);
        }

        [Fact]
        public void IsInBounds_PositionIsInsideTheBoard_ShouldReturnTrue()
        {
            // Arrange
            var pos = new Position(1, 1);

            // Act
            var result = pos.IsInBounds();

            // Assert
            result.ShouldBe(true);
        }
    }
}
