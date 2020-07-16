/*
 *      Variables
 */
var locationAddresses = [];
var locationIds = [];
var locations = [];
var map;
var totalAvg = 0;
var longAvg = 0;
var latAvg = 0;
var currentPin = -1;

class Location {
    async init(address, id) {
        var locData = await getLocationData(address);

        if (locData == undefined)
            return 1;

        this.address = locData.address.addressLine;
        this.longitude = locData.point.coordinates[1];
        this.latitude = locData.point.coordinates[0];
        this.city = locData.address.locality;
        this.databaseId = id;
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
        credentials: 'Au02QBwR7dBBUZiE9NK_er_E7iVbAFbx9EsiHxA3xLOTK6ry7J-Okb9DqZEW98qb',
        center: new Microsoft.Maps.Location(51.50632, -0.12714),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 10
    });

    // Initialize Points
    locationAddresses.forEach(async function(address, index){
        // Convert to Location Object
        locations.push(new Location());
        var location = locations[index];
        await location.init(address, locationIds[index]);

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
        var pin = new Microsoft.Maps.Pushpin(location.locObj, {
            title: location.address,
            color: "#434345"
        });
        map.entities.push(pin);
    });
}

/*
 *      Location Locator
 */

async function getMatrix(zip) {
    // Convert Zip to Location Object
    var orgin = new Location();
    var statusCode = await orgin.init(zip);

    if (statusCode != 0) {
        return null;
    }

    // Place Pin on Zip
    if (currentPin != -1)
        map.entities.removeAt(currentPin);
    var pin = new Microsoft.Maps.Pushpin(orgin.locObj, {
        color: "red",
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
    var json = await getJsonFromURL("https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?key=Au02QBwR7dBBUZiE9NK_er_E7iVbAFbx9EsiHxA3xLOTK6ry7J-Okb9DqZEW98qb&travelMode=driving&origins=" + orginString + "&destinations=" + locationsString);
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
        // City (Heading)
        var titleHTML = document.createElement("h4");
        titleHTML.className = "list-group-item-heading";
        titleHTML.innerText = locationObj.city;
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
        // Details
        var detailsHTML = document.createElement("a");
        detailsHTML.className = "list-group-item-text";
        detailsHTML.innerText = "Details";
        detailsHTML.href = "/MealSite/Details?id=" + locationObj.databaseId;
        // Apply
        listHTML.appendChild(titleHTML);
        listHTML.appendChild(addressHTML);
        listHTML.appendChild(distHTML);
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
        credentials: 'Au02QBwR7dBBUZiE9NK_er_E7iVbAFbx9EsiHxA3xLOTK6ry7J-Okb9DqZEW98qb'
    });
}

function selectedSuggestion(result) {
    document.getElementById('addressLineTbx').value = result.address.addressLine || '';
    document.getElementById('cityTbx').value = result.address.locality || '';
    document.getElementById('stateTbx').value = result.address.adminDistrict || '';
    document.getElementById('postalCodeTbx').value = result.address.postalCode || '';
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
    const json = await getJsonFromURL('https://dev.virtualearth.net/REST/v1/Locations?key=Au02QBwR7dBBUZiE9NK_er_E7iVbAFbx9EsiHxA3xLOTK6ry7J-Okb9DqZEW98qb&q=' + location);
    return json.resourceSets[0].resources[0];
}

function zoomToPoint(latitude, longitude) {
    map.setView({
        center: new Microsoft.Maps.Location(latitude, longitude),
        zoom: 12
    });
}

function clearPushpinNumbers() {
    for (var i = 0; i < map.entities.getLength(); i++) {
        map.entities.get(i).setOptions({
            text: ""
        });
    }
}