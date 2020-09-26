import { Injectable } from '@angular/core';
import { HttpEvent, HttpRequest, HttpHandler, HttpInterceptor, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { tap, catchError } from 'rxjs/operators';
import { IResponseInterface } from '../interfaces/i-response.interface';
import { AuthService } from '../sevices/auth.service';

@Injectable()
export class ResponseInterceptor implements HttpInterceptor {

    constructor(private toaster: ToastrService, private authService: AuthService) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request)
            .pipe(
                catchError(exception => {
                    if ([401, 403].indexOf(exception.status) !== -1) {
                        this.authService.signOut();
                        this.toaster.warning('User logged out');
                        return;
                    }

                    this.toaster.error(exception.message);
                    return throwError(exception);
                }),
                tap(fullResponse => {
                    
                    try {
                        if (fullResponse instanceof HttpResponse && fullResponse.body && fullResponse.body as IResponseInterface) {
                            const response = fullResponse.body as IResponseInterface;
                            if(!response.IsSuccess || response.Errors) {
                                if(response.Errors) {
                                    response.Errors.forEach(error => {
                                        this.toaster.error(error);
                                    });
                                } else {
                                    throwError('Request failed');
                                }
                            }
                        } else {
                            throwError('[ResponseInterceptor].intercept(...): HttpResponse or IResponse CAST problem');
                        }
                    } catch (error) {
                        if (!error) {
                            error = 'Unhandled exception';
                        }
                        this.toaster.error(error);
                    }
                }));
    }
}
