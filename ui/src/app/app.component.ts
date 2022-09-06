import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import {
  combineLatest,
  debounceTime,
  filter,
  map,
  Observable,
  startWith
} from 'rxjs';
import { CryptoCoin } from './models/crypto-coin';
import { CoinService } from './services/coin.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'cryptunics';

  coinCtrl = new FormControl('');
  coins: Observable<CryptoCoin[]> | null = null;
  filteredCoins: Observable<CryptoCoin[]> | null = null;
  selectedCoin: CryptoCoin | null = null;
  displayedCoins: CryptoCoin[] = [];

  constructor(private readonly coinService: CoinService) {}

  ngOnInit(): void {
    this.coins = this.coinService.getCryptoCoins().pipe(map((r) => r.data));
    this.filteredCoins = combineLatest([
      this.coinCtrl.valueChanges.pipe(startWith('')),
      this.coins,
    ]).pipe(
      filter(
        ([value, _]) =>
          !value || (typeof value === 'string' && value.length > 1)
      ),
      debounceTime(300),
      map(([value, coins]) => (value ? this.filterCoins(value, coins) : coins))
    );
  }

  coinSelected(event: MatAutocompleteSelectedEvent) {
    this.selectedCoin = event.option.value;
  }

  onAddSelectedCoin() {
    if (this.selectedCoin) {
      this.displayedCoins.push(this.selectedCoin);
    }
    this.coinCtrl.reset();
    this.selectedCoin = null;
  }

  onRemoveDisplayedCoin(coin: CryptoCoin, index: number) {
    this.displayedCoins.splice(index, 1);
  }

  displayCoin(coin: CryptoCoin) {
    return coin ? `${coin.name} | ${coin.symbol}` : '';
  }

  private filterCoins(value: string, coins: CryptoCoin[]): CryptoCoin[] {
    if (typeof value !== 'string') {
      return coins;
    }

    const filterValue = value.toLowerCase();

    return coins.filter(
      (coin) =>
        coin.name.toLowerCase().includes(filterValue) ||
        coin.symbol.toLowerCase().includes(filterValue)
    );
  }
}
