namespace Domain.Pieces
{
    public interface IPawn : IChessPiece
    {
        Transition GetForwardOne();
        Transition GetForwardTwo();
        (Transition, Transition) GetDiagonals();
        (Transition, Transition) GetSides();

    }
}
