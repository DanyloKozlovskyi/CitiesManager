import { Routes } from '@angular/router';
import { CitiesComponent } from './cities/cities.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app.component';

export const routes: Routes = [
  { path: "cities", component: CitiesComponent },
  { path: "register", component: RegisterComponent },
  { path: "login", component: LoginComponent },
  { path: "logout", component: AppComponent }
];
