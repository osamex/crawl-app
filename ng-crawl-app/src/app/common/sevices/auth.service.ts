import { EventEmitter, Injectable, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { IRequestInterface } from '../interfaces/i-request.interface';
import { IResponseInterface } from '../interfaces/i-response.interface';
import { AuthResponseModel } from '../models/auth-response-model';
import { Utils } from '../utils';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  @Output() userIsSignedInEvent: EventEmitter<AuthResponseModel> = new EventEmitter<AuthResponseModel>();

  constructor(private httpClient: HttpClient) {}

  public signIn(requestData: IRequestInterface): void {
    this.httpClient.post<IResponseInterface>(`${environment.backendApi}/api/auth/signIn`, requestData)
      .subscribe((response: IResponseInterface) => {
        if (response && response.ResponseData && response.IsSuccess) {
          const authUserData = response.ResponseData as AuthResponseModel;
          if(authUserData) {
            this.userIsSignedInEvent.emit(authUserData);
            Utils.LocalStorage.Set('userData', authUserData);
            return;
          }
        }

        this.signOut();
      }, () => { this.signOut(); });
  }

  public signOut(): void {
    Utils.LocalStorage.Remove('userData');
    this.userIsSignedInEvent.emit(null);
  }
}
