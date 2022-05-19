<template>
    <div class="offer-search-view">
        <div class="flex-row">
            <div class="searcher flex-1 flex-column">
                <span>Destination</span> 
                <v-select :options="destinations" v-model="filter.destination" @input="onFilterChange"></v-select>
                <span>Place of departure</span> 
                <v-select :options="placesOfDepartures" v-model="filter.placesOfDeparture" @input="onFilterChange"></v-select>
                <span>Departure date</span> 
                <datepicker v-model="filter.departureDate" @input="onFilterChange"></datepicker>
                <span>Number of adults</span> 
                <vue-numeric-input  v-model="filter.people.adults" :min="1" :max="10" :step="1" @input="onFilterChange"></vue-numeric-input>
                <span>Number of teenagers</span> 
                <vue-numeric-input  v-model="filter.people.teenagers" :min="0" :max="10" :step="1" @input="onFilterChange"></vue-numeric-input>
                <span>Number of children</span> 
                <vue-numeric-input  v-model="filter.people.children" :min="0" :max="10" :step="1" @input="onFilterChange"></vue-numeric-input>
                <span>Number of toddlers</span> 
                <vue-numeric-input  v-model="filter.people.toddlers" :min="0" :max="10" :step="1" @input="onFilterChange"></vue-numeric-input>
            </div>
            <div class="flex-3 trips">
                <div v-for="trip in trips" v-bind:key="trip.id" @click="open(trip)" class="trip-tile flex-column">
                    <b>{{trip.title}}</b>
                    <span>{{trip.arrival}}</span>
                    <span>{{formatDate(trip.startDate) + ' - ' + formatDate(trip.endDate)}}</span>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

import moment from "moment";
import {getArrivalPlaces, getDeparturePlaces} from './places';
import TripView from "./TripView.vue";

export default {
    data() {
        return {
            destinations: getArrivalPlaces(),
            placesOfDepartures: getDeparturePlaces(),
            filter: {
                destination: null,
                placeOfDeparture: null,
                departureDate: new Date(),
                people: {
                    adults: 2,
                    teenagers: 0,
                    children: 0,
                    toddlers: 0,
                }
            },
            trips: []
        };
    },
    methods: {
        onFilterChange() {
            console.log(this.filter);

            this.$http.post('/api/offers/search', this.filter)
            .then(res => {
                this.trips = res.body;
            });
        },
        formatDate(date) {
            return moment(date).format("DD MM YYYY, h:mm:ss");
        },
        open(trip) {
            this.$router.push({ name: "trip", params: { trip: trip } });
        }
    },
    created() {
        this.onFilterChange();
    },
    components: { TripView }
}
</script>

<style scoped>

.offer-search-view {
    margin: 0 10%;
}
.flex-row{
    display: flex;
    flex-direction: row;
}

.flex-column{
    display: flex;
    flex-direction: column;
}

.flex-1 {
    flex: 1;
}

.flex-3 {
    flex: 3;
}

.searcher span {
    margin-top: 15px
}

.trips {
    margin-left: 30px;
}

.trip-tile {
    padding: 10px;
    border-bottom: 1px solid gray;
}

.trip-tile:hover{
    cursor: pointer;
    background-color: beige;
}

</style>>