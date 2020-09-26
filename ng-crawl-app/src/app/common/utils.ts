import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthResponseModel } from './models/auth-response-model';

export class Utils {

    public static Guid = class {
        public static Empty(): string {
            return '00000000-0000-0000-0000-000000000000';
        }
        public static New(): string {
            let output = '';
            const value = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx';

            // tslint:disable-next-line: prefer-for-of
            for (let index = 0; index < value.length; index++) {
                const c = value[index];

                if (c === '-' || c === '4') {
                    output += c;
                    continue;
                }

                // tslint:disable-next-line:no-bitwise
                const r = (Math.random() * 16) | 0;
                // tslint:disable-next-line:no-bitwise
                const v = c === 'x' ? r : (r & 0x3) | 0x8;
                output += v.toString(16);
            }

            return output;
        }
    };

    public static LocalStorage = class {

        public static Constants = class {
            public static DefaultAvatarUrl = 'assets/images/avatars/profile.jpg';
        };

        public static Set(key: string, data: any): boolean {
            localStorage.removeItem(key);
            if (data) {
                localStorage.setItem(key, JSON.stringify(data));
                return localStorage.getItem(key) != null;
            }
            return false;
        }

        public static Get(key: string): any {
            const data = localStorage.getItem(key);
            if (data) {
                return JSON.parse(data);
            }
            return null;
        }

        public static Remove(key: string): boolean {
            localStorage.removeItem(key);
            return localStorage.getItem(key) == null;
        }
    };

    public static GetCurrentUserToken(): string {
        const currentUserCreditionals = Utils.LocalStorage.Get('userData') as AuthResponseModel;
        if (currentUserCreditionals && currentUserCreditionals.BarerToken) {
            return currentUserCreditionals.BarerToken;
        }

        return null;
    }

    public static GetCurrentUserEmail(): string {
        const currentUserCreditionals = Utils.LocalStorage.Get('userData') as AuthResponseModel;
        if (currentUserCreditionals) {
            return currentUserCreditionals.UserEmail;
        }

        return null;
    }

    public static HasSessionExpiredOrNotSignedIn(): boolean {
        const currentUserCreditionals = Utils.LocalStorage.Get('userData') as AuthResponseModel;
        if (currentUserCreditionals && currentUserCreditionals.UserAppKey && currentUserCreditionals.UserEmail && currentUserCreditionals.BarerToken) {
            const helper = new JwtHelperService();
            if (!helper.isTokenExpired(currentUserCreditionals.BarerToken)) {
                console.log('User: ' + currentUserCreditionals.UserEmail + ' session remaining time: ' + helper.getTokenExpirationDate(currentUserCreditionals.BarerToken));
                return false;
            }
        }

        return true;
    }
}
