import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CryptoCoin } from '../models/crypto-coin';
import { Response } from '../models/response';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CoinService {
  private readonly baseUrl: string;

  constructor(private http: HttpClient) {
    this.baseUrl = `${environment.apiEndpoint}/api/coins`;
  }

  getCryptoCoins(): Observable<Response<CryptoCoin[]>> {
    return this.http.get<Response<CryptoCoin[]>>(`${this.baseUrl}/crypto`);
  }
}
