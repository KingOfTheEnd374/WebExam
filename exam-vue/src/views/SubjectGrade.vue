<script setup lang="ts">
import type { ISubject } from '@/domain/ISubject';
import type { IUser } from '@/domain/IUser';
import router from '@/router';
import SubjectService from '@/services/SubjectService';
import UserSubjectService from '@/services/UserSubjectService';
import { useAuthStore } from '@/stores/auth';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

let id = route.params.id

const authStore = useAuthStore();

let users = ref<IUser[]>([]);

let errors = ref<string[]>([]);

let isLoading = ref(true);
let subject = ref<ISubject | null>(null);

let grade = ref(0);
let userId = ref("");

const loadData = async () => {
    if (typeof id != "string")
        return;
    if (!authStore.isAuthenticated) {
        router.push("/login");
        return;
    }
    const response = await SubjectService.get(authStore.jwt!, id);
    if (response.data) {
        subject.value = response.data;

        const res2 = await SubjectService.getUsers(authStore.jwt!, id);
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
    const response = await UserSubjectService.setGrade(authStore.jwt!, userId.value, subject.value!.id, grade.value);
    router.push("/subjects");
};

</script>

<template>

    <ul v-if="isLoading">
        <h1>Subjects - LOADING</h1>
    </ul>

    <ul v-else>
    <div className="text-danger" role="alert">{{ errors.join(",") }}</div>
    <h1>{{ subject!.name }}</h1>
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