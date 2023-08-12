import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { take } from 'rxjs';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {
@Input()appHasRole:string[]=[];
user :User ={} as User;
  constructor(private viewContainerRef:ViewContainerRef,private templateRef:TemplateRef<any>,
    private accountService:AccountService) { 

    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next:res=>{
        if(res)
        {
          this.user = res;
        }
      }
    })
    }
  ngOnInit(): void {
    let result = this.user.role.some(rol=>this.appHasRole.includes(rol));
    if(result)
    {
      console.log("test");
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }
    else
    {
      this.viewContainerRef.clear();
    }
  }

}
