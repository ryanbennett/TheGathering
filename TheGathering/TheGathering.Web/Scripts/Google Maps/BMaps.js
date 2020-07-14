// Variables
var totalAvg = 0;
var longAvg = 0;
var latAvg = 0;
var map;

// Mapping
function initMap() {
    // Make Map
    map = new Microsoft.Maps.Map('#myMap', {
        credentials: 'Au02QBwR7dBBUZiE9NK_er_E7iVbAFbx9EsiHxA3xLOTK6ry7J-Okb9DqZEW98qb',
        center: new Microsoft.Maps.Location(51.50632, -0.12714),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 10
    });

    if (locations) {
        // Iterate through each Location
        for (var i = 0; i < locations.length; i++) {
            // Get Lon and Lat from Address
            getLongLat(locations[i]).then((coords) => {
                // Get Point from Coordinates
                var point = new Microsoft.Maps.Location(coords[0], coords[1]);

                // Find Average Location using Maths
                longAvg = ((longAvg * totalAvg) + coords[0]) / (totalAvg + 1);
                latAvg = ((latAvg * totalAvg) + coords[1]) / (totalAvg + 1);
                totalAvg++;
                var centerPoint = new Microsoft.Maps.Location(longAvg, latAvg);

                // Set Center
                map.setView({
                    center: centerPoint
                });

                // Set Pin
                var pin = new Microsoft.Maps.Pushpin(point, {
                    title: coords.addr
                });
                map.entities.push(pin);
            });
            
        }


    }
}

// Zoom Map to Point
function zoomToPoint(lon, lat) {
    map.setView({
        center: new Microsoft.Maps.Location(lon, lat),
        zoom: 12
    });
}

// Gets Bing's Matrix from a Zipcode and Address Array
async function getMatrix(orginZip, destinationAddresses) {

    // Convert Orgin Point
    const coords = await getLongLat(orginZip);
    var orgin = new Microsoft.Maps.Location(coords[0], coords[1]);

    // Convert Destination Points
    var destinations = [];
    for (var i = 0; i < destinationAddresses.length; i++) {
        var dCoords = await getLongLat(destinationAddresses[i]);
        var dest = new Microsoft.Maps.Location(dCoords[0], coords[1]);
        destinations.push(dest);
    }

    // Input into Matrix
    const userAction = async () => {
        // Convert Orgin + Destination to URL Strings
        var orginStr = orgin.latitude + "," + orgin.longitude;
        var destStr = "";
        for (var i = 0; i < destinations.length; i++) {
            destStr += destinations[i].latitude + "," + destinations[i].longitude;
            if (i != destinations.length - 1)
                destStr += ";";
        }

        // Call Matrix REST API
        const response = await fetch("https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?key=Au02QBwR7dBBUZiE9NK_er_E7iVbAFbx9EsiHxA3xLOTK6ry7J-Okb9DqZEW98qb&travelMode=driving&origins=" + orginStr + "&destinations=" + destStr);
        const myJson = await response.json();
        const sorted = myJson.resourceSets[0].resources[0].results;

        return sorted;
    };

    return await userAction();
}

// Gets Closest Locations by Zip
async function sortZip() {
    if (locations) {
        // Say "Searching..."
        var zipReturn = document.getElementById("zipReturn");
        zipReturn.innerHTML = "Searching...";

        // Get Zip + Matrix
        var zip = document.getElementById("zip").value;
        var matrix = await getMatrix(zip, locations);

        // Sort Matrix
        matrix.sort(function (a, b) {
            var aDist = a.travelDistance;
            var bDist = b.travelDistance;
            return aDist - bDist;
        });

        // Return Matrix
        zipReturn.innerHTML = "";
        for (var o = 0; o < matrix.length && o < 5; o++) {
            var item = matrix[o];

            // HTML Sidebar List

            // List
            var itemA = document.createElement("li");
            itemA.className = "list-group-item";

            // Heading
            var title = document.createElement("h4");
            title.className = "list-group-item-heading";
            title.innerText = await getCity(locations[item.destinationIndex]);

            // Address
            var itemB = document.createElement("p");
            itemB.className = "list-group-item-text";
            itemB.innerText = locations[item.destinationIndex];

            // Distance
            var itemC = document.createElement("p");
            itemC.className = "list-group-item-text";
            itemC.innerText = Math.round(item.travelDistance*10)/10 + " miles away";

            // Apply
            itemA.appendChild(title);
            itemA.appendChild(itemB);
            itemA.appendChild(itemC);
            zipReturn.appendChild(itemA);

            // Add Number to Pushpin
            for (var i = 0; i < map.entities.getLength(); i++) {
                if (map.entities.get(i).getTitle() == locations[item.destinationIndex]) {
                    map.entities.get(i).setOptions({
                        text: (o + 1).toString()
                    });
                    console.log((o + 1).toString());
                }
            }

            // Zoom in on Closest Location
            if (o == 0) {
                getLongLat(locations[item.destinationIndex]).then((coords) => {
                    zoomToPoint(coords[0], coords[1]);
                });
            }
        }
    }

}

// Gets Longitude and Latitude from a given Address
async function getLongLat(address) {
    var data = await getLocationData(address);
    var coords = data.point.coordinates;
    coords.addr = address;
    return coords;
}

// Gets City from a given Address
async function getCity(address) {
    var data = await getLocationData(address);
    return data.address.locality;
}

// Gets Location Data from a given Address
async function getLocationData(address) {
    const userAction = async () => {
        const response = await fetch('https://dev.virtualearth.net/REST/v1/Locations?key=Au02QBwR7dBBUZiE9NK_er_E7iVbAFbx9EsiHxA3xLOTK6ry7J-Okb9DqZEW98qb&q=' + address);
        const myJson = await response.json();
        const data = myJson.resourceSets[0].resources[0];
        return data;
    };

    return await userAction();
}

// Autocomplete
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

// More Autocomplete
function selectedSuggestion(result) {
    document.getElementById('addressLineTbx').value = result.address.addressLine || '';
    document.getElementById('cityTbx').value = result.address.locality || '';
    document.getElementById('stateTbx').value = result.address.adminDistrict || '';
    document.getElementById('postalCodeTbx').value = result.address.postalCode || '';
}