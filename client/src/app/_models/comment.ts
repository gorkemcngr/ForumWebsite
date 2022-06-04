export interface Comment {
    id:number;
    postId:number;
    answer: string;
    username: string;
    likeCount: number;
    created: Date;
    photoUrl: string;
 }