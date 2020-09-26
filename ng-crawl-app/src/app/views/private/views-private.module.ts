import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from 'src/app/common/sevices/auth-guard.service';
import { DashboardViewComponent } from './dashboard/dashboard-view.component';

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
    RouterModule.forRoot(routes),
  ],
  providers: [],
  exports: [
    RouterModule,
    DashboardViewComponent
  ]
})
export class ViewsPrivateModule { }
