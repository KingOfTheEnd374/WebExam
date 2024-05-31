<script setup lang="ts">
import type { ISubject } from '@/domain/ISubject';
import type { IUser } from '@/domain/IUser';
import router from '@/router';
import SubjectService from '@/services/SubjectService';
import { useAuthStore } from '@/stores/auth';
import { jwtDecode } from 'jwt-decode';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

let id = route.params.id

const authStore = useAuthStore();

let isLoading = ref(true);
let subject = ref<ISubject | null>(null);
let average = ref<number>(0);

let users = ref<IUser[]>([]);
let grades = ref<number[]>([]);

let isTeacher = ref(false);

const loadData = async () => {
    if (typeof id != "string")
        return;
    /*if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }*/
    const response = await SubjectService.get(authStore.jwt!, id);
    if (response.data) {
        subject.value = response.data;

        const res2 = await SubjectService.getAverage(authStore.jwt!, id);
        if (res2.data) {
            average.value = res2.data;
        }
    }

    if (authStore.isAuthenticated) {
        const decoded = jwtDecode(authStore.jwt!) as { [key: string]: string };
        isTeacher.value = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "Teacher";


        const res = await SubjectService.getUsers(authStore.jwt!, id);
        if (res.data) {
            users.value = res.data;
            
            for (let i = 0; i < res.data.length; i++) {
                const resGrade = await SubjectService.getGrade(authStore.jwt!, id, res.data[i].id);
                
                grades.value.push(resGrade.data!);
            }
        }
    }

    isLoading.value = false;
};

watch(() => route.params.id, loadData, { immediate: true });

// const deleteSubject = async (id: string) => {
//     const response = await SubjectService.delete(authStore.jwt!, id);
//     loadData();
// }

</script>

<template>

    <ul v-if="isLoading">
        <h1>Subjects - LOADING</h1>
    </ul>

    <ul v-else>
        <h1>Subjects</h1>
        <p>
            <!-- <RouterLink :to="'/subjects/create'">Create New</RouterLink> -->
        </p>
        <table className="table">
            <thead>
                <tr>
                    <th>
                        Subject Name
                    </th>
                    <th>
                        Subject Average Grade
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        {{ subject!.name }}
                    </td>
                    <td>
                        {{ average }}
                    </td>
                    <td>
                        <!-- <RouterLink :to="{ name: 'EditSubject', params: { id: item.id }}">Edit</RouterLink> |
                        <button @click.prevent="deleteSubject(item.id)" className="btn btn-link">Delete</button> -->
                    </td>
                </tr>
            </tbody>
        </table>
        <table v-if="isTeacher" className="table">
            <thead>
                <tr>
                    <th>
                        User
                    </th>
                    <th>
                        Grade
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in users">
                    <td>
                        {{ item.email }}
                    </td>
                    <td>
                        {{ grades[index] }}
                    </td>
                    <td>
                        <!-- <RouterLink :to="{ name: 'EditSubject', params: { id: item.id }}">Edit</RouterLink> |
                        <button @click.prevent="deleteSubject(item.id)" className="btn btn-link">Delete</button> -->
                    </td>
                </tr>
            </tbody>
        </table>
    </ul>
</template>