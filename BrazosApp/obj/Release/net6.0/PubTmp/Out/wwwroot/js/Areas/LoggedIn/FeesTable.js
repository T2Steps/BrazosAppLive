var dataTable;

$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#feesDataTable').DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "bProcessing": true,
        "ajax": {
            "url": "/GetAllFees"
        },
        "columns": [
            { "data": "result.program", width: "8%" },
            { "data": "result.jurisdiction", width: "10%" },
            { "data": "result.description", width: "26%" },
            { "data": "result.accountCode", width: "20%" },
            { "data": "result.accountDescription", width: "20%" },
            {
                "data": "result.fee", width: "4%", 'render': function (fee) {
                    //console.log(fee);
                    return '$' + fee.toFixed(2);
                }
            },
            {
                "data": "result.q2", width: "4%", 'render': function (q2) {
                    return '$' + q2.toFixed(2);
                }
            },
            {
                "data": "result.q3", width: "4%", 'render': function (q3) {
                    return '$' + q3.toFixed(2);
                }
            },
            {
                "data": "result.q4", width: "4%", 'render': function (q4) {
                    return '$' + q4.toFixed(2);
                }
            },
        ],
        "language": {
                "emptyTable": "No Records found"
        },
        "width": "100%",
        "order": []
      });
}