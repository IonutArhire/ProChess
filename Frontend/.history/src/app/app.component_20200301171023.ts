import { Component, ViewChild } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ChessboardComponent, ChessSquareComponent } from './components/chessboard';

import { Move } from './models/move.model';
import { Position } from './models/position.model';
import { MoveCommand } from './models/commands/move-command.model';
import { CastlingCommand } from './models/commands/castling-command.model';
import { PawnPromotionCommand } from './models/commands/pawn-promotion-command.model';
import { EnPassantCommand } from './models/commands/en-passant-command.model';
import { GameResources } from './resources/game.resources';
import { MoveResponse } from './resources/move-response.resources';
import { GameStatusType } from './models/game-status-type.model';
import { GameStatusModel } from './models/game-status.model';
import { GameEndComponent } from './components/game-end/game-end.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  private _hubConnection: HubConnection
  private _gameId: string;
  private _isBoardFlipped: boolean = false;
  private _lastMove: [ChessSquareComponent, ChessSquareComponent] = [null, null];
  private _gameEndStatus: GameStatusModel;

  @ViewChild("chessBoard") private _chessBoard: ChessboardComponent;
  @ViewChild("gameEndView") private _gameEndView: GameEndComponent;

  ngOnInit() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5000/SinglePlayerHub")
      .build();

    this._hubConnection.serverTimeoutInMilliseconds = 10000 * 1000;

    this._hubConnection.on("Connected", (gameId) => { this.onConnected(gameId) });
    this._hubConnection.on("InitialBoardReceived", (initialBoard) => { this.onInitialBoardReceived(initialBoard) });
    this._hubConnection.on("MoveValidated", (moveResponse) => { this.onMoveValidated(moveResponse) });
    this._hubConnection.on("PawnPromotionValidated", (moveResponse) => { this.onMoveValidated(moveResponse) });

    this._hubConnection.start()
      .then(() => { })
      .catch();
  }

  private handleBaseMove(): void {
    this._chessBoard.move(this._chessBoard.getFEN(this._chessBoard.dragStartSquare), this._chessBoard.getFEN(this._chessBoard.draggEndSquare));
    this._chessBoard.dragStartSquare = null;
    this._chessBoard.draggEndSquare = null;
  }

  // TODO (priority 3): see if making a separate command for EnPassant makes sense
  private handleEnPassant(moveCommand: EnPassantCommand): void {
    this.handleBaseMove();

    let toSquare = this._chessBoard.getSquareByPosition(moveCommand.takenPiece.position);
    let piece = toSquare.piece;
    piece.detach();
    toSquare.clearPiece();
  }

  private handleCastling(castlingCommand: CastlingCommand): void {
    this.handleBaseMove();

    let rookEndSquareRow = castlingCommand.move.to.x;
    let rookEndSquareCol: number;
    if (castlingCommand.name == "CastlingLong") {
      rookEndSquareCol = castlingCommand.move.to.y + 1;
    }
    else {
      rookEndSquareCol = castlingCommand.move.to.y - 1;
    }

    let rookStartSquare = this._chessBoard.getSquareByPosition(castlingCommand.movedRook.position);
    let rookEndSquare = this._chessBoard.getSquareXY(rookEndSquareRow, rookEndSquareCol);

    this._chessBoard.move(this._chessBoard.getFEN(rookStartSquare), this._chessBoard.getFEN(rookEndSquare));
  }

  private handlePawnPromotion(moveCommand: PawnPromotionCommand): void {
    this.handleBaseMove();

    let promotionSquare = this._chessBoard.getSquareByPosition(moveCommand.move.to);
    let promotedPawn = promotionSquare.piece;

    this._chessBoard.promote(promotedPawn.color);
  }

  private onMoveValidated(moveResponse: MoveResponse): void {
    let moveCommand = moveResponse.moveCommand;

    if (!moveCommand) {
      return;
    }

    if (moveCommand.name == null) {
      this.handleBaseMove();
    }

    if (moveCommand.name == "CastlingLong" ||
      moveCommand.name == "CastlingShort") {
      this.handleCastling(moveCommand as CastlingCommand);
    }

    if (moveCommand.name == "EnPassant") {
      this.handleEnPassant(moveCommand as EnPassantCommand);
    }

    if (moveCommand.name == "PawnPromotion") {
      let pawnPromotionCommand = moveCommand as PawnPromotionCommand;
      if (pawnPromotionCommand.promotionType == null) {
        this.handlePawnPromotion(pawnPromotionCommand);
      }
    }

    let gameStatus = moveResponse.gameStatusModel;
    if (gameStatus.gameStatusType != GameStatusType.Ongoing) {
      this._gameEndStatus = gameStatus;
    }
  }

  private onConnected(gameId: string): void {
    this._gameId = gameId;
    this._hubConnection.invoke("SendResources", this._gameId);
  }

  private onInitialBoardReceived(initialBoard: GameResources): void {
    for (var piece of initialBoard.whitePlayer.pieces) {
      let x = piece.position.x;
      let y = piece.position.y;

      this._chessBoard.board[x][y].setPiece(piece.name, "white");
    }

    for (var piece of initialBoard.blackPlayer.pieces) {
      let x = piece.position.x;
      let y = piece.position.y;

      this._chessBoard.board[x][y].setPiece(piece.name, "black");
    }
  }

  private onSquareClicked($event) {
    this._isBoardFlipped = !this._isBoardFlipped;
  }

  private onPieceDragEnd($event) {
    this._lastMove[0] = $event.fromSquare;
    this._lastMove[1] = $event.toSquare;

    var move = new Move();

    move.from = new Position($event.fromSquare.x, $event.fromSquare.y);
    move.to = new Position($event.toSquare.x, $event.toSquare.y);

    this._hubConnection.invoke("MakeMove", this._gameId, move);
  }

  private onPromotedPiece($event) {
    let x = this._lastMove[1].x;
    let y = this._lastMove[1].y;

    let promotionSquare = this._chessBoard.getSquareByPosition(new Position(x, y));
    let promotedPawn = promotionSquare.piece;

    this._chessBoard.board[x][y].setPiece($event, promotedPawn.color);

    let move = new Move();
    move.from = new Position(this._lastMove[0].x, this._lastMove[0].y);
    move.to = new Position(this._lastMove[1].x, this._lastMove[1].y);

    this._hubConnection.invoke("PromotePawn", this._gameId, move, $event);
  }
}
