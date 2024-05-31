import axios from "axios";
import type { IResultObject } from "./IResultObject";
import type { ICategory } from "@/domain/ICategory";

export default class CategoryService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: 'https://taltech.akaver.com/api/v1/TodoCategories',
    });

    static async getAll(jwt: string) : Promise<IResultObject<ICategory[]>>{
        try {
            const response = await CategoryService.httpClient.get<ICategory[]>("", {
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

    static async create(jwt: string, id: string, name: string, sort: number, tag: string): Promise<IResultObject<ICategory>> {
        const newData = {
            id: id,
            categoryName: name,
            categorySort: sort,
            tag: tag
        }
        try {
            const response = await CategoryService.httpClient.post<ICategory>("", newData, {
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

    static async get(jwt: string, id: string) : Promise<IResultObject<ICategory>>{
        try {
            const response = await CategoryService.httpClient.get<ICategory>("/" + id, {
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

    static async update(jwt: string, id: string, name: string, sort: number, date: string, tag: string) : Promise<IResultObject<ICategory>>{
        const updateData = {
            id: id,
            categoryName: name,
            categorySort: sort,
            syncDt: date,
            tag: tag
        }
        try {
            const response = await CategoryService.httpClient.put<ICategory>("/" + id, updateData, {
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
            const response = await CategoryService.httpClient.delete("/" + id, {
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