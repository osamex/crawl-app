import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { SignInViewComponent } from './signin-view/signin-view.component';
import { CommonModule } from '@angular/common';
import { NgBootstrapFormValidationModule } from "ng-bootstrap-form-validation";

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'signin',
        component: SignInViewComponent,
      }
    ],
  },
];

@NgModule({
  declarations: [
    SignInViewComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forRoot(routes),
    ReactiveFormsModule,
    NgBootstrapFormValidationModule.forRoot(),
  ],
  providers: [],
  exports: [
    SignInViewComponent,
    RouterModule,
  ],
})
export class ViewsPublicModule {}
