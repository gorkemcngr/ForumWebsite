import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-likes-modal',
  templateUrl: './likes-modal.component.html',
  styleUrls: ['./likes-modal.component.css']
})
export class LikesModalComponent implements OnInit {
  users: User[];
  @Input() updateSelectedRoles = new EventEmitter();
  constructor(public bsModalRef: BsModalRef,private router: Router) { }
  userr: User;
  ngOnInit(): void {
    this.userr=JSON.parse(localStorage.getItem('user'));
  }
  routeUserDetail(username:string){
    this.router.navigate(['/detail/'+username]);
    this.bsModalRef.hide();
  }
  routeUserEdit(){
    this.router.navigate(['/member/edit']);
    this.bsModalRef.hide();
  }

}
