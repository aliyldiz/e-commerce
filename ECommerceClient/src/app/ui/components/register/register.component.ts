import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators} from '@angular/forms';
import {User} from '../../../entities/user';
import {UserService} from '../../../services/common/models/user.service';
import {CreateUser} from '../../../contracts/users/create-user';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../../../services/ui/custom-toastr.service';
import {BaseComponent} from '../../../base/base.component';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent extends BaseComponent implements OnInit {
  constructor(private formBuilder: FormBuilder, private userService: UserService, private toastrService: CustomToastrService, spinner: NgxSpinnerService) {
    super(spinner);
  }

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

  async onSubmit(user: User) {
    this.submitted = true;

    if (this.frm.invalid)
      return;

    const result: CreateUser = await this.userService.create(user);

    if (result.succeded)
      this.toastrService.message(result.message, 'User created successfully', {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      });
    else
      this.toastrService.message(result.message, 'Error creating user', {
        messageType: ToastrMessageType.Error,
        position: ToastrPosition.TopRight
      });
  }
}
