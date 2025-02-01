import {Injectable} from '@angular/core';
import {firstValueFrom, Observable} from 'rxjs';
import {TokenResponse} from '../../../contracts/token/tokenResponse';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../../ui/custom-toastr.service';
import {SocialUser} from '@abacritt/angularx-social-login';
import {HttpClientService} from '../http-client.service';

@Injectable({
  providedIn: 'root'
})
export class UserAuthService {
  constructor(private httpClientService: HttpClientService, private toastrService: CustomToastrService) {
  }

  async login(userNameOrEmail: string, password: string, callBackFunction?: () => void): Promise<any> {
    const observable: Observable<any | TokenResponse> = this.httpClientService.post<any | TokenResponse>({
      controller: 'auth',
      action: 'login'
    }, {userNameOrEmail, password});

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;
    if (tokenResponse) {
      // console.log(tokenResponse);
      // console.log(tokenResponse.token.accessToken);
      // console.log(tokenResponse.token.expiration);

      localStorage.setItem('token', tokenResponse.token.accessToken);
      localStorage.setItem('refreshToken', tokenResponse.token.refreshToken);
      // localStorage.setItem('expiration', tokenResponse.token.expiration.toString());

      this.toastrService.message('Login successful', 'success', {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      });
    }
    callBackFunction();
  }

  async refreshTokenLogin(refreshToken: string, callBackFunction?: () => void): Promise<any> {
    const observable: Observable<any | TokenResponse> = this.httpClientService.post({
      action: 'refreshtokenlogin',
      controller: 'auth'
    }, {refreshToken: refreshToken});

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;
    if (tokenResponse) {
      localStorage.setItem('token', tokenResponse.token.accessToken);
      localStorage.setItem('refreshToken', tokenResponse.token.refreshToken);
    }
    callBackFunction();
  }

  async googleLogin(user: SocialUser, callBackFunction?: () => void): Promise<any> {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      action: 'googlelogin',
      controller: 'auth',
    }, user);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;
    if (tokenResponse) {
      localStorage.setItem('token', tokenResponse.token.accessToken);
      localStorage.setItem('refreshToken', tokenResponse.token.refreshToken);
      this.toastrService.message('Google Login successful', 'success', {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      });
    }
    callBackFunction();
  }

  async facebookLogin(user: SocialUser, callBackFunction?: () => void): Promise<any> {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      action: 'facebooklogin',
      controller: 'auth',
    }, user);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;
    if (tokenResponse) {
      localStorage.setItem('token', tokenResponse.token.accessToken);
      localStorage.setItem('refreshToken', tokenResponse.token.refreshToken);
      this.toastrService.message('Facebook Login successful', 'success', {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      });
    }
    callBackFunction();
  }
}
