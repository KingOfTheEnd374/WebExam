<script setup lang="ts">
import router from '@/router';
import CategoryService from '@/services/CategoryService';
import { useAuthStore } from '@/stores/auth';
import { ref } from 'vue';

const authStore = useAuthStore();
let name = ref("");
let sort = ref(0);

let errors = ref<string[]>([]);

const validateAndCreate = async () => {
        if (name.value.length < 1 || sort.value < 0 || !Number.isInteger(sort.value)) {
            errors.value = ["Invalid inputs"];
            return;
        }
        const response = await CategoryService.create(authStore.jwt!, crypto.randomUUID(), name.value, sort.value, "");
        router.push("/categories");
    };

</script>

<template>
    <h1>Create</h1>

    <h4>TodoCategory</h4>
    <hr />
    <div className="text-danger" role="alert">{{errors.join(",")}}</div>
    <div className="row">
        <div className="col-md-4">
            <div className="form-group">
                <label className="control-label" htmlFor="CategoryName">Category Name</label>
                <input v-model="name" id="CategoryName" type="text" className="form-control"/>
            </div>
            <div className="form-group">
                <label className="control-label" htmlFor="CategorySort">Category Sort</label>
                <input v-model="sort" id="CategorySort" type="number" className="form-control"/>
            </div>
            <div className="form-group">
                <button @click.prevent="validateAndCreate" className="btn btn-primary">Create</button>
            </div>
        </div>
    </div>

    <div>
        <RouterLink :to="'/categories'">Back to List</RouterLink>
    </div>
</template>