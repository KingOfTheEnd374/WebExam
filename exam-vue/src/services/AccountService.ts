import type { IUserInfo } from "./IUserInfo";
import type { IResultObject } from "./IResultObject"
import axios from "axios";

export default class AccountService {
    private constructor() {
        
    }

    private static httpClient = axios.create({
        baseURL: "http://localhost:5275/api/v1/identity/Account"
    });

    static async login(email: string, pwd: string) : Promise<IResultObject<IUserInfo>> {
        const loginData = {
            email: email,
            password: pwd
        };
        try {
            const response = await AccountService.httpClient.post<IUserInfo>("login", loginData);
            if (response.status < 300) {
                return {
                    data: response.data
                };
            }
            return  {
                errors: [response.status.toString() + " " + response.statusText]
            };
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }

    static async register(email: string, pwd: string, userName: string) : Promise<IResultObject<IUserInfo>> {
        const registerData = {
            email: email,
            password: pwd,
            userName: userName
        };
        try {
            const response = await AccountService.httpClient.post<IUserInfo>("register", registerData);
            if (response.status < 300) {
                return {
                    data: response.data
                };
            }
            return  {
                errors: [response.status.toString() + " " + response.statusText]
            };
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }

    static async refreshToken(jwt: string, refreshToken: string) : Promise<IResultObject<IUserInfo>> {
        const refreshData = {
            jwt: jwt,
            refreshToken: refreshToken
        };
        try {
            const response = await AccountService.httpClient.post<IUserInfo>("refreshtoken", refreshData);
            if (response.status < 300) {
                return {
                    data: response.data
                };
            }
            return  {
                errors: [response.status.toString() + " " + response.statusText]
            };
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }
}