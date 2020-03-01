import { Player } from './player.model';
import { GameStatusType } from './game-status-type.model';

export class GameStatusModel
{
    winner: Player;
    gameStatusType: GameStatusType;
}