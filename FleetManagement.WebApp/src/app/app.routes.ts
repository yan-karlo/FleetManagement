import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', loadComponent: () => import('@core/home/home.component').then(m => m.HomeComponent) },
  { path: 'colors', loadComponent: () => import('@feature/colors/colors-list/colors-list.component').then(m => m.ColorsListComponent) },
  { path: 'colors/:action', loadComponent: () => import('@feature/colors/colors-form/colors-form.component').then(m => m.ColorsFormComponent) },
  { path: 'colors/:action/:id', loadComponent: () => import('@feature/colors/colors-form/colors-form.component').then(m => m.ColorsFormComponent) },
  { path: 'vehicles', loadComponent: () => import('@feature/vehicles/vehicles-list/vehicles-list.component').then(m => m.VehiclesListComponent) },
  { path: 'vehicles/:action', loadComponent: () => import('@feature/vehicles/vehicles-form/vehicles-form.component').then(m => m.VehiclesFormComponent) },
  { path: 'vehicles/:action/:id', loadComponent: () => import('@feature/vehicles/vehicles-form/vehicles-form.component').then(m => m.VehiclesFormComponent) },
  { path: 'vehicle-types', loadComponent: () => import('@feature/vehicle-types/vehicle-types-list/vehicle-types-list.component').then(m => m.VehicleTypesListComponent) },
  { path: 'vehicle-types/:action', loadComponent: () => import('@feature/vehicle-types/vehicle-types-form/vehicle-types-form.component').then(m => m.VehicleTypesFormComponent) },
  { path: 'vehicle-types/:action/:id', loadComponent: () => import('@feature/vehicle-types/vehicle-types-form/vehicle-types-form.component').then(m => m.VehicleTypesFormComponent) },
];
