import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Category } from '../_models/category';
import { UpdatePost } from '../_models/updatePost';
import { Comment } from '../_models/comment';
import { CommentParams } from '../_models/commentParams';
import { Pagination } from '../_models/pagination';
import { Post } from '../_models/post';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { PostService } from '../_services/post.service';
import { RolesModalComponent } from '../modals/roles-modal/roles-modal.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { LikesModalComponent } from '../modals/likes-modal/likes-modal.component';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  comments:Comment[];
  id: number;
  post: Post;
  model: any = {}
  commentsLikes: Comment[];
  likeUsers:User[];
  hearthClass: string='fa fa-heart-o';
  commentParams: CommentParams;
  pagination: Pagination;
  categories: Category[];
  changePostMode:boolean = false;
  updatePost : UpdatePost;
  addPostCheck:boolean;
  user: User;
  nonUpdatedPost;
  bsModalRef: BsModalRef;
  
  
  constructor(private postService: PostService,private route: ActivatedRoute, public accountService: AccountService,private modalService: BsModalService) {
    this.commentParams = this.postService.getCommentParams();
   }

  ngOnInit(): void {
    

    this.GetCategories();
    this.user = JSON.parse(localStorage.getItem('user'));
    this.addPostCheck=false;
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.id = +params.get('id')
    })
    if(JSON.parse(localStorage.getItem('user')) !== null){
      this.GetCommentLikes();
    }
    
    this.loadComments(this.id);
    this.loadPost(this.id);

    
  }
  loadPost(id2:number){
    this.postService.getPost(id2).subscribe(response => {
      this.post = response;
      this.nonUpdatedPost = {
        title:this.post?.title,
        content: this.post?.content  
      }
    })
  }
  AddComment(form: NgForm){

    this.postService.AddComment(this.model,this.id).subscribe(response => {    
      this.comments.push(response);
      this.comments.find(x => x.id===response.id).likeCount=0;
      form.resetForm();
      this.pageChanged(Math.floor( this.pagination.totalItems/this.pagination.itemsPerPage )+1);
      this.pagination.totalItems+=1;
    });
 
  }
  GoToChangeMod(){
    this.model.categoryId=this.post.categoryId;
    this.changePostMode=!this.changePostMode;
  }
  GoToNonChangeMod(){
    this.fillOldPost(this.nonUpdatedPost); 
    this.changePostMode=!this.changePostMode;
  }
  GetCategories() {
    this.postService.getcategories().subscribe(response =>{
      this.categories =response;
    })
  }
  fillOldPost(nonUpdatedPost){
    this.post.content=nonUpdatedPost.content;
    this.post.title=nonUpdatedPost.title;
    console.log("asdfasdf");
  }

   float2int (value) {
    
    return Math.floor( value )
}

openRolesModal(user: User) {
  const config = {
    class: 'modal-dialog-centered',
    initialState: {
      users:this.likeUsers
    }
  }
  this.bsModalRef = this.modalService.show(LikesModalComponent, config);
  this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
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
      this.loadPost(this.id);
      })
    }

  loadComments(id:number) {
    this.commentParams.postId=this.id;
    this.postService.setCommentParams(this.commentParams);
    this.postService.getAllComments(this.commentParams).subscribe(response =>{
      this.comments =response.result;
      this.pagination=response.pagination
      this.comments?.forEach(element => {
        this.GetCommentLikesWithId(element?.id);
      });
    })
  }
  AddCommentLike(comment) {
    this.postService.AddCommentLike(comment.id).subscribe(response => {
      if(this.commentsLikes.find(c => c.id === comment.id)){
        this.commentsLikes=this.commentsLikes.filter(c => c.id !== comment.id);
        this.comments.find(x => x.id===comment.id).likeCount=0;
        this.GetCommentLikesWithId(comment.id);
      }else{
        this.commentsLikes.push(comment);
        this.GetCommentLikesWithId(comment.id);
      }});
  }
  GetCommentLikes() {
    
    this.postService.GetCommentLikes().subscribe(response =>{
        this.commentsLikes=response;  
    });
    
    
  }
  GetCommentIsLike(commentId:number){

    var islike= this.commentsLikes?.find(c => c.id===commentId);
    if(islike) return this.hearthClass='fa fa-heart'; 
    else return this.hearthClass='fa fa-heart-o'; 
  }
  GetCommentLikesWithId(commentId:number){ ////kullanılmıyor
    this.postService.GetCommentLikesWithId(commentId).subscribe(response=>{
      this.likeUsers=response;
     this.comments.find(x => x.id===commentId).likeCount=this.likeUsers?.length;

      return this.likeUsers?.length;
    });

   
  }
  pageChanged(event: any) {
    if(typeof event === 'number'){
      this.commentParams.pageNumber = event;
    }else{
      this.commentParams.pageNumber = event.page;
    }
    
    
    this.postService.setCommentParams(this.commentParams);
    this.loadComments(this.id);
  }

}
