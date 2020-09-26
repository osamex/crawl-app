import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorMessage } from 'ng-bootstrap-form-validation';
import { ToastrService } from 'ngx-toastr';
import { CrawlService } from 'src/app/common/sevices/crawl.service';
import { Utils } from 'src/app/common/utils';

@Component({
  selector: 'app-dashboard-view',
  templateUrl: './dashboard-view.component.html',
  styleUrls: ['./dashboard-view.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DashboardViewComponent implements OnInit {
  formGroup: FormGroup;
  isValid: boolean;
  constructor(private crawlService: CrawlService, private toaster: ToastrService) {}

  onSubmit(): void {
    if (this.formGroup.valid) {
        this.crawlService.execute({
            Created: new Date(),
            Id: Utils.Guid.New(),
            RequestData: { Url: this.formGroup.controls.webSiteAddress.value }
        }).subscribe((response) => {
            this.toaster.info('Task has been commissioned');
            this.formGroup.controls.webSiteAddress.setValue('');
        })
    }
  }

  customErrorMessages: ErrorMessage[] = [
    {
      error: 'pattern',
      format: () => 'Is not valid URL!',
    },
  ];

  ngOnInit(): void {
    this.isValid = false;
    const reg = '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?';
    this.formGroup = new FormGroup({
      webSiteAddress: new FormControl('', [
        Validators.required,
        Validators.pattern(reg),
      ]),
    });

    this.formGroup.statusChanges.subscribe((status) => {
      this.isValid = status == 'VALID';
    });
  }
}
