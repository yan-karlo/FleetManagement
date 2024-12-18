import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-form-title',
  imports: [],
  templateUrl: './form-title.component.html',
  styleUrl: './form-title.component.css'
})
export class FormTitleComponent {
    @Input() formAction: string = '';
    @Input() entity: string = '';
}
