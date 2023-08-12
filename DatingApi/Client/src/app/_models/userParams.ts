import {User} from './user'
export class UserParams
{
    gender:string;
    minAge=30;
    maxAge=50
    pageNumber=1;
    pageSize=3;
    orderBy = 'lastActive'
    constructor(user:User) {
       this.gender = user.gender 
       console.log(this.gender)
    }
}