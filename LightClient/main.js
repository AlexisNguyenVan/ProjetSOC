function getRoute(coordS, coordE) {
    // Create an array containing the GPS positions you want to draw
    console.log("entrée:"+coordS);
    console.log("entrée2:"+coordE);
    var coordinates = [coordS, coordE];

    var start = coordinates[0].toString();
    var end = coordinates[1].toString();

    console.log(start);
    console.log(end);
    var url = "https://dev.virtualearth.net/REST/v1/Routes/Walking?wp.0=" + start + "&wp.1=" + end +
        "&routeAttributes=routePath&key=" + "AlS6X5SDL-NZamisSXA_EMRWvm-8zpNK1rDxM6tMwOHHQ_e2nIwYWaSLywYnM4D7";
    console.log(url);
    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            if (xhr.status == 200) {
                var linestring = JSON.parse(xhr.responseText).resourceSets[0].resources[0].routePath.line.coordinates;
                for (var i = 0; i < linestring.length; i++) {
                    linestring[i].reverse();
                }
                var layerLines = new ol.layer.Vector({
                    source: new ol.source.Vector({
                        features: [new ol.Feature({
                            geometry: new ol.geom.LineString(linestring).transform('EPSG:4326', 'EPSG:3857'),
                            name: 'Line'
                        })]
                    }),
                });
                map.addLayer(layerLines);
            }
        }
    }

    xhr.onerror = function (e) { console.log("error"); }
    xhr.ontimeout = function (e) { console.log("timeout"); }

    xhr.open("GET", url, true);
    xhr.timeout = 3000;
    xhr.send();
    xhr.onload = placePoints;
}

function placePoints() {
    var json = JSON.parse(this.responseText);
    var locations = json.resourceSets[0].resources[0].routePath.line.coordinates;
    // OpenLayers uses [lon, lat], not [lat, lon] for coordinates
    locations.map(function (l) {
        return l.reverse();
    });


    var polyline = new ol.geom.LineString(locations);
    // Coordinates need to be in the view's projection, which is
    // 'EPSG:3857' if nothing else is configured for your ol.View instance
    polyline.transform('EPSG:4326', 'EPSG:3857');
    // Configure the style of the line
    var lineStyle = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: '#0000CD',
            width: 3
        })
    });
    var feature = new ol.Feature(polyline);
    var source = new ol.source.Vector({
        features: [feature]
    });

    var vector = new ol.layer.Vector({
        source: source,
        style: [lineStyle]
    });
    map.addLayer(vector);
   
}


function contractsRetrieved() {
    var json = JSON.parse(this.responseText);
    for (var i = 0; i < json.length; i++) {
        var obj = json[i];
        console.log(obj);
        var option = document.createElement("option");
        option.setAttribute("value", obj.name);
        document.getElementById("contracts").appendChild(option);
    }
}

function retrieveAllContracts() {
    var key = document.getElementById("apiKey").value;
    var url = "https://api.jcdecaux.com/vls/v3/contracts?apiKey=" + key;
    var req = new XMLHttpRequest();
    req.open("get", url);
    req.setRequestHeader("Accept", "application/json");
    req.onload = contractsRetrieved;
    req.send();

}

function getDistanceFrom2GpsCoordinates(lat1, lon1, lat2, lon2) {
    // Radius of the earth in km
    var earthRadius = 6371;
    var dLat = deg2rad(lat2 - lat1);
    var dLon = deg2rad(lon2 - lon1);
    var a =
        Math.sin(dLat / 2) * Math.sin(dLat / 2) +
        Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
        Math.sin(dLon / 2) * Math.sin(dLon / 2)
        ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = earthRadius * c; // Distance in km
    return d;
}

function deg2rad(deg) {
    return deg * (Math.PI / 180)
}



function getClosestStation() {
    var key = document.getElementById("apiKey").value;
    var contract = document.getElementById("contract").value;
    var url = "https://api.jcdecaux.com/vls/v3/stations?contract=" + contract + "&apiKey=" + key;
    var req = new XMLHttpRequest();
    req.open("get", url);
    req.setRequestHeader("Accept", "application/json");
    req.send();
    req.onload = function () {
        var json = JSON.parse(req.response);
        var lat = document.getElementById("lat").value;
        var lon = document.getElementById("long").value;
        console.log(json[0]);
        var min = getDistanceFrom2GpsCoordinates(json[0].position.latitude, json[0].position.longitude, lat, lon);
        var closest = json[0];
        for (var i = 1; i < json.length; i++) {
            var dist = getDistanceFrom2GpsCoordinates(json[i].position.latitude, json[i].position.longitude, lat, lon);
            if (dist < min) {
                min = dist;
                closest = json[i];
            }
        }
        document.getElementById("closest").value = closest.name;
        map.getView().setCenter(ol.proj.transform([lon, lat], 'EPSG:4326', 'EPSG:3857'));
        map.getView().setZoom(17);
        getRoute([lat, lon], [closest.position.latitude, closest.position.longitude]);
    }
    
}



var map = new ol.Map({
    target: "map", // <-- This is the id of the div in which the map will be built.
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM()
        })
    ],

    view: new ol.View({
        center: ol.proj.fromLonLat([7.0985774, 43.6365619]), // <-- Those are the GPS coordinates to center the map to.
        zoom: 10 // You can adjust the default zoom.
    })

});

