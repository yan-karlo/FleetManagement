import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-save-delete-try-again-buttons-bundle',
  templateUrl: './save-delete-try-again-buttons-bundle.component.html',
  styleUrls: ['./save-delete-try-again-buttons-bundle.component.css'],
  imports: [RouterModule, CommonModule]
})
export class SaveDeleteTryAgainButtonsBundleComponent {
  @Input() formGroup!: FormGroup;
  @Input() formAction!: string;
  @Input() backLink!: string;
  @Output() onSubmit = new EventEmitter<void>();

  isDelete = this.formAction === 'delete'
}
