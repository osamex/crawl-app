import { Injectable, Type } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { ToastrService } from 'ngx-toastr';
import { throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MessageTypesEnum } from '../enums/message-types.enum';
import { NotificationModel } from '../models/notification.model';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  constructor(private toaster: ToastrService) {}
  private hubConnection: signalR.HubConnection;
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.backendApi}/notificationhub`)
      .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  };

  public addListner = () => {
    this.hubConnection.on('Notify', (notification: NotificationModel) => {
      if (notification) {
        switch (notification.Type) {
            case MessageTypesEnum.Information:
                this.toaster.info(notification.Message, "", {enableHtml: true, closeButton: true, disableTimeOut: true});
                break;
            case MessageTypesEnum.Warning:
                this.toaster.warning(notification.Message, "", {enableHtml: true, closeButton: true, disableTimeOut: true});
                break;
            case MessageTypesEnum.Error:
                this.toaster.error(notification.Message, "", {enableHtml: true, closeButton: true, disableTimeOut: true});
                break;
            default:
                throwError('ERROR');
        }
      }
    });
  };
}
