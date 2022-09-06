import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output
} from '@angular/core';
import {
  combineLatest,
  map,
  mergeMap,
  of,
  startWith,
  Subject,
  Subscription,
  timer
} from 'rxjs';
import { Quote } from 'src/app/models/quote';
import { QuoteService } from 'src/app/services/quote.service';
import { CryptoCoin } from '../../models/crypto-coin';

@Component({
  selector: 'app-coin-tile',
  templateUrl: './coin-tile.component.html',
  styleUrls: ['./coin-tile.component.scss'],
})
export class CoinTileComponent implements OnInit, OnDestroy {
  @Input()
  coin: CryptoCoin | null = null;

  @Output()
  remove: EventEmitter<CryptoCoin> = new EventEmitter<CryptoCoin>();

  quote: Quote | null = null;

  refresh$: Subscription | null = null;
  refresh: Subject<boolean> = new Subject<boolean>();

  constructor(private readonly quoteService: QuoteService) {}

  ngOnInit(): void {
    this.refresh$ = combineLatest([
      timer(0, 60 * 1000),
      this.refresh.pipe(startWith(true)),
    ])
      .pipe(
        mergeMap(() =>
          this.coin ? this.quoteService.getLatestQuote(this.coin) : of(null)
        ),
        map((r) => (r ? r.data : null))
      )
      .subscribe((quote) => (this.quote = quote));
  }

  ngOnDestroy(): void {
    if (this.refresh$) {
      this.refresh$.unsubscribe();
    }
  }

  onRefresh() {
    this.refresh.next(true);
  }

  onRemove() {
    if (this.coin) {
      this.remove.emit(this.coin);
    }
  }

  getCoinLogo() {
    return `url('https://cryptoicons.org/api/icon/${this.coin?.symbol.toLocaleLowerCase()}/50')`;
  }
}
