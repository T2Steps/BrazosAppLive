var dataTable;

function loadDataTable(code, Role) {
    var columns = [];
    var columnDefs = [];
    if (Role == "Inspector") {
        columns = [
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }, "width": "5%", className: "text-center"
            },
            { "data": "area", "width": "10%", className: "text-left" },
            { "data": "permitNumber", "width": "10%", className: "text-left" },
            { "data": "name", "width": "20%" },
            { "data": "address", "width": "20%" },
            { "data": "purpose", "width": "10%" },
            { "data": "scheduledDate", "width": "10%" },        
            { "data": "isFollowUpInspection", "width": "10%", className: "text-center" },           
        ]

        columnDefs = [
            {
                targets: [6],
                "type": "date",
                render: function (data, type, row) {
                    var insDate = data.split("T");
                    insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                    return insDate;
                }
            },
            {
                targets: [7],
                render: function (data, type, row) {
                    if (data == true) {
                        return `<p class="badge bg-danger">YES</p>`
                    }
                    return `<p class="badge bg-success">No</p>`
                }
            },
        ]
    }
    else {
        columns = [
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }, "width": "5%", className: "text-center"
            },
            { "data": "area", "width": "10%", className: "text-left" },
            { "data": "permitNumber", "width": "10%", className: "text-left" },
            { "data": "name", "width": "20%" },
            { "data": "address", "width": "20%" },
            { "data": "purpose", "width": "10%" },
            { "data": "scheduledDate", "width": "10%" },
            { "data": "assignedTo", "width": "10%" },
            { "data": "isFollowUpInspection", "width": "10%", className: "text-center" },
        ]

        columnDefs = [
            {
                targets: [6],
                "type": "date",
                render: function (data, type, row) {
                    var insDate = data.split("T");
                    insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                    return insDate;
                }
            },
            {
                targets: [8],
                render: function (data, type, row) {
                    if (data == true) {
                        return `<p class="badge bg-danger">YES</p>`
                    }
                    return `<p class="badge bg-success">No</p>`
                }
            },
        ]

    }

    dataTable = $('#areawisePendingInspectionsIdx').DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "searching": false,
        "deferRender": true,
        "bProcessing": true,
        "ajax": {
            "url": "/GetAllAreaWisePendingInspections?code=" + code,
            "type": "POST",
            "data": function (d) {
                var formData = new FormData();
                formData.append('Name', $('#estName').val());
                formData.append('Permit', $('#permitNo').val());
                formData.append('Area', $('#areaNo').val());
                formData.append('Purpose', $('#purposeidx').val());
                formData.append('Inspector', $('#insidx').val());
                formData.append('FromDate', $('#lowerdaternge').val());
                formData.append('ToDate', $('#upperdaternge').val());

                var plainObject = {};
                formData.forEach(function (value, key) {
                    plainObject[key] = value;
                });
                return plainObject;
            }
        },
        "columns": columns,
        columnDefs: columnDefs,

        "language": {
            "emptyTable": "No Records found"
        },
        "width": "100%",
    });
}