import axios from "axios";
import type { IResultObject } from "./IResultObject";
import type { IUserSubject } from "@/domain/IUserSubject";
import type { IUser } from "@/domain/IUser";

export default class UserSubjectService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: "http://localhost:5275/api/v1/UserSubject"
    });

    static async getAll(jwt: string) : Promise<IResultObject<IUserSubject[]>>{
        try {
            const response = await UserSubjectService.httpClient.get<IUserSubject[]>("", {
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

    static async setGrade(jwt: string, userId: string, subjectId: string, grade: number): Promise<IResultObject<null>> {
        try {
            const response = await UserSubjectService.httpClient.post("user:" + userId + "/subject:" + subjectId + "/grade:" + grade, {
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

    static async get(jwt: string, id: string) : Promise<IResultObject<IUserSubject>>{
        try {
            const response = await UserSubjectService.httpClient.get<IUserSubject>("/" + id, {
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