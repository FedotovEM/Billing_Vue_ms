<template>
    <div class="container">
        <div class="card card-container">
            <Form @submit="handleRegister" :validation-schema="schema">
                <div v-if="!successful">
                    <div class="form-group">
                        <label for="username">Введите имя пользователя</label>
                        <Field name="username" type="text" class="form-control" />
                        <ErrorMessage name="username" class="error-feedback" />
                    </div>
                    <div class="form-group">
                        <label for="email">Введите Email</label>
                        <Field name="email" type="email" class="form-control" />
                        <ErrorMessage name="email" class="error-feedback" />
                    </div>
                    <div class="form-group">
                        <label for="password">Введите пароль</label>
                        <Field name="password" type="password" class="form-control" />
                        <ErrorMessage name="password" class="error-feedback" />
                    </div>

                    <div class="form-group">
                        <button class="btn btn-primary btn-block" :disabled="loading">
                            <span v-show="loading"
                                  class="spinner-border spinner-border-sm"></span>
                            Зарегистрироваться
                        </button>
                    </div>
                </div>
            </Form>

            <div v-if="message"
                 class="alert"
                 :class="successful ? 'alert-success' : 'alert-danger'">
                {{ message }}
            </div>
        </div>
    </div>
</template>

<script>
    import { Form, Field, ErrorMessage } from "vee-validate";
    import * as yup from "yup";

    export default {
        name: "Register",
        components: {
            Form,
            Field,
            ErrorMessage,
        },
        data() {
            const schema = yup.object().shape({
                username: yup
                    .string()
                    .required("Требуется ввести имя пользователя!")
                    .min(2, "Должно быть не менее 2 символов!")
                    .max(30, "Должно быть не более 30 символов!"),
                email: yup
                    .string()
                    .required("Требуется ввести Email!")
                    .email("Email недействителен!")
                    .max(80, "Должно быть не более 80 символов!"),
                password: yup
                    .string()
                    .required("Требуется ввести пароль")
                    .min(6, "Должно быть не менее 6 символов!")
                    .max(40, "Должно быть не более 40 символов!"),
            });

            return {
                successful: false,
                loading: false,
                message: "",
                schema,
            };
        },
        computed: {
            loggedIn() {
                return this.$store.state.auth.status.loggedIn;
            },
        },
        mounted() {
            if (this.loggedIn) {
                this.$router.push("/profile");
            }
        },
        methods: {
            handleRegister(user) {
                this.message = "";
                this.successful = false;
                this.loading = true;

                this.$store.dispatch("auth/register", user).then(
                    (data) => {
                        this.message = data.message;
                        this.successful = true;
                        this.loading = false;
                    },
                    (error) => {
                        this.message =
                            (error.response &&
                                error.response.data &&
                                error.response.data.message) ||
                            error.message ||
                            error.toString();
                        this.successful = false;
                        this.loading = false;
                    }
                );
            },
        },
    };
</script>

<style>
    .container {
        position: relative;
        top: 20px;
    }
</style>