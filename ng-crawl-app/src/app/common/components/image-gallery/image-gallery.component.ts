import { Component, Input, OnInit } from '@angular/core';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ImageModel } from '../../models/image.model';
import { ImageService } from '../../sevices/image.service';

@Component({
  selector: 'app-image-gallery',
  templateUrl: './image-gallery.component.html',
  styleUrls: ['./image-gallery.component.scss'],
})
export class ImageGalleryComponent implements OnInit {

  @Input() public images: ImageModel[];
  currentImageSrc = null;
  closeResult = '';

  constructor(private modalService: NgbModal) {
    this.images = [];
  }

  ngOnInit(): void {

  }

  onClick(content: any, event: any) {
    this.currentImageSrc = event.srcElement.currentSrc;
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
      this.currentImageSrc = null;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      this.currentImageSrc = null;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
