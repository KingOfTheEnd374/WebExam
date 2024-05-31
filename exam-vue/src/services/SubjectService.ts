import axios from "axios";
import type { IResultObject } from "./IResultObject";
import type { ISubject } from "@/domain/ISubject";
import type { IUser } from "@/domain/IUser";

export default class SubjectService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: "http://localhost:5275/api/v1/Subject"
    });

    static async getAll(jwt: string) : Promise<IResultObject<ISubject[]>>{
        try {
            const response = await SubjectService.httpClient.get<ISubject[]>("", {
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

    static async getAllInSemester(jwt: string, semesterId: string) : Promise<IResultObject<ISubject[]>>{
        try {
            const response = await SubjectService.httpClient.get<ISubject[]>("/semester:" + semesterId, {
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

    static async get(jwt: string, id: string) : Promise<IResultObject<ISubject>>{
        try {
            const response = await SubjectService.httpClient.get<ISubject>("/" + id, {
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

    static async getAverage(jwt: string, id: string) : Promise<IResultObject<number>>{
        try {
            const response = await SubjectService.httpClient.get<number>("/average/" + id, {
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

    static async getUsers(jwt: string, id: string) : Promise<IResultObject<IUser[]>>{
        try {
            const response = await SubjectService.httpClient.get<IUser[]>("/subject:" + id, {
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

    static async getGrade(jwt: string, subjectId: string, userId: string) : Promise<IResultObject<number>>{
        try {
            const response = await SubjectService.httpClient.get<number>("/subject:" + subjectId + "/user:" + userId, {
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