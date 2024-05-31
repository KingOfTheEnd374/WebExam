<script setup lang="ts">
import router from '@/router';
import PriorityService from '@/services/PriorityService';
import { useAuthStore } from '@/stores/auth';
import { jwtDecode } from 'jwt-decode';
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
    const decoded = jwtDecode(authStore.jwt!) as { [key: string]: string };
    const userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

    const response = await PriorityService.create(authStore.jwt!, crypto.randomUUID(), userId, name.value, sort.value, (new Date()).toISOString(), "");
    router.push("/priorities");
};

</script>

<template>
    <h1>Create</h1>

    <h4>TodoPriority</h4>
    <hr />
    <div className="text-danger" role="alert">{{ errors.join(",") }}</div>
    <div className="row">
        <div className="col-md-4">
            <div className="form-group">
                <label className="control-label" htmlFor="PriorityName">Priority Name</label>
                <input v-model="name" id="PriorityName" type="text" className="form-control" />
            </div>
            <div className="form-group">
                <label className="control-label" htmlFor="PrioritySort">Priority Sort</label>
                <input v-model="sort" id="PrioritySort" type="number" className="form-control" />
            </div>
            <div className="form-group">
                <button @click.prevent="validateAndCreate" className="btn btn-primary">Create</button>
            </div>
        </div>
    </div>

    <div>
        <RouterLink :to="'/priorities'">Back to List</RouterLink>
    </div>
</template>