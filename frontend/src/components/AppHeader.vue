<template>
    <div class="app-header">
        <div v-if="username" class="logout-button">
            <span>{{"Logged as " + username}}</span>
            <button type="button" v-on:click="logout()">Logout</button>
        </div>
        <div v-if="username" class="orders-button">
            <button type="button" v-on:click="seeOrders()">SeeOrders</button>
        </div>
        <h2 v-on:click="seeOffers()">{{title}}</h2>
    </div>
</template>

<script>

export default {
    data() {
        return {
            title: "PG Travel"
        };
    },
    computed: {
        username() {
            return this.$store.getters.getUser;
        },
        nearestArrival(){
            return this.$store.getters.getFastestArrival;
        }
    },
    methods: {
        logout() {
            this.$store.commit('clearCredentials');
            this.$router.push({path: "/"});
        },
        seeOrders() {
            this.$router.push({path: "orders"});
        },
        seeOffers() {
            if (this.username) {
                this.$router.push({path: "offers"});
            }
        }
    }
}
</script>

<style scoped>
.app-header{
    width: 100%;
    margin: 0;
    padding: 10px 0;
    background-color: cornsilk;
    border-bottom: 2px solid crimson;
}

.app-header h2{
    padding: 20px 0;
    font-size: 30px;
    text-transform: uppercase;
    text-align: center;
}

.logout-button{
    margin-top: 0;
    float: right;
    padding-right: 15px;
}

.orders-button{
    margin-top: 0;
    float: left;
    padding-left: 15px;
}
</style>>