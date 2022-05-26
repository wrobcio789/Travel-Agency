<template>
    <div id="orders-view">
        <table>
            <tr>
                <th>OrderID</th>
                <th>OfferId</th>
                <th>paymentId</th>
                <th>createdAt</th>
                <th>status</th>
            </tr>
            <tr v-for="order in orders" v-bind:key="order.id">
                <th>{{order.orderId}}</th>
                <th>{{order.offerId}}</th>
                <th>{{order.paymentId}}</th>
                <th>{{formatDate(order.createdAt)}}</th>
                <th>{{order.status}}</th>
            </tr>

        </table>
    </div>
</template>

<script>

import moment from "moment";

export default {
    data() {
        return {
            orders: []
        };
    },
    methods: {
        setOrders() {
            this.$http.get('/api/orders/list')
            .then(res => {
                this.orders = res.body;
            });
        },
        formatDate(date) {
            return moment(date).format('DD MM YYYY, h:mm:ss');
        }
    },
    created() {
        this.setOrders();
    },
}
</script>

<style scoped>
table, th, td {
  border: 1px solid;
}
</style>>