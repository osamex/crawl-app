import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { SignInViewComponent } from './signin-view/signin-view.component';
import { CommonModule } from '@angular/common';
import { NgBootstrapFormValidationModule } from "ng-bootstrap-form-validation";
import { Error404ViewComponent } from './errors/404/error-404-view.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'signin',
        component: SignInViewComponent,
      },
      {
        path: 'error/404',
        component: Error404ViewComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [
    SignInViewComponent,
    Error404ViewComponent
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
    Error404ViewComponent
  ],
})
export class ViewsPublicModule {}
