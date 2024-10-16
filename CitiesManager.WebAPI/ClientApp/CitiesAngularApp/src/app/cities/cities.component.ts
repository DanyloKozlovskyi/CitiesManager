import { Component } from '@angular/core';
import { City } from '../models/city';
import { CitiesService } from '../services/cities.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cities',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent {
  cities: City[] = [];
  constructor(private citiesService: CitiesService) {

  }
  ngOnInit() {
    this.citiesService.getCities().subscribe({
       next: (response: City[]) => {
        this.cities = response;
      },

       error: (error: any) => {
          console.log(error);
        },

       complete: () => { }
     });
  }
}
