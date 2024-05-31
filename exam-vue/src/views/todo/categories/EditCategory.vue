<script setup lang="ts">
import router from '@/router';
import CategoryService from '@/services/CategoryService';
import { useAuthStore } from '@/stores/auth';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const authStore = useAuthStore();

let name = ref("");
let sort = ref(0);

let id = route.params.id

let errors = ref<string[]>([]);

const loadData = async () => {
    if (typeof id != "string")
        return;
    if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }
    const response = await CategoryService.get(authStore.jwt!, id);
    if (response.data) {
        name.value = response.data.categoryName;
        sort.value = response.data.categorySort;
    }
};

watch(() => route.params.id, loadData, { immediate: true });

const validateAndUpdate = async () => {
    if (typeof id != "string")
        return;
    if (name.value.length < 1 || sort.value < 0 || !Number.isInteger(sort.value)) {
        errors.value = ["Invalid inputs"];
        return;
    }
    const response = await CategoryService.update(authStore.jwt!, id, name.value, sort.value, (new Date()).toISOString(), "");
    router.push("/categories");
};

</script>

<template>
    <h1>Edit</h1>

    <h4>TodoCategory</h4>
    <hr />
    <div className="text-danger" role="alert">{{ errors.join(",") }}</div>
    <div className="row">
        <div className="col-md-4">
            <div className="form-group">
                <label className="control-label" htmlFor="CategoryName">Category Name</label>
                <input v-model="name" id="CategoryName" type="text" className="form-control" />
            </div>
            <div className="form-group">
                <label className="control-label" htmlFor="CategorySort">Category Sort</label>
                <input v-model="sort" id="CategorySort" type="number" className="form-control" />
            </div>
            <div className="form-group">
                <button @click.prevent="validateAndUpdate" className="btn btn-primary">Update</button>
            </div>
        </div>
    </div>

    <div>
        <RouterLink :to="'/categories'">Back to List</RouterLink>
    </div>
</template>