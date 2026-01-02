// JavaScript code for WeatherForecast project.

async function initMap1() {
    const mapElement = document.querySelector('gmp-map');

    if (mapElement) {

        // Access the inner map instance to add the standard Maps API listener
        const innerMap = await mapElement.innerMap;

        // Add a 'click' listener to the inner map
        innerMap.addListener('click', (e) => {
            // e is a google.maps.MapMouseEvent
            const latitude = e.latLng.lat(); // Method to get latitude
            const longitude = e.latLng.lng(); // Method to get longitude
            //--alert('Clicked Latitude:' + latitude);
            //--alert('Clicked Longitude:' + longitude);

            const markerElement = document.querySelector('gmp-advanced-marker');
            let mpos = { lat: latitude, lng: longitude };
            markerElement.position = mpos;
            markerElement.latitude = latitude;
            markerElement.longitude = longitude;

            // You can also get a LatLngLiteral
            const latLngLiteral = e.latLng.toJSON();

            const nightValues = ["Tonight", "Overnight"];

            $.ajax({
                url: `https://localhost:7167/api/WeatherForecast?lat=${latitude}&lon=${longitude}`, 
                type: "GET",
                success: function (data) {
                    if (data.longitude !== null && data.latitude !== null) {
                        const spLat = document.getElementById('spLat');
                        const spLng = document.getElementById('spLng');
                        spLat.innerHTML = data.latitude.toFixed(7);
                        spLng.innerHTML = data.longitude.toFixed(7);
                    }
                    if (data.city !== null && data.state !== null) {
                        const spLocationName = document.getElementById('spLocationName');
                        spLocationName.innerHTML = data.city + ', ' + data.state;
                    } else {
                        //alert('City, state not found'); this will show as Unavailable, so no need for alert
                        spLocationName.innerHTML = "Unavailable";
                    }
                    var blockNum = 1;
                    const maxBlocks = 12;
                    const daytimeClass = 'class=\"daytimeForecast\"';
                    const nighttimeClass = 'class=\"nighttimeForecast\"';
                    var forecastTextClass = daytimeClass;
                    if (data.success) {
                        for (let i1 = 0; i1 < data.dayForecasts.length; i1++) {
                            if (blockNum === 1 && nightValues.indexOf(data.dayForecasts[i1].dayOfWeek) > -1) {
                                blockNum++;
                            }
                            forecastTextClass = (blockNum % 2 == 0) ? nighttimeClass : daytimeClass;
                            // Fill in <div> with id 'fcdiv-' + blockNum
                            var divId = 'fcdiv-' + (blockNum < 10 ? '0' : '') + blockNum.toString();
                            var contentId = 'popupContent-' + (blockNum < 10 ? '0' : '') + blockNum.toString();
                            var fcdiv = document.getElementById(divId);
                            if (fcdiv) {
                                fcdiv.innerHTML = '<abbr title=\"'
                                    + data.dayForecasts[i1].details + '\">'
                                    + '<p ' + forecastTextClass + '>' + (data.dayForecasts[i1].dayOfWeek || 'dow')
                                    + ' ' + (data.dayForecasts[i1].partOfDay || '') + '</p>'
                                    + '<img src=\"' + data.dayForecasts[i1].icon + '\" />'
                                    + '<p ' + forecastTextClass + '>' + data.dayForecasts[i1].temperatureF.toString() + ' F' + '</p>'
                                    + '<p ' + forecastTextClass + '>' + (data.dayForecasts[i1].winds || 'winds') + '</p>'
                                    + '<p ' + forecastTextClass + '>' + (data.dayForecasts[i1].summary ?? '-') + '</p>'
                                    + '</abbr>'
                                    ;
                            } else {
                                alert('div not found');
                            }
                            blockNum++;
                            if (blockNum > maxBlocks) {
                                break;
                            }
                        }
                    } else {
                        alert('Unsuccessful: ' + data.message);
                    }

                    // Clear the rest of the forecasts not filled in
                    if (blockNum < maxBlocks) {
                        for (let i2 = blockNum; i2 <= maxBlocks; i2++) {
                            // Fill in <div> with id 'fcdiv-' + blockNum
                            var divId = 'fcdiv-' + (i2 < 10 ? '0' : '') + i2.toString();
                            var fcdiv = document.getElementById(divId);
                            if (fcdiv) {
                                fcdiv.innerHTML = '<p>' + '&nbsp;' + '</p>'
                                    + '<img src=\"' + '' + '\" />'
                                    + '<p>' + '&nbsp;' + '</p>'
                                    + '<p>' + '&nbsp;' + '</p>'
                                    + '<p>' + '&nbsp;' + '</p>'
                                    ;
                            } else {
                                alert('div not found');
                            }
                        }
                    }

                    setTimeout(checkContainerSize, 1000);

                    console.log(data);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('error: ' + xhr.status + '-' + xhr.error);
                    alert(thrownError);
                }
            });

        });

        mapElement.addEventListener('click', (event) => {
            // Access latitude and longitude of the click
            const latLng = event.latLng; //event.detail.latLng;
            if (latLng !== null && typeof (latLng) !== 'undefined') {
                //--alert('Map clicked at:', latLng.lat(), latLng.lng());
            } else {
                //--alert('no lat/lng');
            }
            // Add custom functionality here (e.g., place a new marker)
        });
    }
    else {
        alert('undefined');
    }
}

function checkContainerSize() {
    var maxHeight = 0;
    $(".forecastday").each(function () {
        if ($(this).height() > maxHeight) {
            maxHeight = $(this).height();
        }
    });

    // Also check height from the half-day forecasts added together
    for (var i1 = 1; i1 <= 6; i1++) {
        var dayNum = (i1 * 2) - 1;
        var nightNum = i1 * 2;
        var dayElementId = '#fcdiv-' + (dayNum < 10 ? '0' : '') + dayNum.toString();
        var nightElementId = '#fcdiv-' + (nightNum < 10 ? '0' : '') + nightNum.toString();
        var fullDayHeight = $(dayElementId).height() + $(nightElementId).height();
        if (fullDayHeight > maxHeight) {
            maxHeight = fullDayHeight * 1.02;
        }
    }

    $(".forecastday").height(maxHeight);
    $(".forecastcontainer").height(maxHeight + 10);

}

if (! window.location.pathname.toLowerCase().includes('/home/privacy')) {
    window.onload = initMap1;
}

// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
