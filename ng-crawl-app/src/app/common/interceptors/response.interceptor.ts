import { Injectable } from '@angular/core';
import { HttpEvent, HttpRequest, HttpHandler, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { AuthService } from '../sevices/auth.service';

@Injectable()
export class ResponseInterceptor implements HttpInterceptor {

    constructor(private _authService: AuthService) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request)
            .pipe(
                catchError(exception => {
                    if ([401, 403].indexOf(exception.status) !== -1) {
                        this._authService.signOut();
                        return;
                    }

                    return throwError('error when response');
                }),
                tap(() => {
                    try {
                        throwError('[ResponseInterceptor].intercept(...): HttpResponse or IResponse CAST problem');
                    } catch (error) {
                        if (!error) {
                            error = 'Unhandled exception';
                        }
                        throwError(error);
                    }
                }));
    }
}
