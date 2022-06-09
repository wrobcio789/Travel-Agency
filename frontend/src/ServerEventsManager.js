import { HubConnectionBuilder } from '@microsoft/signalr';

const OFFER_BOUGHT = "OfferBought";
const TOUR_CHANGE = "TourChange";

export default class ServerEventsManager {
    init(){
        this.dispatchers = {};
        this.dispatchers[OFFER_BOUGHT] = new EventsDispatcher();
        this.dispatchers[TOUR_CHANGE] = new EventsDispatcher();

        this.connection = new HubConnectionBuilder()
            .withUrl("/api/offers")
            .build();

        Object.keys(this.dispatchers).forEach(
            key => this.connection.on(data => { this.dispatchers[key].dispatch(data) })
        );


        this.connection.start()
            .then(() => console.log("SignalR connection initialized"));
    }

    getOfferBoughtDispatcher(){
        return this.dispatchers[OFFER_BOUGHT];
    }

    getTourChangeDispatcher() {
        return this.dispatchers[TOUR_CHANGE];
    }
}

class EventsDispatcher {
    constructor(){
        this.eventListeners = [];
    }

    add(listener) {
        this.eventListeners.push(listener);
    }

    remove(listener) {
        this.eventListeners = this.eventListeners.filter(elem => elem != listener);
    }

    dispatch(data) {
        this.eventListeners.forEach(listener => listener(data));
    }
}