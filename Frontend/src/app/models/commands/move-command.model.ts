import { Move } from "../move.model";
import { ChessPiece } from '../chess-piece.model';

export class MoveCommand
{
    name: string;
    move: Move;
    movedPiece: ChessPiece;
    takenPiece: ChessPiece;
}