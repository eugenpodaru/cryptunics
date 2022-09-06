import { FiatCoin } from './fiat-coin';

export interface Rate {
  currency: FiatCoin;
  price: number;
  timestamp: Date;
  isDerived: boolean;
}
