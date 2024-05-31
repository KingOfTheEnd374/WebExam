<script setup lang="ts">
import type { IHomework } from '@/domain/IHomework';
import router from '@/router';
import HomeworkService from '@/services/HomeworkService';
import { useAuthStore } from '@/stores/auth';
import { jwtDecode } from 'jwt-decode';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const subjectId = route.params.id;

const authStore = useAuthStore();

let isTeacher = ref(false);

let isLoading = ref(true);
let homeworks = ref<IHomework[]>([]);
let grades = ref<number[]>([]);

const loadData = async () => {
    if (typeof subjectId != "string")
        return;
    if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }
    const response = await HomeworkService.getAllInSubject(authStore.jwt!, subjectId);
    if (response.data) {
        homeworks.value = response.data;

        const decoded = jwtDecode(authStore.jwt!) as { [key: string]: string };
        const userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

        for (let i = 0; i < response.data.length; i++) {
            const res = await HomeworkService.getGrade(authStore.jwt!, response.data[i].id, userId);
            grades.value.push(res.data!);
        }
    }

    const decoded = jwtDecode(authStore.jwt!) as { [key: string]: string };
    isTeacher.value = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "Teacher";


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
                        My Grade
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in homeworks">
                    <td>
                        {{ item.name }}
                    </td>
                    <td>
                        {{ grades[index] }}
                    </td>
                    <td>
                        <RouterLink :to="{ name: 'HomeworkStats', params: { id: item.id }}">Homework Stats</RouterLink> | 
                    </td>
                    <td v-if="isTeacher">
                        <RouterLink :to="{ name: 'HomeworkGrade', params: { id: item.id }}">Grade Users</RouterLink>
                    </td>
                </tr>
            </tbody>
        </table>
    </ul>
</template>