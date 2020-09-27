import { EventEmitter, Injectable, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { IRequestInterface } from '../interfaces/i-request.interface';
import { IResponseInterface } from '../interfaces/i-response.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
    constructor(private httpClient: HttpClient) {}

    public getAllImages(requestData: IRequestInterface): Observable<any> {
      return this.httpClient.post<IResponseInterface>(`${environment.backendApi}/api/image/getAllImages`, requestData);
  }
}
