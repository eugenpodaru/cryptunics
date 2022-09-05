import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoinTileComponent } from './coin-tile.component';

describe('CoinTileComponent', () => {
  let component: CoinTileComponent;
  let fixture: ComponentFixture<CoinTileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CoinTileComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CoinTileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
