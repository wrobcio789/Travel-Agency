import { HubConnectionBuilder } from '@microsoft/signalr';

const OFFER_BOUGHT = "OfferBought";
const TOUR_CHANGE = "TourChange";

export default class ServerEventsManager {
    init(){
        this.dispatchers = {};
        this.dispatchers[OFFER_BOUGHT] = new EventsDispatcher();
        this.dispatchers[TOUR_CHANGE] = new EventsDispatcher();

        this.connection = new HubConnectionBuilder()
            .withUrl("/offerHub")
            .build();

        const self = this;
        this.connection.on("Message", function(type, content){
            console.log(type);
            console.log(content);
            self.dispatchers[type].dispatch(content);
        })


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