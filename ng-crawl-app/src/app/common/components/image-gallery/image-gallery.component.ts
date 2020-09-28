import { Component, Input, OnInit } from '@angular/core';
import { ImageModel } from '../../models/image.model';

@Component({
  selector: 'app-image-gallery',
  templateUrl: './image-gallery.component.html',
  styleUrls: ['./image-gallery.component.scss'],
})
export class ImageGalleryComponent implements OnInit {

  @Input() public images: ImageModel[];
  currentImageSrc = null;
  closeResult = '';

  constructor() {
    this.images = [];
  }

  ngOnInit(): void {
  }
}
