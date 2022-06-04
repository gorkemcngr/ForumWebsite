import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category';
import { Post } from '../_models/post';
import { PostParams } from '../_models/PostParams';
import { UpdatePost } from '../_models/updatePost';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  postParams: PostParams;

  constructor(private http: HttpClient) {
    this.postParams=new PostParams();
   }

  getUsersWithRoles() {
    return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles');
  }
  getUsersNameWithRoles(username:string) {
    return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles/'+username);
  }
  updateUserRoles(username: string, roles: string[]) {
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }
  ChangePostVisibility(postId:number)
  {
    return this.http.put(this.baseUrl+'admin/post-visibility/'+postId,{});
  }
  DeletePost(postId:number)
  {
    return this.http.delete(this.baseUrl+'admin/post/'+postId);
  }
  DeleteCategory(categoryId:number)
  {
    return this.http.delete(this.baseUrl+'admin/category/'+categoryId);
  }
  getNonVisiblePosts(paginationParams:PostParams) {
    let params = getPaginationHeaders(paginationParams?.pageNumber, paginationParams?.pageSize);
    return getPaginatedResult<Post[]>(this.baseUrl + 'admin/post', params, this.http).pipe(
      map((response)=>{
        return response;
      })
    );;
    
  }
  getPostParams() {
    return this.postParams;
  }
  setPostParams(params: PostParams) {
    this.postParams = params;
  }
  addcategory(categoryName: string) {
    let params = new HttpParams();

      params = params.append('categoryName', categoryName.toString());
    return this.http.post<Category>(this.baseUrl + 'admin/category',{},{params});
  }
}
