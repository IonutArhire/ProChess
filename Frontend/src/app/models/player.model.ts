import { ChessPiece } from './chess-piece.model';
import { PlayerColor } from './player-color.model';

export class Player
{
    color: PlayerColor;
    pieces: Array<ChessPiece>;
}