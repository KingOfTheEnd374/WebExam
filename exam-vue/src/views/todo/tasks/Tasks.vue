<script setup lang="ts">
import type { ICategory } from '@/domain/ICategory';
import type { IPriority } from '@/domain/IPriority';
import type { ITask } from '@/domain/ITask';
import router from '@/router';
import CategoryService from '@/services/CategoryService';
import PriorityService from '@/services/PriorityService';
import TaskService from '@/services/TaskService';
import { useAuthStore } from '@/stores/auth';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const authStore = useAuthStore();

let isLoading = ref(true);

let tasks = ref<ITask[]>([]);
let categories = ref<ICategory[]>([]);
let priorities = ref<IPriority[]>([]);

const loadData = async () => {
    if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }
    const response = await TaskService.getAll(authStore.jwt!);
    if (response.data) {
        tasks.value = response.data;
    }

    const response2 = await CategoryService.getAll(authStore.jwt!);
    if (response2.data) {
        categories.value = response2.data;
    }

    const response3 = await PriorityService.getAll(authStore.jwt!);
    if (response3.data) {
        priorities.value = response3.data;
    }

    isLoading.value = false;
};
watch(() => route.params.id, loadData, { immediate: true });

const deleteTask = async (id: string) => {
    const response = await TaskService.delete(authStore.jwt!, id);
    loadData();
}

</script>

<template>
    <ul v-if="isLoading">
        <h1>Tasks - LOADING</h1>
    </ul>

    <ul v-else>
        <h1>Tasks</h1>

        <p>
            <RouterLink :to="'/tasks/create'">Create New</RouterLink>
        </p>
        <table className="table">
            <thead>
                <tr>
                    <th>
                        TaskName
                    </th>
                    <th>
                        TaskSort
                    </th>
                    <th>
                        CreatedDt
                    </th>
                    <th>
                        DueDt
                    </th>
                    <th>
                        IsCompleted
                    </th>
                    <th>
                        IsArchived
                    </th>
                    <th>
                        TodoCategory
                    </th>
                    <th>
                        TodoPriority
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in tasks">
                    <td>
                        {{item.taskName}}
                    </td>
                    <td>
                        {{item.taskSort}}
                    </td>
                    <td>
                        {{item.createdDt.substring(0, 16).replace("T", " ")}}
                    </td>
                    <td>
                        {{item.dueDt.substring(0, 16).replace("T", " ")}}
                    </td>
                    <td>
                        <input v-model="item.isCompleted" className="form-check-input" type="checkbox" id="IsArchived"
                            name="IsArchived" disabled />
                    </td>
                    <td>
                        <input v-model="item.isArchived" className="form-check-input" type="checkbox" id="IsArchived"
                            name="IsArchived" disabled />
                    </td>
                    <td>
                        {{categories.filter(obj => { return obj.id === item.todoCategoryId })[0].categoryName}}
                    </td>
                    <td>
                        {{priorities.filter(obj => { return obj.id === item.todoPriorityId })[0].priorityName}}
                    </td>
                    <td>
                        <RouterLink :to="{ name: 'EditTask', params: { id: item.id }}">Edit</RouterLink> |
                        <button @click.prevent="deleteTask(item.id)" className="btn btn-link">Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </ul>
</template>