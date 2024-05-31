import axios from "axios";
import type { IResultObject } from "./IResultObject";
import type { IHomework } from "@/domain/IHomework";
import type { IUser } from "@/domain/IUser";

export default class HomeworkService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: "http://localhost:5275/api/v1/Homework"
    });

    static async getAll(jwt: string) : Promise<IResultObject<IHomework[]>>{
        try {
            const response = await HomeworkService.httpClient.get<IHomework[]>("", {
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

    static async getAllInSubject(jwt: string, subjectId: string) : Promise<IResultObject<IHomework[]>>{
        try {
            const response = await HomeworkService.httpClient.get<IHomework[]>("/subject:" + subjectId, {
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

    static async create(jwt: string, id: string, name: string, sort: number, tag: string): Promise<IResultObject<IHomework>> {
        const newData = {
            id: id,
            categoryName: name,
            categorySort: sort,
            tag: tag
        }
        try {
            const response = await HomeworkService.httpClient.post<IHomework>("", newData, {
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

    static async get(jwt: string, id: string) : Promise<IResultObject<IHomework>>{
        try {
            const response = await HomeworkService.httpClient.get<IHomework>("/" + id, {
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
            const response = await HomeworkService.httpClient.get<number>("/average/" + id, {
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

    static async update(jwt: string, id: string, name: string, sort: number, date: string, tag: string) : Promise<IResultObject<IHomework>>{
        const updateData = {
            id: id,
            categoryName: name,
            categorySort: sort,
            syncDt: date,
            tag: tag
        }
        try {
            const response = await HomeworkService.httpClient.put<IHomework>("/" + id, updateData, {
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

    static async delete(jwt: string, id: string): Promise<IResultObject<null>> {
        try {
            const response = await HomeworkService.httpClient.delete("/" + id, {
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
            const response = await HomeworkService.httpClient.get<number>("/hw:" + subjectId + "/user:" + userId, {
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