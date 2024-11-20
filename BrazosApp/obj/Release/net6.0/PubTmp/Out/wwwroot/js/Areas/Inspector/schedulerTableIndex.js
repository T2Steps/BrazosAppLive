var dataTable;

function loadDataTable(apiUrl, code, Tok, Role) {
    dataTable = $('#schedulerTableIdx').DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "searching": false,
        "deferRender": true,
        "bProcessing": true,
        //"searching": false,
        //"dom": "lrtp",
        //layout: {
        //    topEnd: {
        //        search: {
        //            css: 'display:none'
        //        }
        //    },
        //},
        //"dom": "lrtip",
        //"dom": '<"bottom"i>rt<"bottom"rp><"clear">',
        "ajax": {
            "type": "POST",
            "url": apiUrl + "/GetAllScheduledInspections?code=" + code,
            headers: {
                          'Authorization': `Bearer ${Tok}`,
                          //'Role': Role,
                          //'Content-Type': 'application/json'
            },
            
            "data": function (d) {
                var formData = new FormData();
                formData.append('SearchParamsVM.Name', $('#estNameidx').val());
                formData.append('SearchParamsVM.Permit', $('#permitNoidx').val());
                formData.append('SearchParamsVM.Address', $('#estAddressidx').val());
                formData.append('SearchParamsVM.Purpose', $('#purposeidx').val());
                formData.append('SearchParamsVM.AssignedTo', $('#assignedidx').val());
                formData.append('SearchParamsVM.FromDate', $('#lowerdaternge').val());
                formData.append('SearchParamsVM.ToDate', $('#upperdaternge').val());

                var plainObject = {};
                formData.forEach(function (value, key) {
                    plainObject[key] = value;
                });
                return plainObject;
            },
            "dataSrc": function (data) {
                  //console.log(data);
                  //console.log("Test This")
                if (data.success) {
                    return data.response.result.schedulerList;
                } else {
                    return [];
                }
            }
        },
        "columns": [
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    //console.log(row);
                    return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                }, "width": "5%", className: "text-center"
            },
            { "data": "permit", "width": "10%", className: "text-left" },
            { "data": "name", "width": "15%" },
            { "data": "address", "width": "15%" },
            { "data": "assignedTo", "width": "15%" },
            { "data": "purpose", "width": "15%" },
            { "data": "scheduledDate", "width": "20%" },

            {
                "data": "encryptedId", "render": function (data, type, row, meta) {
                    var Insurl = ""; 
                    if (row.purpose == "Opening Inspection") {
                        Insurl = "/OpeningInspection"
                    }
                    else if (row.purpose == "Walk Through") {
                        Insurl = "/WalkThroughInspection"
                    }
                    else {
                        Insurl = "/NewInspection"
                    }
                    return `<div class="m-75 btn-group"  role="group">      
                                          
                                          <a class="btn btn-sm btn-custom"  href="${Insurl}?id=${data}"><i class="fa-solid fa-circle-play" aria-hidden="true" style="cursor:pointer" title="Proceed To Inspection"></i></a>
                                          <a class="btn btn-sm btn-block btn-outline-danger" onclick = "DeleteSchedules('${apiUrl}/DeleteSchedule?id=${data}', '${Tok}')"><i class="fa fa-trash" style="cursor:pointer" title="Delete"></i> </a>

                                    </div>`


                }, "width": "20%"

            }

        ],
        columnDefs: [
            {
                targets: [6],
                render: function (data, type, row) {
                    var schDate = data.split("T");
                    schDate = moment(schDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                    return schDate;
                }
            },
        ],
        "language": {
            "emptyTable": "No Records found"
        },
        "width": "100%",
    });

    //alert(1);
    //setTimeout(function () {

    //}, 100);

    $('#schedulerTableIdx thead tr:eq(0) th').each(function (i) {
        $('input', this).on('keyup change clear', function () {
            //console.log(this.value);
            //console.log(i);
            if (dataTable.column(i).search() !== this.value) {
                dataTable
                    .column(i)
                    .search(this.value)
                    .draw();
            }
        });
    });
}


function nullify() {
      if ($('#estName').val() == "") {
            $('#permitNo').val("");
            $('#hdnestablishment').val("");
            //alert($('#flwUpchk').val());
      }
      else {
            //$('#txtEstablishment').removeClass(' border-danger');
            $('#estError').text("");
      }
}

function clrall(role) {
    if (role == "Admin Inspector" || role == "SuperAdmin") {
        $('#AssignUserSection').show();
    }
    $('#newScheduleForm').trigger('reset')
}

function SchModalert() {

      $('#scheduleModal').modal('hide');
}

function DeleteSchedules(url, Tok) {

    var title = "Are you sure?";;
    var text = "You won't be able to revert this!";
    var confirmButtonText = "Yes, delete it!";
    var cancelButtonText = "Cancel";
    var successmsg = "Deleted Successfully";
    var errormsg = "Unexpected Error Occurred";

    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#7aa1db',
            cancelButtonColor: '#d33',
            cancelButtonText: cancelButtonText,
            confirmButtonText: confirmButtonText
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "DELETE",
                    url: url,
                    headers: {
                        'Authorization': `Bearer ${Tok}`,
                    },
                    beforeSend: function () {
                        $('div#loading-wrapper').show();
                    },
                    success: function (data) {
                        if (data.response.isSuccess) {
                            $('audio#success_sound')[0].play();
                            setTimeout(() => {
                                toastr.success(successmsg);
                            }, 500)
                        }
                        else {
                            $('audio#errorsound')[0].play();
                            setTimeout(() => {
                                toastr.error(errormsg);
                            }, 775)
                        }
                    },
                    error: function (data) {
                        console.log(data);
                    },
                    complete: function () {
                        $('div#loading-wrapper').hide();
                        dataTable.ajax.reload();
                    }
                });

            }
        })
    }, 100)

}







