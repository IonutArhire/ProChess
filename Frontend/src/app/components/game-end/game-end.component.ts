import { Component, OnInit, Input } from '@angular/core';
import { GameStatusModel } from 'src/app/models/game-status.model';
import { PlayerColor } from 'src/app/models/player-color.model';
import { Player } from 'src/app/models/player.model';
import { GameStatusType } from 'src/app/models/game-status-type.model';

@Component({
  selector: 'app-game-end',
  templateUrl: './game-end.component.html',
  styleUrls: ['./game-end.component.scss']
})
export class GameEndComponent {

  @Input("gameEndStatus") private _gameEndStatus: GameStatusModel;

  constructor() { }

  private stringifyColor(player: Player): string
  {
      return player.color == PlayerColor.White
          ? "White"
          : "Black";
  }

  private getStatus(): string
  {
    if (this._gameEndStatus.gameStatusType == GameStatusType.Win)
    {
      return `${this.stringifyColor(this._gameEndStatus.winner)} Player Won!`;
    }

    return "Draw!";
  }

}
