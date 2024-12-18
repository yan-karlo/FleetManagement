import { Injectable } from '@angular/core';
import { RequestService } from '@services/request-service/request.service';
import { Observable } from 'rxjs/internal/Observable';

import endpointConfig from '@configs/api.endpoints.config.json';


export interface IRequestData<T> {
  entity: string;
  action: string;
  body?: T | null;
  params?: string;
}

type Endpoints = {
  colors: string;
  vehicles: string;
  vehicleTypes: string;
};

@Injectable({
  providedIn: 'root'
})
export class DataService {

  request: any = {};

  constructor(private requestService: RequestService) {
    this.request = {
      get: <T>(url: string): Observable<T> => this.requestService.get(url),
      post: <T, K>(url: string, body: T): Observable<K> => this.requestService.post(url, body),
      put: <T, K>(url: string, body: T): Observable<K> => this.requestService.put(url, body),
      delete: <T>(url: string): Observable<T> => this.requestService.delete(url),
    }
  }

  run<T, K>(requestData: IRequestData<T>): Observable<K> {
    const { entity, action, body, params } = requestData;
    var url = this.buildUrl(entity, params);
    return this.request[action](url, body);
  }

  private buildUrl(entity: string, params: string = ''): string {
    const endpoint = endpointConfig[entity as keyof Endpoints] ?? '';
    const url = `${endpoint}${params}`;

    return url;
  }

}
