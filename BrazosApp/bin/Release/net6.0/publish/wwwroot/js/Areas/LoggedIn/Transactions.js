var dataTable;

function loadDataTable(code) {
    dataTable = $('#transactionTableIdx').DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": true,
        "searching": false,
        "deferRender": true,
        "bProcessing": true,
        "pageLength" : 50,
        "ajax": {
            "url": "/GetAllTransactions?code=" + code,
            "type": "POST",
            "data": function (d) {
                var formData = new FormData();
                formData.append('Name', $('#estName').val());
                formData.append('Permit', $('#permitNo').val());
                formData.append('Address', $('#estAddress').val());
                formData.append('Amount', $('#amountSearch').val());
                formData.append('FromDate', $('#lowerdaternge').val());
                formData.append('ToDate', $('#upperdaternge').val());

                var plainObject = {};
                formData.forEach(function (value, key) {
                    plainObject[key] = value;
                });
                return plainObject;
            }
        },
        "columns": [
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    //console.log(row);
                    return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                }, "width": "3%", className: "text-center"
            },
            { "data": "permit", "width": "8%", className: "text-left" },
            { "data": "name", "width": "10%" },
            { "data": "address", "width": "12%" },
            { "data": "invoice_Number", "width": "8%" },
            { "data": "receipt_Number", "width": "8%" },
            {
                  "data": "amount", "width": "5%", className: "text-center", 'render': function (data) {
                        //return data.toFixed(2);
                        return parseFloat(data).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                  }
            },
            { "data": "payment_Date", "width": "7%" },
            { "data": "account_Description", "width": "10%" },
            { "data": "account_Code", "width": "19%" },
        ],
        //columnDefs: [
        //    {
        //        targets: [6],
        //        render: function (data, type, row) {
        //            if (data == null) {
        //                return '--';
        //            }
        //            else {
        //                return data;
        //            }
        //        }
        //    }
        //],

        "language": {
            "emptyTable": "No Records found"
        },
        "width": "100%",
    });
}






