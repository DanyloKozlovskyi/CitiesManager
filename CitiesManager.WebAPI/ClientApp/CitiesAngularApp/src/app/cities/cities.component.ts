import { Component } from '@angular/core';
import { City } from '../models/city';
import { CitiesService } from '../services/cities.service';
import { CommonModule } from '@angular/common';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DisableControlDirective } from '../directives/disable-control.directive'

@Component({
  selector: 'app-cities',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DisableControlDirective],
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent {
  cities: City[] = [];
  postCityForm: FormGroup;
  isPostCityFormSubmitted: boolean = false;

  putCityForm: FormGroup;
  editId: string | null = null;
  constructor(private citiesService: CitiesService) {
    this.postCityForm = new FormGroup({
      name: new FormControl(null, [Validators.required])
    })

    this.putCityForm = new FormGroup({
      cities: new FormArray([])
    })

  }

  get putCityFormArray(): FormArray {
    return this.putCityForm.get("cities") as FormArray;
  }

  loadCities() {
    this.citiesService.getCities().subscribe({
       next: (response: City[]) => {
        this.cities = response;

        this.cities.forEach(city => {
          this.putCityFormArray.push(new FormGroup({
            id: new FormControl(city.id, [Validators.required]),
            name: new FormControl({value: city.name, disabled: true}, [Validators.required])
          }));
        })
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
    this.isPostCityFormSubmitted = true;
    console.log(this.postCityForm.value);

    this.citiesService.postCities(this.postCityForm.value).subscribe({
      next: (response: City) => {
        console.log(response);

        this.loadCities();
        //this.cities.push(response);
      },

      error: (error: any) => {
        console.log(error);
      },

      complete: () => { }
    });
  }

  public editClicked(city: City) {
    this.editId = city.id;
  }

  public updateClicked(i: number) {
    this.citiesService.putCity(this.putCityFormArray.controls[i].value).subscribe({
      next: (response: string) => {
        console.log(response);
        console.log(this.putCityFormArray.controls[i].value);

        this.editId = null;
        this.putCityFormArray.controls[i].reset(this.putCityFormArray.controls[i].value);
      },
      error: (error: any) => {
        console.log(error)
      },
      complete: () => {

      }
    })
  }
}
