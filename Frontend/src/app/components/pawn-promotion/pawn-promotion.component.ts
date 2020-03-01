import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { ConfigService } from 'src/app/services/config.service';

@Component({
  selector: 'app-pawn-promotion',
  templateUrl: './pawn-promotion.component.html',
  styleUrls: ['./pawn-promotion.component.scss']
})
export class PawnPromotionComponent {
  themePath = ConfigService.themePath;

  @Input("color")
	color: string;

  @Output("promotionChosen")
  promotionChosen = new EventEmitter<string>();
  
  selectPromotion(piece: string): void
  {
    this.promotionChosen.emit(piece);
  }
}
