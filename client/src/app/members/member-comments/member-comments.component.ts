import { Component, Input, OnInit } from '@angular/core';
import { Comment } from 'src/app/_models/comment';
import { CommentParams } from 'src/app/_models/commentParams';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-member-comments',
  templateUrl: './member-comments.component.html',
  styleUrls: ['./member-comments.component.css']
})
export class MemberCommentsComponent implements OnInit {
  comments: Comment[];
  likeUsers:User[];
  @Input() userName: string;
  commentParams: CommentParams;
  pagination2: Pagination;

  constructor(private postService: PostService) {
    this.commentParams = this.postService.getCommentParams();
   }

  ngOnInit(): void {
    this.GetUsersComments();
  }

  GetUsersComments(){
    this.commentParams.postId=this.userName;
    this.postService.setCommentParams(this.commentParams);
    this.postService.getAllComments(this.commentParams).subscribe(response=>{
      this.comments=response.result;
      this.pagination2=response.pagination;
      this.comments?.forEach(element => {
        this.GetCommentLikesWithId(element?.id);
      });
    })
  }

  GetCommentLikesWithId(commentId:number){ ////kullanılmıyor
    this.postService.GetCommentLikesWithId(commentId).subscribe(response=>{
      this.likeUsers=response;
     this.comments.find(x => x.id===commentId).likeCount=this.likeUsers?.length;

      return this.likeUsers?.length;
    });

   
  }
  pageChanged(event: any) {
  
      this.commentParams.pageNumber = event.page;
    this.postService.setCommentParams(this.commentParams);
    this.GetUsersComments();
  }

}
