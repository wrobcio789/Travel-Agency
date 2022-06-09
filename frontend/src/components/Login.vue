<template>
    <div id="login">
        <h2>Login</h2>
        <form>
            <input type="text" name="login" v-model="userData.username" placeholder="Login"/>
            <input type="password" name="password" v-model="userData.password" placeholder="Password"/>
            <button type="button" v-on:click="login()">Login</button>
        </form>
        <span v-if="isError" class="error-text">Bad credentials</span>
    </div>
</template>

<script>

export default {
    data() {
        return {
            userData: {
                username: "",
                password: "",
            },
            isError: false,
        };
    },
    methods: {
        login() {
            this.$http.post('/api/customers/login', this.userData)
            .then(res => {
                this.$store.commit('setCredentials', {username: this.userData.username, token: res.bodyText});
                this.isError = false;

                this.$router.push({path: "/offers"});
            })
            .catch(error => {
                this.userData.password = "";
                this.isError = true;
            })
        }
    }
}
</script>

<style scoped>
#login{
    margin: 10% 0;
    display: block;
    text-align: center;
}

form{
    width: 200px;
    margin-left: auto;
    margin-right: auto;
    display: flex;
    flex-direction: column;
}

form input{
    margin-top: 10px
}

form button{
    margin-top: 10px
}

.error-text{
    color: red;
    display: block;
    padding-top: 10px
}
</style>>