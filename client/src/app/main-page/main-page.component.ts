import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Category } from '../_models/category';
import { Pagination } from '../_models/pagination';
import { PostParams } from '../_models/PostParams';
import { Post } from '../_models/post';
import { AccountService } from '../_services/account.service';
import { PostService } from '../_services/post.service';
import { User } from '../_models/user';

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

  constructor(public postService: PostService,private route:Router,public accountService: AccountService) {
    this.postParams = this.postService.getPostParams();
   }

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user'));
    this.addPostCheck=false;
    this.loadPosts();
    this.GetCategories();
  }
  changeWebsite(e) {  
    console.log(e.target.value);  
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
    console.log(this.model);
    this.postService.AddPost(this.model).subscribe(response => {
      this.posts.push(response);
      this.route.navigate(['/comment/'+response.id]);
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

}