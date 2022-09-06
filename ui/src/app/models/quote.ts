import { CryptoCoin } from './crypto-coin';
import { Rate } from './rate';

export interface Quote {
  base: CryptoCoin;
  rates: Rate[];
}
