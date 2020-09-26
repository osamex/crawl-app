import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'app-dashboard-view',
    templateUrl: './dashboard-view.component.html',
    styleUrls: ['./dashboard-view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class DashboardViewComponent implements OnInit {

    constructor() {}

    ngOnInit(): void {
    }
}
