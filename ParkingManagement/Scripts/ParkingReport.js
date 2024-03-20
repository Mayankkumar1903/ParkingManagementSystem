function formatDate(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

const currentDate = new Date();

document.getElementById('startDate').value = formatDate(currentDate);
document.getElementById('endDate').value = formatDate(currentDate);
function generateReport() {
    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();
    if (endDate < startDate) {
        alert("Can't procced");
    }
    else {
        console.log(endDate);
        $.ajax({
            url: '/Export/Export',
            type: 'GET',
            data: { startDate: startDate, endDate: endDate },
            dataType: 'json',
            success: function (data) {
                var reportContent = `<h3>Generated Report</h3>`;

                reportContent += `<table id="reporttable">
                            <tr>
                                <th>Parking Zone</th>
                                <th>Parking Space</th>
                                <th># of Booking</th>
                                <th># of Vehicle Parked (0/1)</th>
                            </tr>`;

                for (var i = 0; i < data.length; i++) {
                    var entry = data[i];

                    if (i === 0 || entry.ParkingZone !== data[i - 1].ParkingZone) {
                        reportContent += `<tr>
                                        <td rowspan="${data.filter(item => item.ParkingZone === entry.ParkingZone).length}">${entry.ParkingZone}</td>
                                        <td>${entry.ParkingSpace}</td>
                                        <td>${entry.NumberOfBookings}</td>
                                        <td>${entry.NumberOfVehiclesParked}</td>
                                    </tr>`;
                    } else {
                        reportContent += `<tr>
                                    <td>${entry.ParkingSpace}</td>
                                    <td>${entry.NumberOfBookings}</td>
                                    <td>${entry.NumberOfVehiclesParked}</td>
                                </tr>`;
                    }
                }

                reportContent += `</table>`;

                document.getElementById("reportContainer").innerHTML = reportContent;
            },
            error: function (error) {
                console.log(error);

                alert('Export failed. Check the console for error details.');
            }
        });
    }
}

function generatePDF() {


    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();
    if (endDate < startDate) {
        alert("Can't procced");
    }
    else {
        $.ajax({
            url: '/Export/Export',
            type: 'GET',
            data: { startDate: startDate, endDate: endDate },
            dataType: 'json',
            success: function (data) {
                var reportContent = `<h3>Generated Report</h3>`;

                reportContent += `<div><table id="reporttable">
                <tr>
                    <th>Parking Zone</th>
                    <th>Parking Space</th>
                    <th># of Booking</th>
                    <th># of Vehicle Parked (0/1)</th>
                </tr>`;

                for (var i = 0; i < data.length; i++) {
                    var entry = data[i];
                    reportContent += `<tr>
                        <td>${entry.ParkingZone}</td>
                        <td>${entry.ParkingSpace}</td>
                        <td>${entry.NumberOfBookings}</td>
                        <td>${entry.NumberOfVehiclesParked}</td>
                    </tr>`;
                }

                reportContent += `</table></div>`;

                var pdf = new jsPDF();

                pdf.fromHTML(reportContent, 15, 30);

                pdf.save('exported_report.pdf');

            },
            error: function (error) {
                console.log(error);

                alert('Export failed. Check the console for error details.');
            }
        });
    }
}