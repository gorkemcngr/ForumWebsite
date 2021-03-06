import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  activeTab: TabDirective;
  member: Member;
  username:string;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  messages: Message[] = [];
  user: User;

  constructor(private memberService: MembersService, private route: ActivatedRoute, 
    private messageService: MessageService,public accountService:AccountService,private router: Router, public presence: PresenceService ) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
      this.router.routeReuseStrategy.shouldReuseRoute = () =>false;
    }
 

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.username = params.get('username');
    });
    this.getUserWithUsername(this.username);

    
  }
  getImages():NgxGalleryImage[] {
    const imageUrls = [];
    for(const photo of this.member.photos){
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      })
    }
    return imageUrls;
  }
  galeryUploads(){
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]

    this.galleryImages = this.getImages();
    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })
  }

  getUserWithUsername(username:string){
    this.memberService.getUserWithUsername(username).subscribe(response => {
      this.member = response;
     this.galeryUploads();
    })
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.messageService.createHubConnection(this.user,this.member.username);
    } else{
      this.messageService.stopHubConnection();
    }
  }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  loadMessages() {
    this.messageService.getMessageThread(this.member.username).subscribe(messages => {
      this.messages = messages;
    })
  }
  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }
  

}
