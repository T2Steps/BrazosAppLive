var dataTable;

function loadDataTable(code) {
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
            "url": "/Schedules/GetAllSchedules?code=" + code,
              "type": "POST",
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
              }
            //headers: {
            //              'Authorization': `Bearer ${Tok}`,
            //              'Content-Type': 'application/json'
            //},
            //"dataSrc": function (data) {
            //    //console.log(data);
            //    if (data.success) {
            //        return data.response.result;
            //    } else {
            //        return [];
            //    }
            //}
        },
        "columns": [
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    //console.log(row);
                    return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                }, "width": "5%", className: "text-center"
            },
            { "data": "permit", "width": "15%", className: "text-left" },
            { "data": "name", "width": "20%" },
            { "data": "address", "width": "15%" },
            { "data": "purpose", "width": "10%" },
            { "data": "assignedTo", "width": "10%" },
            { "data": "scheduledDate", "width": "20%" },

            {
                "data": "encryptedId", "render": function (data, type, row, meta) {
                    var Insurl = "";
                    if (row.purpose == "Opening Inspection") {
                        Insurl = "/CreateOpeningInspection"
                    }
                    else if (row.purpose == "Walk Through") {
                        Insurl = "/CreateWalkThroughInspection"
                    }
                    else {
                        Insurl = "/CreateInspection"
                    }
                    return ` <div class="m-75 btn-group"  role="group">
                                        <a class="btn btn-sm btn-custom" onclick="SchedulerModal('/Schedules/GetSchedule?id=' + '${data}')" ><i class="fas fa-edit" style=" cursor:pointer" title="Edit"></i></a>
                                        <a class="btn btn-sm btn-custom"  href="${Insurl}?id=${data}"><i class="fa-solid fa-circle-play" aria-hidden="true" style="cursor:pointer" title="Proceed To Inspection"></i></a>
                                        <a class="btn btn-sm btn-block btn-outline-danger" onclick = DeleteSchedules('/Schedules/DeleteSchedule?id=${data}')><i class="fa fa-trash" style="cursor:pointer" title="Delete"></i> </a>
                                    </div>`
                    //return null;
                }, "width": "20%"

            }

        ],
        columnDefs: [
            {
                targets: [6],
                "type": "date",
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

    //$('#schedulerTableIdx thead tr:eq(0) th').each(function (i) {
    //    $('input', this).on('keyup change clear', function () {
    //        //console.log(this.value);
    //        //console.log(i);
    //        if (dataTable.column(i).search() !== this.value) {
    //            dataTable
    //                .column(i)
    //                .search(this.value)
    //                .draw();
    //        }
    //    });
    //});
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
    if (role == "Admin Inspector" || role == "SuperAdmin" || role =="Admin") {
        $('#AssignUserSection').show();
    }
    $('#estName').prop('readonly', false)
    //$('#permitNo').prop('readonly', false)
    $('#newScheduleForm').trigger('reset')
}

function SchModalert() {

      $('#scheduleModal').modal('hide');
}

function SchedulerModal(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            console.log(data);
            $('#SchId').val(data.schedule.id);
            $('#hdnestablishment').val(data.schedule.establishmentId);
            $('#estName').val(data.schedule.establishment.name);
            $('#permitNo').val(data.schedule.establishment.permitNumber);
            $('#estName').prop('readonly', true)
            //$('#permitNo').prop('readonly', true)
            var dateStr = data.schedule.scheduledDate.split('T')[0];
            $('#schdate').val(dateStr);
            $('#purpose').val(data.schedule.purposeId);
            $('#inspectorId').val(data.schedule.assignedTo);
            $('#scheduleModal').modal('show');
        }
    });
}



function DeleteSchedules(url) {
    
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
                    beforeSend: function () {
                        $('div#loading-wrapper').show();
                    },
                    success: function (data) {
                        if (data.success) {
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







