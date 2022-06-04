import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from 'src/app/_models/pagination';
import { Post } from 'src/app/_models/post';
import { PostParams } from 'src/app/_models/PostParams';
import { AccountService } from 'src/app/_services/account.service';
import { AdminService } from 'src/app/_services/admin.service';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-admin-post',
  templateUrl: './admin-post.component.html',
  styleUrls: ['./admin-post.component.css']
})
export class AdminPostComponent implements OnInit {
  posts: Post[];
  postParams: PostParams;
  pagination: Pagination;


  constructor(public adminService: AdminService,public accountService: AccountService,private toastr:ToastrService) {
    this.postParams = this.adminService.getPostParams();
   }

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    this.adminService.setPostParams(this.postParams);

    this.adminService.getNonVisiblePosts(this.postParams).subscribe(response =>{
      this.posts =response.result;
     this.pagination=response.pagination;
     console.log(this.posts);
    })
    
  }
  pageChanged(event: any) {
    this.postParams.pageNumber = event.page;
    
    this.adminService.setPostParams(this.postParams);
    this.loadPosts();
  }
  RefReshPage(){
    this.loadPosts();
    this.toastr.success("change completed successfully")
  }



}
