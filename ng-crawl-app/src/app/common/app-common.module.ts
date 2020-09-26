import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AuthGuardService } from './sevices/auth-guard.service';

@NgModule({
  declarations: [
  ],
  imports: [
    HttpClientModule
  ],
  providers: [
    AuthGuardService
  ],
  exports: [
  ],
})
export class AppCommonModule {}
