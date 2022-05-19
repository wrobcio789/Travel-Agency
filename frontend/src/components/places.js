const arrivals = [
    "Algarve",
    "Attyka i Evia",
    "Bali",
    "Chalkidiki",
    "Costa Barcelona",
    "Costa de la Luz",
    "Costa del Sol",
    "Dalmacja Południowa",
    "Dalmacja Środkowa",
    "Djerba",
    "Fuerteventura",
    "Hurghada",
    "Korfu",
    "Kos",
    "Kreta",
    "La Gomera",
    "Lanzarote",
    "Larnaka",
    "Madera",
    "Majorka",
    "Marsa Alam",
    "Minorka",
    "Mykonos",
    "Pafos",
    "Prowincja Madryt",
    "Punta Cana",
    "Riwiera Bułgarska",
    "Riwiera Turecka",
    "Rodos",
    "Samos",
    "Santorini",
    "Sharm el Sheikh",
    "Tunezja kontynentalna",
    "Turcja Egejska",
    "Varadero",
    "Wyspa Phuket",
    "Wyspa Sal"
];

const placesOfDeparture = [
    "Alghero",
    "Amsterdam",
    "Antalia",
    "Barcelona",
    "Bari",
    "Bazylea",
    "Belgrad",
    "Berlin",
    "Billund",
    "Birmingham",
    "Bruksela",
    "Budapeszt",
    "Bukareszt",
    "Burgas",
    "Catania",
    "Charków",
    "Charleroi-Gosselies",
    "Dalaman",
    "Delhi",
    "Doha",
    "Doncaster-Sheffield",
    "Dubai",
    "Dusseldorf",
    "Eindhoven",
    "Erywań",
    "Frankfurt",
    "Funchal Madeira",
    "Gdańsk",
    "Genewa",
    "Goeteborg-Landvetter",
    "Gran Canaria",
    "Hamburg",
    "Helsinki",
    "Katowice",
    "Kijów-Żuliany",
    "Kluż-Napoka",
    "Kopenhaga",
    "Koszyce",
    "Kraków",
    "Kutaisi",
    "Larnaca",
    "Lizbona",
    "Londyn-Heathrow",
    "Londyn-Luton",
    "Los Angeles",
    "Lublin",
    "Luksemburg",
    "Madryt",
    "Malaga",
    "Malta",
    "Marsa-Alam",
    "Mediolan",
    "Monachium",
    "Nicea",
    "Nowy Jork JFK",
    "Nykoping-Skavsta",
    "Oslo",
    "Paryż",
    "Paryż-Orly",
    "Podgorica",
    "Poznań",
    "Praga",
    "Ryga",
    "Rzeszów",
    "Rzym",
    "Sandefjord-Torp",
    "Seul",
    "Sharm El Sheikh",
    "Sofia",
    "Stambuł",
    "Szczecin",
    "Sztokholm",
    "Tallinn",
    "Tbilisi",
    "Tel Awiw",
    "Teneryfa",
    "Tirana",
    "Toronto",
    "Wenecja",
    "Wiedeń",
    "Wilno",
    "Wrocław",
    "Zakynthos",
    "Zielona Góra",
    "Zurych"
];

const transportationTypes = [
    {code: "BUS", label: "bus"},
    {code: "PLAIN", label: "plain"},
    {code: "TRAIN", label: "train"},
]

export function getArrivalPlaces() {
    return arrivals;
};

export function getDeparturePlaces() {
    return placesOfDeparture;
}

export function getTransportationTypes() {
    return transportationTypes;
}