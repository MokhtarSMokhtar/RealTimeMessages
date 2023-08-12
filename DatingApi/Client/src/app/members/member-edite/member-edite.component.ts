import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edite',
  templateUrl: './member-edite.component.html',
  styleUrls: ['./member-edite.component.css']
})
export class MemberEditeComponent implements OnInit {
@ViewChild('editForm')editeForm:NgForm|undefined;
@HostListener('window:beforeunload',['$event']) unloadNotfication($event:any)
{
  if (this.editeForm?.dirty)
  {
    $event.returnValue = true;
  }
}
member:Member|undefined
user:User|null =null;

constructor(private accountService:AccountService,private memberService:MembersService,private toastr:ToastrService) {
 
  this.accountService.currentUser$.pipe(take(1)).subscribe({
    
    next:response=>{ 
      console.log("from constructor")
      console.log(response);
      this.user = response
    }  
  })
 
}

  ngOnInit(): void {
    this.loadMember()
  }
  
  loadMember()
  {
    if(!this.user)
    {
      return;
    }
    this.memberService.getMember(this.user.userName).subscribe({
      next:response=> { 
        this.member = response;
        console.log("load member")
        console.log(response);
      }
    })
    
  }

  updateMember()
  {
  this.memberService.updateMember(this.editeForm?.value).subscribe({
    next:response=> {
      console.log(response)
      this.toastr.success("success");
      this.editeForm?.reset(this.member);
    },
    error:error=> console.log(error)
  
    
  })

  }
}
