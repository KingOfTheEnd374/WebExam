<script setup lang="ts">
import type { ISubject } from '@/domain/ISubject';
import router from '@/router';
import SubjectService from '@/services/SubjectService';
import { useAuthStore } from '@/stores/auth';
import { jwtDecode } from 'jwt-decode';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const semesterId = route.params.id;

const authStore = useAuthStore();

let isLoading = ref(true);
let subjects = ref<ISubject[]>([]);

let grades = ref<number[]>([]);
let isTeacher = ref(false);

const loadData = async () => {
    if (typeof semesterId != "string")
        return;
    /*if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }*/
    const response = await SubjectService.getAllInSemester(authStore.jwt!, semesterId);
    if (response.data) {
        subjects.value = response.data;

        if (authStore.isAuthenticated) {
            const decoded = jwtDecode(authStore.jwt!) as { [key: string]: string };
            const userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

            for (let i = 0; i < response.data.length; i++) {
                const res = await SubjectService.getGrade(authStore.jwt!, response.data[i].id, userId);
                grades.value.push(res.data!);
            }
        }
    }

    if (authStore.isAuthenticated) {
        const decoded = jwtDecode(authStore.jwt!) as { [key: string]: string };
        isTeacher.value = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "Teacher";
    }

    isLoading.value = false;
};

watch(() => route.params.id, loadData, { immediate: true });

</script>

<template>

    <ul v-if="isLoading">
        <h1>Subjects - LOADING</h1>
    </ul>

    <ul v-else>
        <h1>Subjects</h1>
        <table className="table">
            <thead>
                <tr>
                    <th>
                        Subject Name
                    </th>
                    <th>
                        My Grade
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in subjects">
                    <td>
                        {{ item.name }}
                    </td>
                    <td>
                        {{ grades[index] }}
                    </td>
                    <td>
                        <RouterLink :to="{ name: 'Homeworks', params: { id: item.id }}">Homeworks</RouterLink> | 
                        <RouterLink :to="{ name: 'SubjectStats', params: { id: item.id }}">Subject Stats</RouterLink>
                        
                    </td>
                    <td v-if="isTeacher">
                        <RouterLink :to="{ name: 'SubjectGrade', params: { id: item.id }}">Grade Users</RouterLink>
                    </td>
                </tr>
            </tbody>
        </table>
    </ul>
</template>