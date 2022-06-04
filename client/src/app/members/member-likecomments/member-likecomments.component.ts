import { Component, Input, OnInit } from '@angular/core';
import { Comment } from 'src/app/_models/comment';
import { CommentParams } from 'src/app/_models/commentParams';
import { User } from 'src/app/_models/user';
import { PostService } from 'src/app/_services/post.service';
import { Pagination } from '../../_models/pagination';

@Component({
  selector: 'app-member-likecomments',
  templateUrl: './member-likecomments.component.html',
  styleUrls: ['./member-likecomments.component.css']
})
export class MemberLikecommentsComponent implements OnInit {
  comments: Comment[];
  likeUsers:User[];
  @Input() userId: number;
  commentParams: CommentParams;
  pagination: Pagination;
  
  constructor(private postService: PostService) {
    this.commentParams = this.postService.getCommentParams();
   }

  ngOnInit(): void {
    this.GetCommentsUserLikes();
  }

  GetCommentsUserLikes(){
    this.commentParams.userId=this.userId;
    this.postService.setCommentParams(this.commentParams);
    this.postService.GetCommentsUserLikes(this.commentParams).subscribe(response=>{
      this.comments=response.result;
      this.pagination=response.pagination;
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
    this.GetCommentsUserLikes();
  }

}
