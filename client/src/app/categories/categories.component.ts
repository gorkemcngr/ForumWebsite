import { Component, OnInit } from '@angular/core';
import { Category } from '../_models/category';
import { Pagination } from '../_models/pagination';
import { PostParams } from '../_models/PostParams';
import { PostService } from '../_services/post.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
  categories: Category[];
  oldCategoryId:number=1;
  paginationParams: PostParams;
  pagination: Pagination;
  
  
  constructor(private postService: PostService) {
    this.paginationParams = this.postService.getPostParams();

   }

  ngOnInit(): void {
    this.GetCategories();
  }

  GetCategories() {
    this.postService.getcategories().subscribe(response =>{
      this.categories =response;
    })
  }

  NoCategory(){
    this.paginationParams.categoryId=null;
    this.GetPostsWithCategory();
    var category:Category= this.categories.find(c => c.isActive ===true);
    category.isActive=false;
  }

  GetPostsWithCategory(categoryId?:number){
    if( this.paginationParams.categoryId!==null){
      this.isActive(categoryId);
    }
    
    this.paginationParams.categoryId=categoryId;
    this.postService.setPostParams(this.paginationParams);

    this.postService.getAllPosts(this.paginationParams).subscribe(response => {
      this.pagination=response.pagination;
      this.postService.setPagination(this.pagination);
    });
    
  }

  isActive(categoryId:number){
    if(this.oldCategoryId===categoryId){

      return;
    }
   var category:Category= this.categories.find(c => c.id ===categoryId);
   var oldCategory:Category= this.categories.find(c => c.id ===this.oldCategoryId);
  category.isActive=true;
  oldCategory.isActive=false;
  this.oldCategoryId=categoryId;
  }

}
