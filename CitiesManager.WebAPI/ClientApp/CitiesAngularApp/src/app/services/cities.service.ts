import { Injectable } from '@angular/core';
import { City } from "../models/city";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CitiesService {
  constructor(private httpClient: HttpClient)
  {
    
  }
  public getCities(): Observable<City[]> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", "Bearer mytoken");
    return this.httpClient.get<City[]>("https://localhost:7228/api/City", { headers: headers });
  }
}
