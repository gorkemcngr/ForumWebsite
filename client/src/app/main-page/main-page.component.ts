import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Category } from '../_models/category';
import { Pagination } from '../_models/pagination';
import { PostParams } from '../_models/PostParams';
import { Post } from '../_models/post';
import { AccountService } from '../_services/account.service';
import { PostService } from '../_services/post.service';
import { User } from '../_models/user';
import { AdminService } from '../_services/admin.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {
  posts: Post[];
  model: any = {};
  pagination: Pagination;
  categories: Category[];
  postParams: PostParams;
  addPostCheck:boolean;
  user: User;
  mymodel;

  constructor(public postService: PostService,private route:Router,
    public accountService: AccountService,private adminService: AdminService, private toastr: ToastrService) {
    this.postParams = this.postService.getPostParams();
   }

  ngOnInit(): void {
    
    
    this.addPostCheck=false;
    this.loadPosts();
    this.GetCategories();
     this.accountService.currentUser$.subscribe(response => {
      this.user = response;
    })
    
  }
  changeWebsite(e) {  

  }  

  loadPosts() {
    this.postService.setPostParams(this.postParams);
    if(this.posts?.length !== 0){
      this.postService.posts$.subscribe(response=>{
        this.posts=response;
      })
    }
    this.postService.getAllPosts(this.postParams).subscribe(response =>{
      this.posts =response.result;
     this.pagination=response.pagination;
      this.postService.setPagination(response?.pagination);
      this.postService.pagination$.subscribe(result => {
        this.pagination=result;
      });
    })
    
  }
  valuechange(newValue) {
    this.mymodel = newValue;
    this.pagination.currentPage=1;
    this.postParams.postTitle=newValue;
    this.loadPosts();
    this.postParams.postTitle=null;
  }
  GetCategories() {
    this.postService.getcategories().subscribe(response =>{
      this.categories =response;
      this.model.categoryId=this.categories[0].id;
    })
  }

  AddPost(){
    if(!this.model.title || !this.model.content || !this.model.categoryId){
      this.addPostCheck=true;
      return;
    }

    this.postService.AddPost(this.model).subscribe(response => {
      this.posts.push(response);
      this.route.navigate(['/comment/'+response.id]);
    })
  }
  deletePost(postId: number){
    this.adminService.DeletePost(postId).subscribe(response => {
      this.posts = this.posts.filter(x => x.id !==postId);
      this.toastr.success("Post Deleted successfully");
      this.loadPosts();
    })
  }

  pageChanged(event: any) {
    this.postParams.pageNumber = event.page;
    
    this.postService.setPostParams(this.postParams);
    this.loadPosts();
  }

  longContent(content:string){
    return content.substring(0, 150).concat("...").toString();
  }
  mediumLongcontent(content:string){
    return content.substring(0, 40).concat("...").toString();
  }

}
