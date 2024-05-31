import axios from "axios";
import type { IResultObject } from "./IResultObject";
import type { IUserHomework } from "@/domain/IUserHomework";
import type { IUser } from "@/domain/IUser";

export default class UserHomeworkService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: "http://localhost:5275/api/v1/UserHomework"
    });

    static async getAll(jwt: string) : Promise<IResultObject<IUserHomework[]>>{
        try {
            const response = await UserHomeworkService.httpClient.get<IUserHomework[]>("", {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });

            if (response.status < 300) {
                return {
                    data: response.data
                }
            }
            return {
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }

    static async setGrade(jwt: string, userId: string, homeworkId: string, grade: number): Promise<IResultObject<null>> {
        try {
            const response = await UserHomeworkService.httpClient.post("user:" + userId + "/hw:" + homeworkId + "/grade:" + grade, {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });

            if (response.status < 300) {
                return {
                    data: response.data
                }
            }
            return {
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }

    static async get(jwt: string, id: string) : Promise<IResultObject<IUserHomework>>{
        try {
            const response = await UserHomeworkService.httpClient.get<IUserHomework>("/" + id, {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });

            if (response.status < 300) {
                return {
                    data: response.data
                }
            }
            return {
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }
}