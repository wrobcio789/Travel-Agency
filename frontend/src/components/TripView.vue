<template>
    <div class="trip-view">
        <div class="flex-row">
            <div class="searcher flex-1 flex-column">
                <b class="group-label">Transport to {{trip.arrival}}</b> 
                <v-select :options="placesOfDepartures" v-model="offerRequest.transportationTo.departure"></v-select>
                <!-- <v-select :options="transportationTypes" v-model="offerRequest.transportationTo.type" :reduce="type => type.type" :label="'label'"></v-select> -->
                <b class="group-label">Transport from {{trip.arrival}} </b> 
                <!-- <v-select :options="transportationTypes" v-model="offerRequest.transportationFrom.type" :reduce="type => type.type" :label="'label'"></v-select> -->
                <b class="group-label">Participants</b>
                <span>Number of adults</span> 
                <vue-numeric-input  v-model="offerRequest.people.adults" :min="1" :max="10" :step="1"></vue-numeric-input>
                <span>Number of teenagers</span> 
                <vue-numeric-input  v-model="offerRequest.people.teenagers" :min="0" :max="10" :step="1"></vue-numeric-input>
                <span>Number of children</span> 
                <vue-numeric-input  v-model="offerRequest.people.children" :min="0" :max="10" :step="1"></vue-numeric-input>
                <span>Number of toddlers</span> 
                <vue-numeric-input  v-model="offerRequest.people.toddlers" :min="0" :max="10" :step="1"></vue-numeric-input>
                <b class="group-label">Hotel</b>
                <v-select :options="hotels" v-model="offerRequest.accommodation.hotelId" :reduce="hotel => hotel.id" :label="'name'"></v-select>
                <span>Number of small rooms</span> 
                <vue-numeric-input  v-model="offerRequest.accommodation.smallRooms" :min="0" :max="10" :step="1"></vue-numeric-input>
                <span>Number of medium rooms</span> 
                <vue-numeric-input  v-model="offerRequest.accommodation.mediumRooms" :min="0" :max="10" :step="1"></vue-numeric-input>
                <span>Number of large rooms</span> 
                <vue-numeric-input  v-model="offerRequest.accommodation.largeRooms" :min="0" :max="10" :step="1"></vue-numeric-input>
                <b class="group-label">Promo code</b> 
                <input  v-model="offerRequest.promoCode" placeholder="'promo code'"/>

                <button class="availability-button" @click="checkAvailability()">Check availability</button>
                <div class="ok-label" v-if="isAvailable === true">Trip is available in selected configuration</div>
                <div class="error-label" v-if="isAvailable === false">Trip is unavailable in selected configuration</div>
            </div>
            <div class="flex-3 trip flex-column">
                <b>{{trip.title}}</b>
                <span>{{trip.arrival}}</span>
                <span>{{formatDate(trip.startDate) + ' - ' + formatDate(trip.endDate)}}</span>

                <div v-if="order && !order.isFinalized" class="payment-tile flex-column">
                    <b class="group-label">Credit card data</b>
                    <span>{{"Price: " + order.price}}</span>
                    <input v-model="creditCardData.creditCardNumber" placeholder="Number"/>
                    <input type="number" v-model="creditCardData.cvCoder" placeholder="CV"/>
                    <input v-model="creditCardData.expirationDate" placeholder="Exp date"/>
                    <span v-if="order.paymentError" class="error-label">Payment unsuccesful</span>
                    <button class="availability-button" @click="payForOrder()">Pay for order</button>
                </div>
                <span v-if="order && order.isFinalized === true" class="ok-label">Offer bought successfuly</span>
                <button v-if="isAvailable && !order" class="availability-button" @click="buyTrip()">Buy trip in the configuration</button>
            </div>
        </div>
    </div>
</template>

<script>

import moment from "moment";
import {getArrivalPlaces, getDeparturePlaces, getTransportationTypes} from './places';

export default {
    data() {
        return {
            offerRequest: {
                tourId: null,
                people: {
                    adults: 2,
                    teenagers: 0,
                    children: 0,
                    toddlers: 0,
                },
                transporationFrom: {
                    departure: null,
                    type: "PLANE",
                },
                transportationTo: {
                    departure: null,
                    type: "PLANE",
                },
                accommodation: {
                    hotelId: null,
                    smallRooms: 0,
                    mediumRooms: 0,
                    largeRooms: 0,
                    numberOfMeals: 3,
                },
                promoCode: null,

            },
            destinations: getArrivalPlaces(),
            placesOfDepartures: getDeparturePlaces(),
            transportationTypes: getTransportationTypes(),
            hotels: [],
            isAvailable: null,
            order: null,
            creditCardData: {}
        };
    },
    created() {
        this.offerRequest.tourId = this.trip.id;
        this.offerRequest.transportationTo.departure = this.trip.arrival;
        this.setHotels();
    },
    methods: {
        checkAvailability() {
            this.$http.post('/api/offers/availability', this.offerRequest)
            .then(res => {
                this.isAvailable = res.body.isAvailable;
            });
        },
        formatDate(date) {
            return moment(date).format('DD MM YYYY, h:mm:ss');
        },
        setHotels() {
            this.$http.get('/api/offers/hotels', {params: {place: this.trip.arrival}})
            .then(res => {
                this.hotels = res.body;
            });
        },
        buyTrip() {
            this.$http.post('/api/orders/make', this.offerRequest)
            .then(res => {
                this.order = res.body;
            });
        },
        payForOrder() {
            this.$http.post('/api/payments/pay', {paymentId: this.order.paymentId, cardData: this.creditCardData})
            .then(res => {
                this.order.isFinalized = true;
                this.order.paymentError = false;
            })
            .catch(erro => {
                this.order.isFinalized = false;
                this.order.paymentError = true;
            });

            this.creditCardData = {};
        }
    },
    props: {
        trip: {},
    }
}
</script>

<style scoped>

.trip-view {
    margin: 10px 10%;
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

.group-label {
    display: block;
    margin-top: 20px;
}

.searcher span {
    margin-top: 15px
}

.trip {
    margin-left: 30px;
    padding: 10px;
}

.error-label {
    color:red; 
}

.ok-label {
    color: green;
}

.availability-button {
    margin-top: 20px;
}

.payment-tile {
    margin-top: 30px;
    padding: 15px;
    width: 300px;
    border: 1px solid firebrick;
}

</style>>