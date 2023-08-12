import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

@Output()cancelRegister= new EventEmitter();
registerForm:FormGroup= new FormGroup({})
maxDate: Date = new Date();
validationErrors:string|undefined;

 constructor(private accountservice:AccountService,private toastr:ToastrService
  ,private formbuilder:FormBuilder,private router:Router) {
  
 }
  register() {
    const date = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value)
    const values = {...this.registerForm.value,dateOfBirth:date}
    this.accountservice.register(values).subscribe({
      next:response=>{
          this.router.navigateByUrl('/member')
      },
      error:errorResponse=>{
        this.validationErrors =errorResponse;
      }
    })
  }
  ngOnInit(): void {
    this.initializeForm()
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18);
  }

  initializeForm()
  {
   
    this.registerForm =this.formbuilder.group({
      gender:['male'],
      username:['',[Validators.minLength(3),Validators.required]],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password:['',[Validators.required,Validators.minLength(6)]],
      confirmPassword : ['',[Validators.required,this.matchesValue('password')]],
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next:()=> this.registerForm.controls['confirmPassword'].updateValueAndValidity()
      })
  }

  matchesValue(matchTo:string):ValidatorFn{
    return (control:AbstractControl)=>{
      return control.value ===control.parent?.get(matchTo)?.value ? null:{notMatching:true}
    }
  }
  private getDateOnly(dob: string | undefined) {
    if (!dob) return;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset()))
      .toISOString().slice(0,10);
  }
  cancel(){
   this.cancelRegister.emit(false)
  }
}
