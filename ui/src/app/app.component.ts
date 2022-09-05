import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { map, Observable, startWith } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'cryptunics';

  coinCtrl = new FormControl('');
  filteredCoins: Observable<any[]>;
  selectedCoin: any;

  coins: any[] = [
    {
      id: 1,
      symbol: 'BTC',
      name: 'Bitcoin',
    },
    {
      id: 2,
      symbol: 'LTC',
      name: 'Litecoin',
    },
    {
      id: 3,
      symbol: 'NMC',
      name: 'Namecoin',
    },
    {
      id: 4,
      symbol: 'TRC',
      name: 'Terracoin',
    },
    {
      id: 5,
      symbol: 'PPC',
      name: 'Peercoin',
    },
  ];

  constructor() {
    this.filteredCoins = this.coinCtrl.valueChanges.pipe(
      startWith(''),
      map((coin) => (coin ? this.filterCoins(coin) : this.coins.slice()))
    );
  }

  coinSelected(event: MatAutocompleteSelectedEvent) {
    this.selectedCoin = event.option.value;
  }

  onAddSelectedCoin() {
    this.coinCtrl.reset();
    this.selectedCoin = null;
  }

  displayCoin(coin: any) {
    return coin ? `${coin.name} | ${coin.symbol}` : '';
  }

  private filterCoins(value: string): any[] {
    const filterValue = value.toLowerCase();

    return this.coins.filter((coin) =>
      coin.name.toLowerCase().includes(filterValue)
    );
  }
}
