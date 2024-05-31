<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { jwtDecode } from 'jwt-decode';
import { ref, watch } from 'vue';

const authStore = useAuthStore();

const doLogout = () => {
    authStore.jwt = null;
    authStore.refreshToken = null;
}
const userName = ref("");

const setUserName = () => {
    if (authStore.isAuthenticated && authStore.jwt) {
        const decoded = jwtDecode(authStore.jwt) as { [key: string]: string };
        userName.value = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
    } else {
        userName.value = "";
    }
}

// Watch for changes in the authentication state
watch(() => authStore.isAuthenticated, setUserName);

// Initialize userName on component mount
setUserName();

</script>

<template>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <RouterLink class="navbar-brand" :to="'/'">WebApp</RouterLink>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse"
                    aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <RouterLink class="nav-link text-dark" to="/">Home</RouterLink>
                        </li>
                        <li class="nav-item">
                            <RouterLink class="nav-link text-dark" to="/semesters">Semesters</RouterLink>
                        </li>
                        <!-- <li class="nav-item">
                            <RouterLink class="nav-link text-dark" to="/priorities">Priorities</RouterLink>
                        </li>
                        <li class="nav-item">
                            <RouterLink class="nav-link text-dark" to="/tasks">Tasks</RouterLink>
                        </li> -->
                    </ul>

                    <ul v-if="!authStore.isAuthenticated" class="navbar-nav">
                        <li class="nav-item">
                            <RouterLink class="nav-link text-dark" :to="'/register'">Register</RouterLink>
                        </li>
                        <li class="nav-item">
                            <RouterLink class="nav-link text-dark" :to="'/login'">Login</RouterLink>
                        </li>
                    </ul>
                    <ul v-else class="navbar-nav">
                        <li class="nav-item">
                            <RouterLink class="nav-link text-dark" :to="'/'">Hello, {{ userName }}!</RouterLink>
                        </li>
                        <li class="nav-item">
                            <RouterLink class="nav-link text-dark" @click.prevent="doLogout" :to="'/'">Logout</RouterLink>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
</template>

<style scoped></style>