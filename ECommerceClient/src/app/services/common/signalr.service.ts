import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder, HubConnectionState} from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private _connection: HubConnection;
  get connection(): HubConnection {
    return this._connection;
  }

  start(hubUrl: string) {
    if(!this.connection || this._connection?.state == HubConnectionState.Disconnected) {
      const builder: HubConnectionBuilder = new HubConnectionBuilder();

      const hubConnection: HubConnection = builder.withUrl(hubUrl).withAutomaticReconnect().build();

      hubConnection.start()
        .then(() => console.log('Connection started'))
        .catch(err => setTimeout(() => this.start(hubUrl), 2000));
      this._connection = hubConnection;
    }
    this._connection.onreconnected(connectionId => console.log('Reconnected'));
    this._connection.onreconnecting(error => console.log('Reconnecting'));
    this._connection.onclose(error => console.log('Connection closed'));
  }

  invoke(procedureName: string, message: any, successCallback?: (value) => void, errorCallback?: (error) => void) {
    this.connection.invoke(procedureName, message).then(successCallback).catch(errorCallback);
  }

  on(procedureName: string, callback: (...message: any) => void) {
    this.connection.on(procedureName, callback);
  }
}
