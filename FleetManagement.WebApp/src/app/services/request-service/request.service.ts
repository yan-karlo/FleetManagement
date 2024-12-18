import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RequestService {
  constructor(private http: HttpClient) { }

  get(url: string): Observable<any> {
    return this.http.get(url);
  }
  post<T>(url: string, body?: T): Observable<any> {
    var resp = this.http.post(url, body);
    //return this.http.post(url, body);
    return resp;
  }
  put<T>(url: string, body: T): Observable<any> {
    return this.http.put(url, body);
  }
  delete(url: string): Observable<any> {
    return this.http.delete(url);
  }
}
