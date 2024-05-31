import axios from "axios";
import type { IResultObject } from "./IResultObject";
import type { ISemester } from "@/domain/ISemester";

export default class SemesterService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: "http://localhost:5275/api/v1/Semester"
    });

    static async getAll(jwt: string) : Promise<IResultObject<ISemester[]>>{
        try {
            const response = await SemesterService.httpClient.get<ISemester[]>("", {
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