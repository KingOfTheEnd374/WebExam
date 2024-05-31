<script setup lang="ts">
import type { ICategory } from '@/domain/ICategory';
import type { IPriority } from '@/domain/IPriority';
import router from '@/router';
import CategoryService from '@/services/CategoryService';
import PriorityService from '@/services/PriorityService';
import TaskService from '@/services/TaskService';
import { useAuthStore } from '@/stores/auth';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const authStore = useAuthStore();

let name = ref("");
let sort = ref(0);
let due = ref((new Date()).toISOString().substring(0, 16));
let completed = ref(false);
let archived = ref(false);
let categoryId = ref("");
let priorityId = ref("");
let categories = ref<ICategory[]>([]);
let priorities = ref<IPriority[]>([]);

let errors = ref<string[]>([]);

const loadData = async () => {
    if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }
    const responseCat = await CategoryService.getAll(authStore.jwt!);
    if (responseCat.data) {
        categories.value = responseCat.data;
    }

    const responsePri = await PriorityService.getAll(authStore.jwt!);
    if (responsePri.data) {
        priorities.value = responsePri.data;
    }
};
watch(() => route.params.id, loadData, { immediate: true });

const validateAndCreate = async () => {
    if (name.value.length < 1 || sort.value < 0 || !Number.isInteger(sort.value) || due.value.length < 16 || categoryId.value === "" || priorityId.value === "") {
        errors.value = ["Invalid inputs"];
        return;
    }
    const response = await TaskService.create(authStore.jwt!, crypto.randomUUID(), name.value, sort.value, (new Date()).toISOString(), due.value, completed.value, archived.value, categoryId.value, priorityId.value, (new Date()).toISOString());
    router.push("/tasks");
};

</script>

<template>
    <h1>Create</h1>

    <h4>TodoTask</h4>
    <hr />
    <div className="text-danger" role="alert">{{ errors.join(",") }}</div>
    <div className="row">
        <div className="col-md-4">
            <div className="form-group">
                <label className="control-label" htmlFor="TaskName">Task Name</label>
                <input v-model="name" id="TaskName" type="text" className="form-control" />
            </div>
            <div className="form-group">
                <label className="control-label" htmlFor="TaskSort">Task Sort</label>
                <input v-model="sort" id="TaskSort" type="number" className="form-control" />
            </div>
            <div className="form-group">
                <label className="control-label" htmlFor="DueDt">Due Date</label>
                <input v-model="due" id="DueDt" type="datetime-local" className="form-control" />
            </div>
            <div className="form-group form-check">
                <label className="form-check-label">
                    <input v-model="completed" className="form-check-input" type="checkbox" id="IsCompleted"
                        name="IsCompleted" /> IsCompleted
                </label>
            </div>
            <div className="form-group form-check">
                <label className="form-check-label">
                    <input v-model="archived" className="form-check-input" type="checkbox" id="IsArchived"
                        name="IsArchived" /> IsArchived
                </label>
            </div>
            <div className="form-group">
                <label className="control-label" htmlFor="TodoCategoryId">TodoCategoryId</label>
                <select v-model="categoryId" className="form-control" id="TodoCategoryId" name="TodoCategoryId">
                    <option value=""></option>
                    <option v-for="item in categories" :value="item.id">{{item.categoryName}}</option>
                </select>
            </div>
            <div className="form-group">
                <label className="control-label" htmlFor="TodoPriorityId">TodoPriorityId</label>
                <select v-model="priorityId" className="form-control" id="TodoPriorityId" name="TodoPriorityId">
                    <option value=""></option>
                    <option v-for="item in priorities" :value="item.id">{{item.priorityName}}</option>
                </select>
            </div>
            <div className="form-group">
                <button @click.prevent="validateAndCreate" className="btn btn-primary">Create</button>
            </div>
        </div>
    </div>

    <div>
        <RouterLink :to="'/tasks'">Back to List</RouterLink>
    </div>
</template>