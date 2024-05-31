<script setup lang="ts">
import type { IPriority } from '@/domain/IPriority';
import router from '@/router';
import PriorityService from '@/services/PriorityService';
import { useAuthStore } from '@/stores/auth';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const authStore = useAuthStore();

let isLoading = ref(true);
let priorities = ref<IPriority[]>([]);

const loadData = async () => {
    if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }
    const response = await PriorityService.getAll(authStore.jwt!);
    if (response.data) {
        priorities.value = response.data;
    }

    isLoading.value = false;
};

watch(() => route.params.id, loadData, { immediate: true });

const deletePriority = async (id: string) => {
    const response = await PriorityService.delete(authStore.jwt!, id);
    loadData();
}

</script>

<template>

    <ul v-if="isLoading">
        <h1>Priorities - LOADING</h1>
    </ul>

    <ul v-else>
        <h1>Priorities</h1>
        <p>
            <RouterLink :to="'/priorities/create'">Create New</RouterLink>
        </p>
        <table className="table">
            <thead>
                <tr>
                    <th>
                        PriorityName
                    </th>
                    <th>
                        PrioritySort
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in priorities">
                    <td>
                        {{ item.priorityName }}
                    </td>
                    <td>
                        {{ item.prioritySort }}
                    </td>
                    <td>
                        <RouterLink :to="{ name: 'EditPriority', params: { id: item.id }}">Edit</RouterLink> |
                        <button @click.prevent="deletePriority(item.id)" className="btn btn-link">Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </ul>
</template>