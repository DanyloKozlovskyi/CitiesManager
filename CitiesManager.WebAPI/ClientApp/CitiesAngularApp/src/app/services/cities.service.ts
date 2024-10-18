import { Injectable } from '@angular/core';
import { City } from "../models/city";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from "rxjs";

const API_BASE_URL: string = "https://localhost:7228/api/";

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
    return this.httpClient.get<City[]>(`${API_BASE_URL}city`, { headers: headers });
  }
  public postCities(city: City): Observable<City> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", "Bearer mytoken");
    return this.httpClient.post<City>(`${API_BASE_URL}city`, city, { headers: headers });
  }
  public putCity(city: City): Observable<string> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", "Bearer mytoken");

    return this.httpClient.put<string>(`${API_BASE_URL}city/${city.id}`, city, { headers: headers });
  }
}
