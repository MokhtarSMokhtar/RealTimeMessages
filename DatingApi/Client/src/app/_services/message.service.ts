import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPagination, getPaginationHeader } from './paginationHelper';
import { Message } from '../_models/message';
import { BehaviorSubject, retry, take } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
baseUrl = environment.apiUrl;
hubUrl = environment.hubUrl;
private messageThreadSource = new BehaviorSubject<Message[]>([]);
messageThread$ = this.messageThreadSource.asObservable();
private hubConnection?:HubConnection;

  constructor(private http:HttpClient) {

  }

  createHubConnection(user: User ,otherUsername:string)
  {
      this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user='+otherUsername,{
        accessTokenFactory :()=> user.token
      })
      .withAutomaticReconnect()
      .build();
      this.hubConnection.start().catch(err=> console.log(err))

      this.hubConnection.on('ReceiveMessageThread',message=> {
          this.messageThreadSource.next(message);
      })

      this.hubConnection.on('NewMessage',message=>{
        this.messageThread$.pipe(take(1)).subscribe(mess=>
          this.messageThreadSource.next([...mess,message])
        )
      })
  }

  getMessages(pageNumber:number,pageSize:number,container:string)
  {


    let param = getPaginationHeader(pageNumber,pageSize);
    param =  param.append('Container',container);
    return getPagination<Message>(this.baseUrl + 'messages', param,this.http);
  }

 stopHubConnection()
   {
    if(this.hubConnection)
    {
      this.hubConnection?.stop().catch(err=> console.log(err));

    }
   }

  getMessageThread(username:string)
  {
      return this.http.get<Message[]>(this.baseUrl +'messages/thread/'+ username)
  }


  async sendMessage(username:string,content:string)
  {
    return this.hubConnection?.invoke('SendMessage',{recipientUsername:username,content})
    .catch(err=> console.log(err));

  }
  deleteMessage(id:number)
  {
    return this.http.delete(this.baseUrl + 'Messages/' + id )
  }
}
