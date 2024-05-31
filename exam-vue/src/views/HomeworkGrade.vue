<script setup lang="ts">
import type { IHomework } from '@/domain/IHomework';
import type { IUser } from '@/domain/IUser';
import router from '@/router';
import HomeworkService from '@/services/HomeworkService';
import SubjectService from '@/services/SubjectService';
import UserHomeworkService from '@/services/UserHomeworkService';
import { useAuthStore } from '@/stores/auth';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

let id = route.params.id

const authStore = useAuthStore();

let users = ref<IUser[]>([]);

let errors = ref<string[]>([]);

let isLoading = ref(true);
let homework = ref<IHomework | null>(null);

let grade = ref(0);
let userId = ref("");

const loadData = async () => {
    if (typeof id != "string")
        return;
    if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }
    const response = await HomeworkService.get(authStore.jwt!, id);
    if (response.data) {
        homework.value = response.data;

        const res2 = await SubjectService.getUsers(authStore.jwt!, response.data.subjectId);
        if (res2.data) {
            users.value = res2.data;
        }
    }

    isLoading.value = false;
};

watch(() => route.params.id, loadData, { immediate: true });

const validateAndCreate = async () => {
    if (userId.value === "" || grade.value < 0) {
        errors.value = ["Invalid inputs"];
        return;
    }
    const response = await UserHomeworkService.setGrade(authStore.jwt!, userId.value, homework.value!.id, grade.value);
    router.push("/homeworks");
};

</script>

<template>

    <ul v-if="isLoading">
        <h1>Homeworks - LOADING</h1>
    </ul>

    <ul v-else>
    <div className="text-danger" role="alert">{{ errors.join(",") }}</div>
    <h1>{{ homework!.name }}</h1>
    <div className="row">
        <div className="col-md-4">
            <div className="form-group">
                <label className="control-label" htmlFor="user">User</label>
                <select v-model="userId" id="user" className="form-control">
                    <option value=""></option>
                    <option v-for="item in users" :value="item.id">{{item.email}}</option>
                </select>
            </div>

            <div className="form-group">
                <label className="control-label" htmlFor="grade">Grade</label>
                <input v-model="grade" id="grade" type="number" className="form-control" />
            </div>
            <div className="form-group">
                <button @click.prevent="validateAndCreate" className="btn btn-primary">Update</button>
            </div>
        </div>
    </div>
    </ul>
</template>