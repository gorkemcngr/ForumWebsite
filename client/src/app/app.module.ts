import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { MainPageComponent } from './main-page/main-page.component';
import { RegisterComponent } from './register/register.component';
import {TabsModule} from 'ngx-bootstrap/tabs';
import { ToastrModule } from 'ngx-toastr';
import { FileUploadModule } from 'ng2-file-upload';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { CommentComponent } from './comment/comment.component';
import { CategoriesComponent } from './categories/categories.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { TimeagoModule } from 'ngx-timeago';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MemberLikecommentsComponent } from './members/member-likecomments/member-likecomments.component';
import { MemberCommentsComponent } from './members/member-comments/member-comments.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { MessagesComponent } from './messages/messages.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { HasRoleDirective } from './has-role.directive';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { AdminRoleComponent } from './admin/admin-role/admin-role.component';
import { RolesModalComponent } from './modals/roles-modal/roles-modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AdminPostComponent } from './admin/admin-post/admin-post.component';
import { AdminSinglePostComponent } from './admin/admin-single-post/admin-single-post.component';
import { MemberPendingPostComponent } from './members/member-pending-post/member-pending-post.component';
import { MemberPostComponent } from './members/member-post/member-post.component';
import { AdminCategoryComponent } from './admin/admin-category/admin-category.component';
import { LikesModalComponent } from './modals/likes-modal/likes-modal.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { SingleCommentComponent } from './single-comment/single-comment.component';
import { FooterComponent } from './footer/footer.component';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    MainPageComponent,
    RegisterComponent,
    CommentComponent,
    CategoriesComponent,
    MemberDetailComponent,
    MemberLikecommentsComponent,
    MemberCommentsComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    NotFoundComponent,
    MessagesComponent,
    MemberMessagesComponent,
    HasRoleDirective,
    AdminPanelComponent,
    AdminRoleComponent,
    RolesModalComponent,
    AdminPostComponent,
    AdminSinglePostComponent,
    MemberPendingPostComponent,
    MemberPostComponent,
    AdminCategoryComponent,
    LikesModalComponent,
    SingleCommentComponent,
    FooterComponent,
    

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    FileUploadModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    ModalModule.forRoot(),
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    TimeagoModule.forRoot(),
    PaginationModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    })
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
