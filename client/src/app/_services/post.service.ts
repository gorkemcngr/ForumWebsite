import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category';
import { Comment } from '../_models/comment';
import { Pagination } from '../_models/pagination';
import { PostParams } from '../_models/PostParams';
import { Post } from '../_models/post';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { CommentParams } from '../_models/commentParams';
import { UpdatePost } from '../_models/updatePost';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  baseUrl = environment.apiUrl;
  private _posts = new ReplaySubject<Post[]>();
  posts$ = this._posts.asObservable();
  postParams: PostParams;
  commentParams: CommentParams;


  private _pagination = new Subject<Pagination>();
  pagination$ = this._pagination.asObservable();

  constructor(private http:HttpClient) { 
    this.postParams=new PostParams();
    this.commentParams = new CommentParams();
  }

  setPagination(pagination: Pagination)
  {  
    this._pagination.next(pagination);
  }

  setPostParams(params: PostParams) {
    this.postParams = params;
  }
  setCommentParams(params: CommentParams) {
    this.commentParams = params;
  }

  getPostParams() {
    return this.postParams;
  }
  getCommentParams() {
    return this.commentParams;
  }

   getAllPosts(paginationParams:PostParams) {
    let params = getPaginationHeaders(paginationParams.pageNumber, paginationParams.pageSize);
    if(paginationParams.categoryId >0){
      params = params.append('categoryId', paginationParams.categoryId.toString());
    }
    
    return getPaginatedResult<Post[]>(this.baseUrl + 'post', params, this.http).pipe(
      map((response)=>{
        this._posts.next(response.result);
        return response;
      })
    );;
    
  }
  getPost(id2:number) {
    return this.http.get<Post>(this.baseUrl + 'post/'+id2);
  }

  getAllComments(commentParams:CommentParams) {
    let params = getPaginationHeaders(commentParams.pageNumber, commentParams.pageSize);
    return getPaginatedResult<Comment[]>(this.baseUrl + 'post/comments/'+commentParams.postId, params, this.http).pipe(
      map((response)=>{
        return response;
      })
    );
  }
  getPendingComments(postParams:PostParams) {
    let params = getPaginationHeaders(postParams.pageNumber, postParams.pageSize);
    return getPaginatedResult<Post[]>(this.baseUrl + 'post/pending-post/', params, this.http).pipe(
      map((response)=>{
        return response;
      })
    );
  }
  getUsersPosts(postParams:PostParams) {
    let params = getPaginationHeaders(postParams.pageNumber, postParams.pageSize);
    return getPaginatedResult<Post[]>(this.baseUrl + 'post/user/'+postParams.userId, params, this.http).pipe(
      map((response)=>{
        return response;
      })
    );
  }
  getcategories() {
    return this.http.get<Category[]>(this.baseUrl + 'post/categories/');
  }
  AddComment(model:any,id:number){
    return this.http.post<Comment>(this.baseUrl + 'post/'+id, model);
  }
  AddPost(model:any){
    return this.http.post<Post>(this.baseUrl + 'post', model);
  }
  GetPostsWithCategory(id:number){
    return this.http.get<Post[]>(this.baseUrl + 'post/category/'+id).pipe(
      map((response:Post[])=>{
        this._posts.next(response);
        return response;
      })
    );
  }
  AddCommentLike(commentId:number){
    return this.http.post(this.baseUrl + 'post/commentLike/'+commentId, {});
  }
  GetCommentLikes(){
    return this.http.get<Comment[]>(this.baseUrl +'post/commentLike');
  }
  GetCommentLikesWithId(commentId:number){
    return this.http.get<User[]>(this.baseUrl +'post/commentLike/'+commentId);
  }
  UpdatePost(updatepost: UpdatePost,postId:number)
  {
    return this.http.put(this.baseUrl+'post/'+postId,updatepost);
  }
  GetCommentsUserLikes(commentParams:CommentParams){
    let params = getPaginationHeaders(commentParams.pageNumber, commentParams.pageSize);
    return getPaginatedResult<Comment[]>(this.baseUrl + 'post/commentUser/'+commentParams.userId, params, this.http).pipe(
      map((response)=>{
        return response;
      })
    );
  }
}
