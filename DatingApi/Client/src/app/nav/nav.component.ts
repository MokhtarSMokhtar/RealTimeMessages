import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
model:any = {};
/**
 *
 */
constructor(public accountService:AccountService,private router:Router,private toastr: ToastrService) {
 

}
ngOnInit(): void {
  console.log(this.accountService.currentUser$)
}

login()
{
  this.accountService.login(this.model).subscribe({
    next:_=>this.router.navigateByUrl('/members'),
    error:errorResponse=>this.toastr.error(errorResponse.error)
  })
}
logout()
{
  this.accountService.logout()
  this.router.navigateByUrl('/')

}

}
