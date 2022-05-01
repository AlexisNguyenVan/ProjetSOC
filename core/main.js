
function getRoute() {


    // Create an array containing the GPS positions you want to draw
    var start = document.getElementById("start").value
    var end = document.getElementById("end").value

    console.log(start);
    console.log(end);

    var url = "http://localhost:8001/Routing/REST/path?startName=" + start + "&endName=" + end;
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
    xhr.timeout = 20000;
    xhr.send();
    xhr.onload = placePoints;
}

function drawRoute(coords, color) {
    var polyline = new ol.geom.LineString(coords);
    // Coordinates need to be in the view's projection, which is
    // 'EPSG:3857' if nothing else is configured for your ol.View instance
    polyline.transform('EPSG:4326', 'EPSG:3857');
    // Configure the style of the line
    var lineStyle = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: color,
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

function placePoints() {
    var json = JSON.parse(this.responseText);
    drawRoute(json[0], '#FF0000')

    var iconFeature = new ol.Feature({
        geometry: new ol.geom.Point(ol.proj.fromLonLat([json[1][0][0], json[1][0][1]])),
        name: 'Bike Station',
        population: 4000,
        rainfall: 500,
    });
    var iconFeature2 = new ol.Feature({
        geometry: new ol.geom.Point(ol.proj.fromLonLat([json[2][0][0], json[2][0][1]])),
        name: 'Bike Station',
        population: 4000,
        rainfall: 500,
    });

    var iconStyle = new ol.style.Style({
        image: new ol.style.Icon({
            anchor: [0.5, 0.5],
            anchorXUnits: 'fraction',
            anchorYUnits: 'pixels',
            src: 'bike.png',
            scale:0.04
        }),
    });

    var source = new ol.source.Vector({
        features: [iconFeature]
    });
    var source2 = new ol.source.Vector({
        features: [iconFeature2]
    });

    var vector = new ol.layer.Vector({
        source: source,
        style: [iconStyle]
    });
    var vector2 = new ol.layer.Vector({
        source: source2,
        style: [iconStyle]
    });
    map.addLayer(vector);
    map.addLayer(vector2);
    drawRoute(json[1], '#0080FF')
    drawRoute(json[2], '#FF0000')
    map.getView().setCenter(ol.proj.transform([json[0][0][0], json[0][0][1]], 'EPSG:4326', 'EPSG:3857'));
    map.getView().setZoom(15);
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

