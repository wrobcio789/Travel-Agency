<template>
    <div class="app-footer">
        <h2>Statistics</h2>
        <div class="row">
            <div class="statistics column">
                <b>Hotels</b>
                <span v-for="hotelStatistics in statistics.hotels" v-bind:key="hotelStatistics.hotelName">
                    <span>{{hotelStatistics.hotelName}}</span>
                    <span>:</span>
                    <span>{{hotelStatistics.visits}}</span>
                </span>
            </div>
            <div class="statistics column">
                <b>Transports</b>
                <span v-for="transportStatistics in statistics.transports" v-bind:key="transportStatistics.code">
                    <span>{{transportNameByCode[transportStatistics.code]}}</span>
                    <span>:</span>
                    <span>{{transportStatistics.visits}}</span>
                </span>
            </div>
            <div class="statistics column">
                <b>Tours</b>
                <span v-for="tourStatistics in statistics.tours" v-bind:key="tourStatistics.departure">
                    <span>{{tourStatistics.departure}}</span>
                    <span>:</span>
                    <span>{{tourStatistics.visits}}</span>
                </span>
            </div>
        </div>
    </div>
</template>

<script>
import { getTransportationTypes } from './places';

export default {
    data() {
        return {
            statistics: {}
        };
    },
    computed: {
        transportNameByCode() {
            let result = {};
            getTransportationTypes().forEach(
                transportationType => result[transportationType.code] = transportationType.label
            );

            return result;
        },
    },
    methods: {
        setStatistics() {
            this.$http.get('/api/offers/statistics', {params: {head: 5}})
            .then(res => {
                this.statistics = res.body;
            });
        }
    },
    created() {
        this.setStatistics();
        this.$eventsManager.getOfferBoughtDispatcher().add(this.setStatistics);
    },
    beforeDestroy() {
        this.$eventsManager.getOfferBoughtDispatcher().remove(this.setStatistics);
    }
}
</script>

<style scoped>
.app-footer{
    position: fixed;
    bottom: 0;
    left: 0;
    padding: 20px 20px;
    width: 100%;
    background-color: cornsilk;
    border-top: 2px solid crimson;
}

.row {
    display: flex;
    align-items: left;
}

.column {
    display: flex;
    flex-direction: column;
}

.statistics {
    min-width: 200px;
}

</style>>