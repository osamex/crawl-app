import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorMessage } from 'ng-bootstrap-form-validation';
import { ToastrService } from 'ngx-toastr';
import { ImageGalleryComponent } from 'src/app/common/components/image-gallery/image-gallery.component';
import { CrawlService } from 'src/app/common/sevices/crawl.service';
import { ImageService } from 'src/app/common/sevices/image.service';
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
  isComplete: boolean;
  @ViewChild(ImageGalleryComponent)imageGallery: ImageGalleryComponent;

  constructor(
    private crawlService: CrawlService,
    private toaster: ToastrService,
    private imageService: ImageService
  ) {}

  onSubmit(): void {
    if (this.formGroup.valid) {
      this.imageGallery.images = [];
      this.crawlService
        .execute({
          Created: new Date(),
          Id: Utils.Guid.New(),
          RequestData: { Url: this.formGroup.controls.webSiteAddress.value },
        })
        .subscribe(() => {
          this.toaster.info(
            'Sent to processing: ' +
              this.formGroup.controls.webSiteAddress.value
          );
          this.getImagesForWebSite();
        });
    }
  }

  getImagesForWebSite(): void {
    if (this.formGroup.valid) {
      this.imageService
      .getAllImages({
        Created: new Date(),
        Id: Utils.Guid.New(),
        RequestData: this.formGroup.controls.webSiteAddress.value,
      })
      .subscribe((response) => {
        if (response && response.DataList) {
          console.log('is ok!', response);
          this.imageGallery.images = response.DataList;
        }
  
        this.isComplete = true;
      });
    } else {
      this.toaster.warning('No images found!')
    }
  }

  customErrorMessages: ErrorMessage[] = [
    {
      error: 'pattern',
      format: () => 'Is not valid URL!',
    },
  ];

  ngOnInit(): void {
    this.isComplete = false;
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

  getDataFromDb(): void {
    this.imageGallery.images = [];
    this.getImagesForWebSite();
  }

  resetForm(): void {
    this.imageGallery.images = [];
    this.formGroup.controls.webSiteAddress.setValue(null);
    this.formGroup.markAsPristine();
    this.formGroup.markAsUntouched();
    this.isComplete = false;
  }
}
