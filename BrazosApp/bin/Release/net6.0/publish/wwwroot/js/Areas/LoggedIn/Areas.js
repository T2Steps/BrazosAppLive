var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = $('#areaIdx').DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "searching": false,
        "bProcessing": true,
        "ajax": {
            "url": "/GetAllAreas"
        },
        "columns": [
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                }, width: "5%", className: "text-center"
            },
            { "data": "area", width: "10%", className: "text-center" },
            { "data": "description", width: "35%" },
            { "data": "assignedUsernames", width: "40%" },
            {
                "data": "encryptedId", "render": function (data, type, row, meta) {
                    if (row.isActive == true) {
                        if (/*row.area != 0 && row.area != 8 && row.area != 12 &&*/ row.area != 51) {
                            return `<div class="m-75 btn-group"  role="group">
                                          <a class="btn btn-sm btn-custom" onclick=EditArea('/GetArea?id=${data}')><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit Area"></i></a>
                                          <a class="btn btn-sm btn-custom"  onclick=ManageUserModal('/GetAllAreaAssignedUsers?id=${data}')><i class="fa-solid fa-user-gear" aria-hidden="true" style="cursor:pointer" title="Manage Assigned Users"></i></a>
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeAreaState?id=${data}')><i class="fa-solid fa-xmark" id="icon"  style="cursor:pointer" title="Inactive"></i> </a>
                                    </div>`
                        }
                        else {
                            return `<div class="m-75 btn-group"  role="group">      
                                          <a class="btn btn-sm btn-custom" onclick=EditArea('/GetArea?id=${data}')><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit Area"></i></a>
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeAreaState?id=${data}')><i class="fa-solid fa-xmark" id="icon"  style="cursor:pointer" title="Inactive"></i> </a>
                                    </div>`
                        }
                    }
                    else {
                        return `<div class="m-75 btn-group"  role="group">      
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeAreaState?id=${data}')><i class="fa-solid fa-check" id="icon"  style="cursor:pointer" title="Active"></i> </a>
                                    </div>`
                    }
                }, width: "10%"

            },

        ],
        columnDefs: [
            {
                targets: [3],
                render: function (data, type, row) {
                    var str = `<ol>`;
                    for (let i = 0; i < data.length; i++) {
                        str = str.concat(`<li>${data[i].result}</li>`)
                    }
                    //var color = data;
                    str = str.concat(`</ol>`)
                    //console.log(str);
                    return str
                }
            },
        ],
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

function EditArea(url) {
    $('#AreaUpsertForm').trigger('reset');
    $.ajax({
        type: "GET",
        url: url,
        beforeSend: function () {
            $('div#loading-wrapper').show();
        },
        success: function (data) {
            //console.log(data)
            if (data.success) {
                $('#areaId').val(data.area.id);
                $('#isActive').val(data.area.isActive);
                $('#areaNumber').val(data.area.areaNumber);
                $('#description').val(data.area.description)
            }
        },
        error: function (data) {
            console.log(data);
        },
        complete: function () {
            $('div#loading-wrapper').hide();
            $('#areaUpsertModal').modal('show');
            
        }
    });
}

function Modalert() {
    var flg = 0;
    if ($('#areaNumber').val() != "") {
        flg = 1;
    }
    if ($('#description').val() != "") {
        flg = 1;
    }
    if (flg == 1) {
        if (confirm("You have some unsaved Changes. On closing, your progress will be lost")) {
            $('#areaUpsertModal').modal('hide');
        }
    }
    else {
        $('#areaUpsertModal').modal('hide');
    }
}

function saveArea()
{
    $('#areasaveBtn').prop('disabled', true)
    var flg = 0;
    if ($('#areaNumber').val() == "") {
        $('#areaNumberErr').text("ⓘ Required Field");
        setTimeout(() => {
            $('#areaNumberErr').html("");
        }, 4000)
        flg = 1;
    }
    if ($('#description').val() == "") {
        $('#descriptionErr').text("ⓘ Required Field");
        setTimeout(() => {
            $('#descriptionErr').html("");
        }, 4000)
        flg = 1;
    }

    if (flg == 1) {
        $('#areasaveBtn').prop('disabled', false)
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            url: "/AreaUpsert",
            data: $('#AreaUpsertForm').serialize(),
            beforeSend: function () {
                $('div#loading-wrapper').show();
                $('#spin').show();
                $('#saveicon').hide();
                //$('#areasaveBtn').prop('disabled', true)
            },
            success: function (data) {
                //console.log(data)
                if (data.success) {
                    $('audio#success_sound')[0].play();
                    setTimeout(() => {
                        toastr.success(data.msg);
                    }, 500)
                }
            },
            error: function (data) {
                console.log(data);
            },
            complete: function () {
                $('div#loading-wrapper').hide();
                $('#areasaveBtn').prop('disabled', false)
                $('#spin').hide();
                $('#saveicon').show();
                $('#areaUpsertModal').modal('hide');
                dataTable.ajax.reload();
            }
        })
    }
}

