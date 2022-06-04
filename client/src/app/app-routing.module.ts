import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { CommentComponent } from './comment/comment.component';

import { MainPageComponent } from './main-page/main-page.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MessagesComponent } from './messages/messages.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { RegisterComponent } from './register/register.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path: '', component: MainPageComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'member/edit', component: MemberEditComponent, canActivate: [AuthGuard]},
  {path: 'comment/:id', component: CommentComponent},
  {path: 'detail/:username', component: MemberDetailComponent},
  {path: 'messages',component: MessagesComponent},
  {path: 'admin',component: AdminPanelComponent, canActivate: [AdminGuard,AuthGuard]},
  {path: '**',component: NotFoundComponent, pathMatch: 'full'},
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
