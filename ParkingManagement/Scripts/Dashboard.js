$(document).ready(function () {
    fetchAllData();
    setInterval(fetchAllData, 9000);
});

function fetchAllData() {
    $('#parkingTable thead').empty();
    $('#parkingTable tbody').empty();
    $.ajax({
        url: '/Dashboard/FetchAllData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.sessionType == 1) {
                var header = '<tr>' +
                    '<th>' + "Parking Space Title" + '</th>' +
                    '<th>' + "Status" + '</th>' +
                    '<th>' + "Vehicle Registration Number" +'</th>' +
                    '<th>' + "Action"+ '</th>' +
                    '</tr>';
                $('#parkingTable thead').append(header);
                $.each(data.allspace, function (index, item) {
                    var statusClass = item.Availability === 'occupied' ? 'occupied' : 'vacant';
                    var RegistrationNumber = item.RegistrationNumber == null ? "" : item.RegistrationNumber;
                    var newRow = '<tr>' +
                        '<td>' + item.ParkingSpaceTitle + '</td>' +
                        '<td class="' + statusClass + '">' + item.Availability + '</td>' +
                        '<td>' + (item.RegistrationNumber == null ? '<input type="text" id="Registrationno' + item.ParkingSpaceId + '" name="Registrationno" />' : RegistrationNumber) + '</td>' +
                        '<td>' + (item.RegistrationNumber == null ? '<input type="button" class="addparking" id="AddParking' + item.ParkingSpaceId + '" name="Submit" value="Add" onclick="bookSpaceById(this)" />' : '<input type="button" class="freeparking" id="FreeParking' + item.ParkingSpaceId + '" name="Submit" value="Free" onclick="freeSpaceById(this)"/>') + '</td>' +
                        '</tr>';
                    $('#parkingTable tbody').append(newRow);

                });
            }
            else {
                var header = '<tr>' +
                    '<th>' + "Parking Space Title" + '</th>' +
                    '<th>' + "Status" + '</th>' +
                    '<th>' + "Vehicle Registration Number" + '</th>' +
                    '</tr>';
                $('#parkingTable thead').append(header);
                $.each(data.allspace, function (index, item) {
                    var statusClass = item.Availability === 'occupied' ? 'occupied' : 'vacant';
                    var RegistrationNumber = item.RegistrationNumber == null ? "" : item.RegistrationNumber;
                    var newRow = '<tr>' +
                        '<td>' + item.ParkingSpaceTitle + '</td>' +
                        '<td class="' + statusClass + '">' + item.Availability + '</td>' +
                        '<td>' + RegistrationNumber + '</td>' +
                        '</tr>';
                    $('#parkingTable tbody').append(newRow);
                });
            }
        },
        error: function (error) {
            console.log('Error fetching data: ', error);
        }
    });
}

$('#showAll').on('click', function () {
    showAllData();
});

$('#deleteAll').on('click', function () {
    var userConfirmation = confirm('Are you sure you want to delete all transactions?');

    if (userConfirmation) {
        $.ajax({
            url: '/Dashboard/DeleteAllTransaction',
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                if (result.success) {
                    alert('Data Deleted successfully!');
                    fetchAllData();
                } else {
                    alert('Failed to Delete');
                }
            },
            error: function () {
                alert('Failed to delete');
            }
        });
    } else {
        alert('Deletion canceled by user.');
    }
});


function showAllData() {
    $('#parkingTable tbody tr').each(function () {
        $(this).show();
    });
}

function filterSpaces() {
    var selectedZone = $('#filterZone option:selected').text();

    $('#parkingTable tbody tr').each(function () {
        var spaceTitle = $(this).find('td:first').text();
        var showRow = spaceTitle.startsWith(selectedZone);

        if (showRow) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
}

function bookSpaceById(e) {
    var registrationno = document.getElementById("Registrationno" + e.id.slice("AddParking".length,)).value;
    var indianVehicleNumberPattern = /^[A-Z]{2}\s\d{1,2}\s[A-Z0-9]{1,4}\s\d{1,4}$/;
    if (indianVehicleNumberPattern.test(registrationno)) {
        var id = e.id.slice("AddParking".length,);
        var parkingspacemodel = {
            "ParkingSpaceId": parseInt(id),
            "RegistrationNumber": registrationno
        }
        $.ajax({
            url: '/Dashboard/BookParkingSpaceById',
            type: 'POST',
            data: parkingspacemodel,
            dataType: 'json',
            success: function (result) {
                if (result.success) {
                    alert('Car booked successfully!');
                    $("Registrationno" + e.id.slice("AddParking".length,)).val("");
                    fetchAllData();
                } else {
                    alert('Failed to book parking space.');
                }
            },
            error: function () {
                alert('Error while making the AJAX call.');
            }
        });
    }
    else {
        alert("Enter a valid number");
    }
}

function freeSpaceById(e) {
    var id = e.id.slice("FreeParking".length,);
    var parkingspacemodel = {
        "ParkingSpaceId": parseInt(id),
    }
    $.ajax({
        url: '/Dashboard/FreeParkingSpaceById',
        type: 'POST',
        data: parkingspacemodel,
        dataType: 'json',
        success: function (result) {
            if (result.success) {
                alert('Car Released successfully!');
                $("Registrationno" + e.id.slice("AddParking".length,)).val("");
                fetchAllData();
            } else {
                alert('Failed to release parking space.');
            }
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });

}

function bookSpace() {
    var registrationNumber = $('#registrationNumber').val();
    var indianVehicleNumberPattern = /^[A-Z]{2}\s\d{1,2}\s[A-Z0-9]{1,4}\s\d{1,4}$/;

    // Check if the registrationNumber matches the pattern
    if (indianVehicleNumberPattern.test(registrationNumber)) {

        $.ajax({
            url: '/Dashboard/BookParkingSpace',
            type: 'POST',
            data: { vehicleRegistrationNumber: registrationNumber },
            dataType: 'json',
            success: function (result) {
                if (result.success) {
                    alert('Car booked successfully!');
                    $('#registrationNumber').val("");
                    fetchAllData();
                } else {
                    alert('Failed to book parking space.');
                }
            },
            error: function () {
                alert('Error while making the AJAX call.');
            }
        });
    } else {
        alert('Not a valid Registration Number')
    }
}

function releaseSpace() {
    var registrationNumber = $('#releaseRegistrationNumber').val();
    $.ajax({
        url: '/Dashboard/FreeParkingSpace',
        type: 'POST',
        data: { vehicleRegistrationNumber: registrationNumber },
        dataType: 'json',
        success: function (result) {
            if (result.success) {
                alert('Car released successfully!');
                $('#releaseRegistrationNumber').val("");
                fetchAllData();
            } else {
                alert('Failed to release parking space.');
            }
        },
        error: function () {
            alert('Error while making the AJAX call.');
        }
    });
}