import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Category } from 'src/app/_models/category';
import { Post } from 'src/app/_models/post';
import { AdminService } from 'src/app/_services/admin.service';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-admin-single-post',
  templateUrl: './admin-single-post.component.html',
  styleUrls: ['./admin-single-post.component.css']
})
export class AdminSinglePostComponent implements OnInit {
  @Input() post: Post;
  changePostMode:boolean = false;
  categories: Category[];
  model: any = {};
  nonUpdatedPost;
  addPostCheck:boolean;
  @Output() acceptPost = new EventEmitter<number>();
  @Output() deletePost = new EventEmitter<number>();

  constructor(private postService: PostService,private adminService:AdminService) { }

  ngOnInit(): void {
    this.model.categoryId=this.post.categoryId;
    this.nonUpdatedPost = {
      title:this.post?.title,
      content: this.post?.content  
    }
    this.GetCategories();
  }
  GoToChangeMod(){
      
    this.changePostMode=!this.changePostMode;
  }
  GoToNonChangeMod(){
    this.fillOldPost(this.nonUpdatedPost); 
    this.changePostMode=!this.changePostMode;
  }
  fillOldPost(nonUpdatedPost){
    this.post.content=nonUpdatedPost.content;
    this.post.title=nonUpdatedPost.title;
  }
  GetCategories() {
    
    this.postService.getcategories().subscribe(response =>{
      this.categories =response;
    })
  }
  ChangePostVisibility(){
    this.adminService.ChangePostVisibility(this.post.id).subscribe(response => {
      this.acceptPost.emit(this.post.id);
    })
  }
  DeletePost(){
    this.adminService.DeletePost(this.post.id).subscribe(response => {
      this.deletePost.emit(this.post.id);
    })
  }
  ChangePost(){
    if(!this.post.title || !this.post.content || !this.model.categoryId){
      this.addPostCheck=true;
      return;
    }
    this.addPostCheck=false;
    var updatePost = { 
      categoryId : this.model?.categoryId, 
      content : this.post?.content ,
      title :this.post?.title
  }  
    this.postService.UpdatePost(updatePost,this.post.id).subscribe(response =>{
    this.changePostMode=false;
    
    })
  }
  onChange($event) {
    this.post.categoryId=this.model.categoryId;
}

}
