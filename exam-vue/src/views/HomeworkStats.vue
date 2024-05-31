<script setup lang="ts">
import type { IHomework } from '@/domain/IHomework';
import type { IUser } from '@/domain/IUser';
import router from '@/router';
import HomeworkService from '@/services/HomeworkService';
import SubjectService from '@/services/SubjectService';
import { useAuthStore } from '@/stores/auth';
import { jwtDecode } from 'jwt-decode';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

let id = route.params.id

const authStore = useAuthStore();

let isLoading = ref(true);
let homework = ref<IHomework | null>(null);
let average = ref<number>(0);

let users = ref<IUser[]>([]);
let grades = ref<number[]>([]);

let isTeacher = ref(false);

const loadData = async () => {
    if (typeof id != "string")
        return;
    if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }
    const response = await HomeworkService.get(authStore.jwt!, id);
    if (response.data) {
        homework.value = response.data;

        const res2 = await HomeworkService.getAverage(authStore.jwt!, id);
        if (res2.data) {
            average.value = res2.data;
        }
    }

    if (authStore.isAuthenticated) {
        const decoded = jwtDecode(authStore.jwt!) as { [key: string]: string };
        isTeacher.value = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "Teacher";


        const res = await SubjectService.getUsers(authStore.jwt!, response.data!.subjectId);
        if (res.data) {
            users.value = res.data;
            
            for (let i = 0; i < res.data.length; i++) {
                const resGrade = await HomeworkService.getGrade(authStore.jwt!, id, res.data[i].id);
                
                grades.value.push(resGrade.data!);
            }
        }
    }

    isLoading.value = false;
};

watch(() => route.params.id, loadData, { immediate: true });

// const deleteHomework = async (id: string) => {
//     const response = await HomeworkService.delete(authStore.jwt!, id);
//     loadData();
// }

</script>

<template>

    <ul v-if="isLoading">
        <h1>Homeworks - LOADING</h1>
    </ul>

    <ul v-else>
        <h1>Homeworks</h1>
        <p>
            <!-- <RouterLink :to="'/homeworks/create'">Create New</RouterLink> -->
        </p>
        <table className="table">
            <thead>
                <tr>
                    <th>
                        Homework Name
                    </th>
                    <th>
                        Homework Average Grade
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        {{ homework!.name }}
                    </td>
                    <td>
                        {{ average }}
                    </td>
                    <td>
                        <!-- <RouterLink :to="{ name: 'EditHomework', params: { id: item.id }}">Edit</RouterLink> |
                        <button @click.prevent="deleteHomework(item.id)" className="btn btn-link">Delete</button> -->
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