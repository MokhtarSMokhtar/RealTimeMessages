import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditeComponent } from '../members/member-edite/member-edite.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditeComponent> {
  canDeactivate(
    component: MemberEditeComponent): boolean  {

      if(component.editeForm?.dirty)
      {
        return confirm('Ary you sure you want to continue? Any unsaved will be lost ')
      }
    return true;
  }
  
}
