import { MoveCommand } from '../models/commands/move-command.model';
import { GameStatusModel } from '../models/game-status.model';

export class MoveResponse
{
    moveCommand: MoveCommand;
    gameStatusModel: GameStatusModel;
}