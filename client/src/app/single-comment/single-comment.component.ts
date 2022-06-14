import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { LikesModalComponent } from '../modals/likes-modal/likes-modal.component';
import { Comment } from '../_models/comment';
import { Post } from '../_models/post';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { PostService } from '../_services/post.service';

@Component({
  selector: 'app-single-comment',
  templateUrl: './single-comment.component.html',
  styleUrls: ['./single-comment.component.css']
})
export class SingleCommentComponent implements OnInit {
  @Input() comment: Comment;
  @Input() user: User;
  @Input() post: Post;
  usersComments: Comment[];
  commentsLikes: Comment[];
  hearthClass: string='fa fa-heart-o';
  likeUsers:User[];
  bsModalRef: BsModalRef;
  @Output() deleteComment = new EventEmitter<number>();

  constructor(public accountService: AccountService,private postService: PostService,private modalService: BsModalService) { }

  ngOnInit(): void {
    console.log(this.post.userName);
    console.log(this.comment.username);
    if(JSON.parse(localStorage.getItem('user')) !==null){
      this.GetUsersComments();
      
      this.GetCommentLikes();
    }
    this.GetCommentLikesWithId(this.comment.id);
  }

  IsUserCommentContain(commentId:number){
    if(this.usersComments?.find(x => x.id === commentId)) return true;
    return false;
}

GetUsersComments(){
  this.postService.GetCommentWithUserIdWitPostId(this.post.id).subscribe(response => {
    this.usersComments = response;
  })
}
GetCommentLikes() {
  this.postService.GetCommentLikes().subscribe(response =>{
      this.commentsLikes=response;  
      console.log(response);
  });
}
DeleteComment(){
  this.postService.DeleteComment(this.comment.id).subscribe(response => {
    this.deleteComment.emit(this.comment.id);
  })
}
AddCommentLike(comment) {
  this.postService.AddCommentLike(comment.id).subscribe(response => {
    if(this.commentsLikes.find(c => c.id === comment.id)){
      this.commentsLikes=this.commentsLikes.filter(c => c.id !== comment.id);
      this.GetCommentLikesWithId(comment.id);
    }else{
      this.commentsLikes.push(comment);
      this.GetCommentLikesWithId(comment.id);
    }});
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

GetCommentIsLike(commentId:number){

  var islike= this.commentsLikes?.find(c => c.id===commentId);
  if(islike) return this.hearthClass='fa fa-heart'; 
  else return this.hearthClass='fa fa-heart-o'; 
}
 GetCommentLikesWithId(commentId:number){ ////kullanılmıyor
    this.postService.GetCommentLikesWithId(commentId).subscribe(response=>{
      
      this.likeUsers=response;
    });

   
  }
  

}
