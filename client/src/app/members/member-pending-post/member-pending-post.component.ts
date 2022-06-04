import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/_models/pagination';
import { Post } from 'src/app/_models/post';
import { PostParams } from 'src/app/_models/PostParams';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-member-pending-post',
  templateUrl: './member-pending-post.component.html',
  styleUrls: ['./member-pending-post.component.css']
})
export class MemberPendingPostComponent implements OnInit {
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
    this.postService.setPostParams(this.postParams);

    this.postService.getPendingComments(this.postParams).subscribe(response =>{
      this.posts =response.result;
     this.pagination=response.pagination;
     console.log(this.posts);
    })
    
  }
  pageChanged(event: any) {
    this.postParams.pageNumber = event.page;
    
    this.postService.setPostParams(this.postParams);
    this.loadPosts();
  }

}
