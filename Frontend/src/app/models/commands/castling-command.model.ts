import { MoveCommand } from './move-command.model';
import { ChessPiece } from '../chess-piece.model';

export class CastlingCommand extends MoveCommand
{
    movedRook: ChessPiece
}   
