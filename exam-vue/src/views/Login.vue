<script setup lang="ts">
import router from '@/router';
import AccountService from '@/services/AccountService';
import { useAuthStore } from '@/stores/auth';
import { ref } from 'vue';


const authStore = useAuthStore();

let loginEmail = ref('teach@eesti.ee');
let loginPassword = ref('Test123.');

let loginOngoing = ref(false);
let errors = ref<string[]>([]);

const doLogin = async () => {

    if (loginEmail.value.length < 5 || loginPassword.value.length < 6) {
        errors.value = ["Invalid input lengths"];
        return;
    }

    loginOngoing.value = true;

    const res = await AccountService.login(loginEmail.value, loginPassword.value);

    if (res.data) {
        authStore.jwt = res.data.jwt;
        authStore.refreshToken = res.data.refreshToken;
        errors.value = [];
        router.push("/");
    }
    else {
        errors.value = res.errors!;
    }
    loginOngoing.value = false;
}
</script>

<template>
    <h1>Log in</h1>
    <div class="row">
        <div class="col-md-6">
            <section>
                <hr />

                <div>{{ errors.join(",") }}</div>
                <div class="form-floating mb-3">
                    <input v-model="loginEmail" class="form-control" autocomplete="username" aria-required="true"
                        placeholder="name@example.com" type="email" id="Input_Email" name="Input.Email" value="" />
                    <label class="form-label" for="Input_Email">Email</label>
                </div>

                <div class="form-floating mb-3">
                    <input v-model="loginPassword" class="form-control" autocomplete="current-password"
                        aria-required="true" placeholder="password" type="password" id="Input_Password"
                        name="Input.Password" />
                    <label class="form-label" for="Input_Password">Password</label>
                </div>

                <div>
                    <button @click.prevent="doLogin" id="login-submit" class="w-100 btn btn-lg btn-primary">{{
                        loginOngoing ? "Wait.." : "Login" }}</button>
                </div>
            </section>
        </div>

    </div>

</template>

<style scoped></style>
