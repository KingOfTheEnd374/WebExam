<script setup lang="ts">
import type { ISemester } from '@/domain/ISemester';
import router from '@/router';
import SemesterService from '@/services/SemesterService';
import { useAuthStore } from '@/stores/auth';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const authStore = useAuthStore();

let isLoading = ref(true);
let semesters = ref<ISemester[]>([]);

const loadData = async () => {
    /*if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }*/
    const response = await SemesterService.getAll(authStore.jwt!);
    if (response.data) {
        semesters.value = response.data;
    }

    isLoading.value = false;
};

watch(() => route.params.id, loadData, { immediate: true });

</script>

<template>

    <ul v-if="isLoading">
        <h1>Semesters - LOADING</h1>
    </ul>

    <ul v-else>
        <h1>Semesters</h1>
        <table className="table">
            <thead>
                <tr>
                    <th>
                        Semester Name
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in semesters">
                    <td>
                        {{ item.name }}
                    </td>
                    <td>
                        <RouterLink :to="{ name: 'Subjects', params: { id: item.id }}">Subjects</RouterLink>
                    </td>
                </tr>
            </tbody>
        </table>
    </ul>
</template>