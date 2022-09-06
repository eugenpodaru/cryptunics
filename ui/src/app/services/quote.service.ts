import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CryptoCoin } from '../models/crypto-coin';
import { Quote } from '../models/quote';
import { Response } from '../models/response';

@Injectable({
  providedIn: 'root',
})
export class QuoteService {
  private readonly baseUrl: string;

  constructor(private http: HttpClient) {
    this.baseUrl = `${environment.apiEndpoint}/api/quotes`;
  }

  getLatestQuote(coin: CryptoCoin): Observable<Response<Quote>> {
    return this.http.get<Response<Quote>>(`${this.baseUrl}/latest/${coin.id}`);
  }
}
