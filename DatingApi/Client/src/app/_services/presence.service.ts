import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
hubUrl = environment.hubUrl;
private hubConnection?:HubConnection;
private OnlineUsersSourse= new BehaviorSubject<string[]>([]);
onlineUsers$ = this.OnlineUsersSourse.asObservable();
  constructor(private tosatr:ToastrService) {
   }
   createHubConnection(user:User)
   {
      this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl+ 'presence',{
        accessTokenFactory:()=> user.token
      })
      .withAutomaticReconnect()
      .build();

      this.hubConnection.start().catch(err=> console.log(err))

      this.hubConnection.on('UserIsOnline',username=> {
        this.tosatr.success (username + 'has connected')
      })

      this.hubConnection.on('UserIsOffline',userName=> {
        this.tosatr.success(userName + 'has disconnected')
      })
      this.hubConnection.on('GetOnlineUsers',userName=>{
          this.OnlineUsersSourse.next(userName);
      })
    }

   stopHubConnection()
   {
       this.hubConnection?.stop().catch(err=> console.log(err));
   }
}
