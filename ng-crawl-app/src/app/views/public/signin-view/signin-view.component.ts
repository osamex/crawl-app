import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthResponseModel } from 'src/app/common/models/auth-response-model';
import { AuthService } from 'src/app/common/sevices/auth.service';
import { Utils } from 'src/app/common/utils';

@Component({
  selector: 'app-signin-view',
  templateUrl: './signin-view.component.html',
  styleUrls: ['./signin-view.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class SignInViewComponent implements OnInit {
  formGroup: FormGroup;
  isValid: boolean;
  IsAuthenticated: boolean;

  constructor(private authService: AuthService) {}

  onSubmit(): void {
    if (this.formGroup.valid) {
      this.authService.signIn({
        Created: new Date(),
        Id: Utils.Guid.New(),
        RequestData: {
          Email: this.formGroup.controls.userEmail.value,
          Password: this.formGroup.controls.userPassword.value,
        },
      });
    }
  }

  ngOnInit(): void {
    this.IsAuthenticated = !Utils.HasSessionExpiredOrNotSignedIn();
    this.authService.userIsSignedInEvent.subscribe(
      (response: AuthResponseModel) => {
        this.IsAuthenticated =
          response != null &&
          response.BarerToken != null &&
          response.UserEmail != null;
      }
    );
    this.isValid = false;
    this.formGroup = new FormGroup({
      userEmail: new FormControl('', [Validators.required, Validators.email]),
      userPassword: new FormControl('', Validators.required),
    });

    this.formGroup.statusChanges.subscribe((status) => {
      this.isValid = status == 'VALID';
    });
  }
}
