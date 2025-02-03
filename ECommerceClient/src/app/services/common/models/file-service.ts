import { Injectable } from '@angular/core';
import {HttpClientService} from '../http-client.service';
import {firstValueFrom, Observable} from 'rxjs';
import {BaseUrl} from '../../../contracts/base-url';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  constructor(private httpClientService: HttpClientService) {
  }

  async getBaseUrlStorage(): Promise<BaseUrl> {
    const getObservable: Observable<BaseUrl> = this.httpClientService.get<BaseUrl>({
      controller: "files",
      action: "getbaseurlstorage"
    });
    return await firstValueFrom(getObservable);
  }
}
