import { Component } from '@angular/core';
import { City } from '../models/city';
import { CitiesService } from '../services/cities.service';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-cities',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent {
  cities: City[] = [];
  postCityForm: FormGroup;
  constructor(private citiesService: CitiesService) {
    this.postCityForm = new FormGroup({
      name: new FormControl(null, [Validators.required])
    })
  }

  loadCities() {
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
  ngOnInit() {
    this.loadCities();
  }

  get postCity_NameControl(): any {
    return this.postCityForm.controls['name'];
  }

  public postCitySubmitted() {

  }
}
