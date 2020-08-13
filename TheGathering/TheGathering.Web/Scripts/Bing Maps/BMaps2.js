/*
 *      Variables
 */
const API_KEY = 'Au02QBwR7dBBUZiE9NK_er_E7iVbAFbx9EsiHxA3xLOTK6ry7J-Okb9DqZEW98qb';
const GATHERING_PIN_URL = "/Content/Logo/The_Gathering_Icon.png";
const NON_GATHERING_PIN_URL = "/Content/Icons/Fork_Knife.png";
const LOCATOR_PIN_COLOR = "red";
const DEFAULT_ZOOM = 10;
const POINT_ZOOM = 12;

var locations = [];
var map;
var totalAvg = 0;
var longAvg = 0;
var latAvg = 0;
var currentPin = -1;

// Location Class
class Location {
    constructor(address, id, lat, lon, name, isGathering) {
        this.address = address;
        this.longitude = lon;
        this.latitude = lat;
        this.databaseId = id;
        this.name = name;
        this.isGathering = isGathering;
        if (typeof Microsoft !== 'undefined')
            this.locObj = new Microsoft.Maps.Location(this.latitude, this.longitude);
        return 0;
    }
}

/*
 *      Initialization
 */
function init() {
    // Initialize Map
    map = new Microsoft.Maps.Map('#myMap', {
        credentials: API_KEY,
        center: new Microsoft.Maps.Location(51.50632, -0.12714),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: DEFAULT_ZOOM
    });

    // Initialize Points
    locations.forEach(function(location, index){
        // Set Objects
        location.locObj = new Microsoft.Maps.Location(location.latitude, location.longitude);

        // Find Center Location
        longAvg = ((longAvg * totalAvg) + location.longitude) / (totalAvg + 1);
        latAvg = ((latAvg * totalAvg) + location.latitude) / (totalAvg + 1);
        totalAvg++;
        var centerPoint = new Microsoft.Maps.Location(latAvg, longAvg);

        // Set Center Location
        map.setView({
            center: centerPoint
        });

        // Initialize Pushpin
        var pin;
        if (location.isGathering) {
            pin = new Microsoft.Maps.Pushpin(location.locObj, {
                title: location.address,
                icon: GATHERING_PIN_URL
            });
        }
        else {
            pin = new Microsoft.Maps.Pushpin(location.locObj, {
                title: location.address,
                icon: NON_GATHERING_PIN_URL
            });
        }
        map.entities.push(pin);
    });
}

/*
 *      Location Locator
 */

async function getMatrix(zip) {
    // Convert Zip to Location Object
    var locData = await getLocationData(zip);
    var orgin = new Location(zip, 0, locData.point.coordinates[0], locData.point.coordinates[1]);

    // Place Pin on Zip
    if (currentPin != -1)
        map.entities.removeAt(currentPin);
    var pin = new Microsoft.Maps.Pushpin(orgin.locObj, {
        color: LOCATOR_PIN_COLOR,
        title: zip
    });
    map.entities.push(pin);
    currentPin = map.entities.getLength() - 1;

    // Convert Locations to URL String
    var orginString = orgin.latitude + "," + orgin.longitude;
    var locationsString = "";
    locations.forEach((location, index) => {
        locationsString += location.latitude + "," + location.longitude;
        if (index != locations.length - 1)
            locationsString += ";";
    });

    // Load JSON from URL
    var json = await getJsonFromURL("https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?key=" + API_KEY + "&travelMode=driving&origins=" + orginString + "&destinations=" + locationsString);
    return json.resourceSets[0].resources[0].results;
}

