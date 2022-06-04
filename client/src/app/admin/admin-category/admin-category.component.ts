import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Category } from 'src/app/_models/category';
import { AdminService } from 'src/app/_services/admin.service';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-admin-category',
  templateUrl: './admin-category.component.html',
  styleUrls: ['./admin-category.component.css']
})
export class AdminCategoryComponent implements OnInit {
  categories: Category[];
  newCategoryName:string;

  constructor(private postService: PostService,private adminService: AdminService,private toastr:ToastrService) { }

  ngOnInit(): void {
    this.GetCategories();
  }

  GetCategories() {
    this.postService.getcategories().subscribe(response =>{
      this.categories =response;
      console.log("asdff");
    })
  }
  AddCategory(){
    this.adminService.addcategory(this.newCategoryName).subscribe(response => {
      this.categories.push(response);
      this.newCategoryName=null;
    })
  }
  DeleteCategory(categoryId: number){
    this.adminService.DeleteCategory(categoryId).subscribe(response => {
      this.categories=this.categories.filter(x => x.id !==categoryId);
      this.toastr.success("category successfully deleted")
    })
  }

}
