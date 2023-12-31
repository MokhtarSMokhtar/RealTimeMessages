import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditeComponent } from './members/member-edite/member-edite.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailedResolver } from './_resolver/member-detailed.resolver';
import { AdminPanalComponent } from './admin/admin-panal/admin-panal.component';
import { AdminGuard } from './_guards/admin.guard';

const routes: Routes = [
  {path:'',component:HomeComponent},
  {path:'',
    runGuardsAndResolvers:'always',
    canActivate:[AuthGuard],
    children:[
      {path:'members',component:MemberListComponent},
      {path:'members/:username',component:MemberDetailComponent,resolve:{member:MemberDetailedResolver}},
      {path:'member/edit',component:MemberEditeComponent,canDeactivate :[PreventUnsavedChangesGuard]},
      {path:'lists',component:ListsComponent},
      {path:'messages',component:MessagesComponent},
      {path:'members',component:MemberListComponent},
      {path:'admin',component:AdminPanalComponent,canActivate:[AdminGuard]},
    ]
  },
  {path:'error',component:TestErrorComponent,pathMatch:'full'},
  {path:'not-found',component:NotFoundComponent},
  {path:'server-error',component:ServerErrorComponent},
  {path:'**',component:NotFoundComponent,pathMatch:'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