function ManageUserModal(url) {
    //console.log(url);
    $('#userManageData').empty();
    $('#manageUserForm').trigger('reset');
    $.ajax({
        type: "GET",
        url: url,
        beforeSend: function () {
            $('div#loading-wrapper').show();
        },
        success: function (data) {
            if (data.success) {
                console.log(data);
                //var cnt = parseInt($('#rcount').val());
                //console.log("Test")
                //console.log(cnt);
                $('#areaId').val(data.area.id);
                $('#areaNo').val(data.area.areaNumber);
                $('#areaDescription').val(data.area.description);
                //$('#ucolorcode').css('background-color', data.inspectors[0].territory.colorCode);
                for (let i = 0; i < data.inspectors.length; i++) {
                    $('#userManageData').append(
                        `<tr>
                                        <td width="10%">${i + 1}</td>
                                        <td style="display:none">${data.inspectors[i].encryptedId}</td>
                                        <td width="80%">${data.inspectors[i].assignedUser.firstName} ${data.inspectors[i].assignedUser.lastName}</td>
                                        <td width="10%"><a class="btn btn-sm btn-block btn-outline-danger"  onclick=RemoveInspector('/RemoveInspectorFromArea?id=${data.inspectors[i].encryptedId}')><i class="fa-solid fa-trash" aria-hidden="true" style="cursor:pointer" title="Delete"></i></a></td>
                                  </tr>`)
                    $('#rcount').val(i);
                }

            }
        },
        error: function (data) {
            console.log(data);
        },
        complete: function () {
            $('div#loading-wrapper').hide();
            $('#manageUserModal').modal('show');
            $('#rcount').val(parseInt($('#rcount').val()) + 1);
            //console.log($('#rcount').val());
        }
    });


}

function AssignInspector() {
    var flg = 0;
    if ($('#InspectorId').val() == null) {
        $('#inspectorAssignErr').text("ⓘ Required Field");
        setTimeout(() => {
            $('#inspectorAssignErr').html("");
        }, 5000)
        flg = 1;
    }

    if (flg == 1) {
        return false;
    }
    else {
        var formData = new FormData();
        formData.append('AreaWiseInspectors.AreaId', $('#areaId').val());
        //formData.append('AreaWiseInspectors.TypeId', $('#typeId').val());
        formData.append('AreaWiseInspectors.AssignedUserId', $('#InspectorId').val());
        //console.log(formData);
        //console.log($('#uterritoryId').val());
        //console.log($('#typeId').val());
        //console.log($('#InspectorId').val());
        $.ajax({
            type: "POST",
            url: "/AssignInspectorsToAreas",
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function () {
                $('div#loading-wrapper').show();
                //$('#spin').show();
                //$('#saveicon').hide();
            },
            success: function (data) {
                //console.log(data)
                if (data.success) {
                    var cnt = parseInt($('#rcount').val())+1;
                    //console.log("Test")
                    //console.log(cnt);
                    $('#userManageData').append(
                        `<tr>
                                          <td width="10%">${cnt}</td>
                                          <td style="display:none">${data.encryptedId}</td>
                                          <td width="80%">${data.name}</td>
                                          <td width="10%"><a class="btn btn-sm btn-block btn-outline-danger"  onclick=RemoveInspector('/RemoveInspectorFromArea?id=${data.encryptedId}')><i class="fa-solid fa-trash" aria-hidden="true" style="cursor:pointer" title="Delete"></i></a></td>
                                    </tr>`)
                    $('#rcount').val(parseInt($('#rcount').val()) + 1);
                    $('audio#success_sound')[0].play();
                    setTimeout(() => {
                        toastr.success(data.msg);
                    }, 500)

                }
                else {
                    $('audio#errorsound')[0].play();
                    setTimeout(() => {
                        toastr.error(data.msg);
                    }, 775)
                }
            },
            error: function (data) {
                console.log(data);
            },
            complete: function () {
                $('div#loading-wrapper').hide();
                $('#InspectorId').val("---Select---")
                //$('#spin').hide();
                //$('#saveicon').show();
                //$('#territoryModal').modal('hide');
                dataTable.ajax.reload();
            }
        })
    }
}

function RemoveInspector(url) {

    //console.log(url);
    $.ajax({
        type: "POST",
        url: url,
        beforeSend: function () {
            $('div#loading-wrapper').show();
        },
        success: function (data) {
            if (data.success) {
                //console.log(data);
                var rowToRemove = $('#userManageData td:contains("' + data.encryptedId + '")').closest('tr');
                rowToRemove.remove();
                $('#rcount').val(parseInt($('#rcount').val()) - 1);
                $('audio#success_sound')[0].play();
                setTimeout(() => {
                    toastr.success(data.msg);
                }, 500)
                $('#userManageData tr').each(function (index) {
                    $(this).find('td').eq(0).text(index + 1);
                });
            }
            else {
                $('audio#errorsound')[0].play();
                setTimeout(() => {
                    toastr.error(data.msg);
                }, 775)
            }
        },
        error: function (data) {
            console.log(data);
        },
        complete: function () {
            $('div#loading-wrapper').hide();
            //console.log($('#rcount').val());
            dataTable.ajax.reload();
        }
    });
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