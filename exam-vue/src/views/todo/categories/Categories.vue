<script setup lang="ts">
import type { ICategory } from '@/domain/ICategory';
import router from '@/router';
import CategoryService from '@/services/CategoryService';
import { useAuthStore } from '@/stores/auth';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const authStore = useAuthStore();

let isLoading = ref(true);
let categories = ref<ICategory[]>([]);

const loadData = async () => {
    if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }
    const response = await CategoryService.getAll(authStore.jwt!);
    if (response.data) {
        categories.value = response.data;
    }

    isLoading.value = false;
};

watch(() => route.params.id, loadData, { immediate: true });

const deleteCategory = async (id: string) => {
    const response = await CategoryService.delete(authStore.jwt!, id);
    loadData();
}

</script>

<template>

    <ul v-if="isLoading">
        <h1>Categories - LOADING</h1>
    </ul>

    <ul v-else>
        <h1>Categories</h1>
        <p>
            <RouterLink :to="'/categories/create'">Create New</RouterLink>
        </p>
        <table className="table">
            <thead>
                <tr>
                    <th>
                        CategoryName
                    </th>
                    <th>
                        CategorySort
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in categories">
                    <td>
                        {{ item.categoryName }}
                    </td>
                    <td>
                        {{ item.categorySort }}
                    </td>
                    <td>
                        <RouterLink :to="{ name: 'EditCategory', params: { id: item.id }}">Edit</RouterLink> |
                        <button @click.prevent="deleteCategory(item.id)" className="btn btn-link">Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </ul>
</template>