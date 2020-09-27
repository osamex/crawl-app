import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from 'src/app/common/sevices/auth-guard.service';
import { DashboardViewComponent } from './dashboard/dashboard-view.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgBootstrapFormValidationModule } from "ng-bootstrap-form-validation";
import { AppCommonModule } from 'src/app/common/app-common.module';

const routes: Routes = [
  {
    path: 'private',
    children: [
      {
        path: 'dashboard',
        component: DashboardViewComponent,
        canActivate: [AuthGuardService],
      }
    ],
  },
];

@NgModule({
  declarations: [
    DashboardViewComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forRoot(routes),
    ReactiveFormsModule,
    NgBootstrapFormValidationModule.forRoot(),
    AppCommonModule
  ],
  providers: [],
  exports: [
    RouterModule,
    DashboardViewComponent
  ]
})
export class ViewsPrivateModule { }