async function sortMatrix() {
    // Grab Parameters
    var locationReturn = document.getElementById("zipReturn");
    var inputZip = document.getElementById("zip").value;

    // Check if Zip is Existant
    if (inputZip == "") {
        locationReturn.innerHTML = "<center>Invalid Location</center>";
        return;
    }

    // Grab Parameters
    locationReturn.innerHTML = "<center>Searching...</center>";
    var matrix = await getMatrix(inputZip);

    // Check in Location is Valid
    if (matrix == null) {
        locationReturn.innerHTML = "<center>Invalid Location</center>";
        return;
    }

    // Sort Matrix by Distance
    matrix.sort(function (a, b) {
        var aDist = a.travelDistance;
        var bDist = b.travelDistance;
        return aDist - bDist;
    });

    // Return Matrix Values
    locationReturn.innerHTML = "";
    clearPushpinNumbers();
    for (var o = 0; o < matrix.length && o < 5; o++) {
        var locationMatrix = matrix[o];
        var locationObj = locations[locationMatrix.destinationIndex];

        // HTML Manipulations
        var listHTML = document.createElement("button");
            listHTML.type = "button";
            listHTML.setAttribute("onclick", "zoomToPoint(" + locations[locationMatrix.destinationIndex].latitude + "," + locations[locationMatrix.destinationIndex].longitude + ");");
            listHTML.className = "list-group-item";
        // Name
        var titleHTML = document.createElement("h4");
            titleHTML.className = "list-group-item-heading";
            titleHTML.innerText = locationObj.name;
        // Address
        var addressHTML = document.createElement("p");
            addressHTML.className = "list-group-item-text";
            addressHTML.innerText = locationObj.address;
        // Distance
        var distanceText = (Math.round(locationMatrix.travelDistance * 10) / 10) + " miles away";
            if (locationMatrix.travelDistance < 0)
                distanceText = "Cannot calculate distance";
        var distHTML = document.createElement("p");
            distHTML.className = "list-group-item-text";
            distHTML.innerText = distanceText;
        // Not Part of The Gathering
        var notGathering = document.createElement("p");
        if (!locationObj.isGathering) {
            notGathering.className = "list-group-item-text";
            notGathering.innerText = "(Not Part of The Gathering)"
        }
        // Details
        var detailsHTML = document.createElement("a");
            detailsHTML.className = "list-group-item-text";
            detailsHTML.innerText = "Details";
            detailsHTML.href = "/MealSite/Details?id=" + locationObj.databaseId;
        // Apply
            listHTML.appendChild(titleHTML);
            listHTML.appendChild(addressHTML);
            listHTML.appendChild(distHTML);
            listHTML.appendChild(notGathering);
            listHTML.appendChild(detailsHTML);
            locationReturn.appendChild(listHTML);

        // Add Numbers to Pushpins
        for (var i = 0; i < map.entities.getLength(); i++) {
            if (map.entities.get(i).getTitle() == locationObj.address) {
                map.entities.get(i).setOptions({
                    text: (o + 1).toString()
                });
            }
        }

        // Zoom in on Closest Location
        if (o == 0) {
            zoomToPoint(locations[locationMatrix.destinationIndex].latitude, locations[locationMatrix.destinationIndex].longitude);
        }
    }
}

/*
 *      Update Longitude and Latitude Hiddens
 */
async function updateLonLat()
{
    var address = document.getElementById("addressLineTbx").value + ", " + document.getElementById("cityTbx").value;

    var locData = await getLocationData(address);

    // Verify Location Exists
    if (!(locData)) {
        document.getElementById("submit").disabled = true;
        document.getElementById("addressLineGroup").className = "form-group has-error";
        document.getElementById("addressLineReturn").innerText = "Invalid Location";
        return true;
    }
    if (locData.entityType == "Address" && locData.confidence != "Low") {
        document.getElementById("submit").disabled = false;
        document.getElementById("addressLineGroup").className = "form-group";
        document.getElementById("addressLineReturn").innerText = "";
        //document.getElementById("addressLineTbx").value = locData.address.addressLine;
        //document.getElementById("cityTbx").value = locData.address.locality;
        var latitude = document.getElementById("lat");
        var longitude = document.getElementById("lon");
        latitude.value = locData.point.coordinates[0];
        longitude.value = locData.point.coordinates[1];
        return false;
    }
    else {
        document.getElementById("submit").disabled = true;
        document.getElementById("addressLineGroup").className = "form-group has-error";
        document.getElementById("addressLineReturn").innerText = "Invalid Location";
        return true;
    }
}

/*
 *      Autocomplete
 */
function initAutosuggest() {
    Microsoft.Maps.loadModule('Microsoft.Maps.AutoSuggest', {
        callback: function () {
            var manager = new Microsoft.Maps.AutosuggestManager({
                placeSuggestions: false
            });
            manager.attachAutosuggest('#searchBox', '#searchBoxContainer', selectedSuggestion);
        },
        errorCallback: function (msg) {
            alert(msg);
        },
        credentials: API_KEY
    });
}

function selectedSuggestion(result) {
    document.getElementById('addressLineTbx').value = result.address.addressLine || '';
    document.getElementById('cityTbx').value = result.address.locality || '';
    document.getElementById('stateTbx').value = result.address.adminDistrict || '';
    document.getElementById('postalCodeTbx').value = result.address.postalCode || '';

    updateLonLat();
}

/*
 *      Misc Functions
 */
async function getJsonFromURL(url) {
    const response = await fetch(url);
    const json = await response.json();
    return json;
}

async function getLocationData(location) {
    const json = await getJsonFromURL('https://dev.virtualearth.net/REST/v1/Locations?key=' + API_KEY + '&q=' + location);
    return json.resourceSets[0].resources[0];
}

function zoomToPoint(latitude, longitude) {
    map.setView({
        center: new Microsoft.Maps.Location(latitude, longitude),
        zoom: POINT_ZOOM
    });
}

function clearPushpinNumbers() {
    for (var i = 0; i < map.entities.getLength(); i++) {
        map.entities.get(i).setOptions({
            text: ""
        });
    }
}

function checkLonLat() {
    $("#mealSiteForm").submit(async function (e) {
        var latitude = document.getElementById("lat");
        var longitude = document.getElementById("lon");

        if (latitude.value == "" || longitude.value == "") {
            document.getElementById("addressLineGroup").className = "form-group has-error";
            document.getElementById("addressLineReturn").innerText = "Invalid Location";
            e.preventDefault();
        }
    });
}