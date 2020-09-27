import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { IResponseInterface } from './common/interfaces/i-response.interface';
import { AuthResponseModel } from './common/models/auth-response-model';
import { AuthService } from './common/sevices/auth.service';
import { SignalRService } from './common/sevices/signalr.service';
import { Utils } from './common/utils';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  AppName = 'Crawl App';
  IsAuthenticated: boolean;
  SwaggerLink = `${environment.backendApi}/swagger/index.html`;
  UserEmail = ''

  constructor(private authService: AuthService, private router: Router, private signalRService: SignalRService,) {

    this.signalRService.startConnection();
    this.signalRService.addListner();
    this.IsAuthenticated = !Utils.HasSessionExpiredOrNotSignedIn();
    if(this.IsAuthenticated) {
      this.UserEmail = Utils.GetCurrentUserEmail();
    } else {

    }

    this.authService.userIsSignedInEvent.subscribe((response: AuthResponseModel) => {
      this.IsAuthenticated = response != null && response.BarerToken != null && response.UserEmail != null;
      if(this.IsAuthenticated) {
        this.UserEmail = response.UserEmail;
        this.router.navigateByUrl('/private/dashboard');
      } else {
        this.router.navigateByUrl('/signin');
      }
    })

  }

  onSignOut() {
    this.authService.signOut();
  }
}
