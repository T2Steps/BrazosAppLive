var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = $('#eventIdx').DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "searching": false,
        "bProcessing": true,
        "ajax": {
            "url": "/GetAllEvents"
        },
        "columns": [
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                }, width: "5%", className: "text-center"
            },
            { "data": "name", width: "30%" },
            { "data": "location", width: "35%" },
            { "data": "startDate", width: "10%" },
            { "data": "endDate", width: "10%" },
            {
                "data": "encryptedId", "render": function (data, type, row, meta) {
                    if (row.isActive == true) {
                        return `<div class="m-75 btn-group"  role="group">
                                          <a class="btn btn-sm btn-custom"  onclick=EditEvent('${data}')><i class="fas fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit Event"></i></a>
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeEventState?id=${data}')><i class="fa-solid fa-xmark" id="icon"  style="cursor:pointer" title="Inactive"></i> </a>
                                    </div>`
                    }
                    else {
                        return `<div class="m-75 btn-group"  role="group">
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeEventState?id=${data}')><i class="fa-solid fa-check" id="icon"  style="cursor:pointer" title="Active"></i> </a>
                                    </div>`
                    }
                }, width: "10%"

            },

        ],
        //columnDefs: [
        //    {
        //        targets: [3],
        //        render: function (data, type, row) {
        //            var str = `<ol>`;
        //            for (let i = 0; i < data.length; i++) {
        //                str = str.concat(`<li>${data[i].result}</li>`)
        //            }
        //            //var color = data;
        //            str = str.concat(`</ol>`)
        //            //console.log(str);
        //            return str
        //        }
        //    },
        //],
        "language": {
            "emptyTable": "No Records found"
        },
        "width": "100%",
        "createdRow": function (row, data, dataIndex) {
            if ($(data)[0].isActive == false) {
                $(row).css('color', 'red');
            }
        }
    });
}


function eventModalert()
{
    var flg = 0;
    var alertTxt = "You have some unsaved Changes. On closing, your progress will be lost";
    if ($('#name').val() != "") {
        flg = 1;
    }

    if ($('#location').val() != "") {
        flg = 1;
    }

    if ($('#startDt').val() != "") {
        flg = 1;
    }

    if ($('#endDt').val() != "") {
        flg = 1;
    }

    //console.log($('#startDt').val());
    //console.log($('#endDt').val())

    if (flg == 1) {
        if (confirm(alertTxt)) {
            $('#eventUpsertModal').modal('hide');
        }
    }
    else {
        $('#eventUpsertModal').modal('hide');
    }
    
}


function EditEvent(id)
{
    $('#eventUpsertModal').modal('show');
    $.ajax({
        type: "GET",
        url: '/GetEvent?id=' + id,
        success: function (data) {            
            if (data.success) {
                var startDate = data.event.startDate.split("T");
                var endDate = data.event.endDate.split("T");
                $('#eventId').val(data.event.id);
                $('#name').val(data.event.name);
                $('#location').val(data.event.location);
                $('#startDt').val(startDate[0]);
                $('#endDt').attr('min', startDate[0]);
                $('#endDt').val(endDate[0]);
                
            }
        }
    })
}

function EventUpsert()
{
    var flg = 0;
    
    if ($('#name').val() == "") {
        $('#nameError').text("ⓘ Required Field")
        flg = 1;
    }

    if ($('#location').val() == "") {
        $('#locationError').text("ⓘ Required Field")
        flg = 1;
    }

    if ($('#startDt').val() == "") {
        $('#startDtError').text("ⓘ Required Field")
        flg = 1;
    }

    if ($('#endDt').val() == "") {
        $('#endDtError').text("ⓘ Required Field")
        flg = 1;
    }

    if (flg == 1) {
        return false;
    }

    else {
        $.ajax({
            type: "POST",
            url: '/EventsUpsert',
            beforeSend: function () {
                $('div#loading-wrapper').show();
                $('#spin').show();
                $('#saveicon').hide();
                $('#submitBtn').prop('disabled', true);

            },
            data: $('#eventUpsertForm').serialize(),
            success: function (data) {
                if (data.success) {
                    $('audio#success_sound')[0].play();
                    setTimeout(() => {
                        toastr.success(data.msg);
                    }, 500)
                    dataTable.ajax.reload();
                }
                else {
                    $('audio#errorsound')[0].play();
                    setTimeout(() => {
                        toastr.error(data.msg);
                    }, 775)
                }
            },
            complete: function () {
                $('div#loading-wrapper').hide();
                $('#spin').hide();
                $('#saveicon').show();
                $('#submitBtn').prop('disabled', false);
                $('#eventUpsertModal').modal('hide');
            }
        })
    }
}

function ActiveInactive(url) {
    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: 'Are you sure?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#8B9BB2',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            $('audio#success_sound')[0].play();
                            setTimeout(() => {
                                toastr.success(data.msg);
                            }, 500)

                            //toastr.success(data.message);
                            dataTable.ajax.reload();
                        }
                    }
                });

            }
        })
    }, 100)

}