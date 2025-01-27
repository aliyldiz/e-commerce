import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators} from '@angular/forms';
import {User} from '../../../entities/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  constructor(private formBuilder: FormBuilder) {}

  frm: FormGroup;

  ngOnInit() {
    this.frm = this.formBuilder.group({
      fullName: ['', [Validators.required, Validators.maxLength(50), Validators.minLength(3)]],
      userName: ['', [Validators.required, Validators.maxLength(50), Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.maxLength(50), Validators.email]],
      password: ['', [Validators.required]],
      passwordConfirm: ['', [Validators.required]]
    }, {validators: (group: AbstractControl): ValidationErrors | null => {
      let password = group.get('password').value;
      let passwordConfirm = group.get('passwordConfirm').value;
      return password == passwordConfirm ? null : {passwordMismatch: true};
    }
    });
  }

  get component() {
    return this.frm.controls;
  }

  submitted: boolean = false;

  onSubmit(data: User) {
    this.submitted = true;

    if (this.frm.invalid)
      return;
  }
}
