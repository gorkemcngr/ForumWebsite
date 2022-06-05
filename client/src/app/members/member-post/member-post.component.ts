import { Component, Input, OnInit } from '@angular/core';
import { Pagination } from 'src/app/_models/pagination';
import { Post } from 'src/app/_models/post';
import { PostParams } from 'src/app/_models/PostParams';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-member-post',
  templateUrl: './member-post.component.html',
  styleUrls: ['./member-post.component.css']
})
export class MemberPostComponent implements OnInit {
  @Input() userId: number;
  posts: Post[];
  pagination: Pagination;
  postParams: PostParams;

  constructor(public postService: PostService) {
    this.postParams = this.postService.getPostParams();
   }

   ngOnInit(): void {
    this.loadPosts();
  }
  loadPosts() {
    this.postParams.userId=this.userId;
    this.postService.setPostParams(this.postParams);

    this.postService.getUsersPosts(this.postParams).subscribe(response =>{
      this.posts =response.result;
     this.pagination=response.pagination;
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
