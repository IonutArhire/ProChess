import { MoveCommand } from './move-command.model';

export class PawnPromotionCommand extends MoveCommand
{
    promotionType: string;
}