import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ChessboardComponent } from './components/chessboard';
import { PawnPromotionComponent } from './components/pawn-promotion/pawn-promotion.component';
import { ConfigService } from './services/config.service';
import { GameEndComponent } from './components/game-end/game-end.component';

@NgModule({
  declarations: [
    AppComponent,
    ChessboardComponent,
    PawnPromotionComponent,
    GameEndComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FlexLayoutModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }