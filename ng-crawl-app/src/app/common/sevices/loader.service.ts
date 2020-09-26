import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class LoaderService {
    isLoading = new Subject<boolean>();

    show() {
        console.debug('this.isLoading.next(true)');
        this.isLoading.next(true);
    }

    hide() {
        console.debug('this.isLoading.next(false)');
        this.isLoading.next(false);
    }
}