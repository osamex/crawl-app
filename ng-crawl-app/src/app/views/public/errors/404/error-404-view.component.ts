import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-error-404-view',
  templateUrl: './error-404-view.component.html',
  styleUrls: ['./error-404-view.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class Error404ViewComponent implements OnInit {
  formGroup: FormGroup;
  isValid: boolean;
  IsAuthenticated: boolean;

  constructor() {}

  ngOnInit(): void {
  }
}
