<script setup lang="ts">
import router from '@/router';
import AccountService from '@/services/AccountService';
import { useAuthStore } from '@/stores/auth';
import { ref } from 'vue';


const authStore = useAuthStore();

let email = ref("");
let pwd = ref("");
let userName = ref("");

let errors = ref<string[]>([]);

const validateAndRegister = async () => {

    if (email.value.length < 5 || pwd.value.length < 6 || userName.value.length < 1) {
        errors.value = ["Invalid input lengths"];
        return;
    }

    const res = await AccountService.register(email.value, pwd.value, userName.value);

    if (res.data) {
        authStore.jwt = res.data.jwt;
        authStore.refreshToken = res.data.refreshToken;
        errors.value = [];
        router.push("/");
    }
    else {
        errors.value = res.errors!;
    }
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
                    <input v-model="email" class="form-control" autocomplete="username" aria-required="true"
                        placeholder="name@example.com" type="email" id="Input_Email" name="Input.Email" value="" />
                    <label class="form-label" for="Input_Email">Email</label>
                </div>

                <div class="form-floating mb-3">
                    <input v-model="pwd" class="form-control" autocomplete="current-password" aria-required="true"
                        placeholder="password" type="password" id="Input_Password" name="Input.Password" />
                    <label class="form-label" for="Input_Password">Password</label>
                </div>

                <div class="form-floating mb-3">
                    <input v-model="userName" class="form-control" aria-required="true" placeholder="userName"
                        type="text" id="userName" name="userName" />
                    <label class="form-label" for="userName">First Name</label>
                </div>

                <div>
                    <button @click.prevent="validateAndRegister" id="register-submit"
                        class="w-100 btn btn-lg btn-primary">Register</button>
                </div>
            </section>
        </div>

    </div>

</template>

<style scoped></style>
