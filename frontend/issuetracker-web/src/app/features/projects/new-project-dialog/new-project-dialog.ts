import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

export interface NewProjectDialogResult {
  name: string;
}

@Component({
  selector: 'app-new-project-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './new-project-dialog.html'
})
export class NewProjectDialogComponent {
  name = new FormControl<string>('', {
    nonNullable: true,
    validators: [Validators.required, Validators.minLength(2), Validators.maxLength(120)]
  });

  constructor(private ref: MatDialogRef<NewProjectDialogComponent, NewProjectDialogResult>) {}

  cancel(): void {
    this.ref.close();
  }

  submit(): void {
    if (this.name.invalid) return;
    this.ref.close({ name: this.name.value.trim() });
  }
}
