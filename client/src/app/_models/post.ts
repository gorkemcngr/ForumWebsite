export interface Post {
   id: number;
   title: string;
   content: string;
   userName: string;
   categoryName: string;
   categoryId: number;
   created: Date;
   photoUrl: string;
   postVisibility: boolean;
}