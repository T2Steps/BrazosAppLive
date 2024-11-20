var dataTable;
//$(function () {
//      loadDataTable();
//});

function loadDataTable(Role) {

      dataTable = $('#userManageIdx').DataTable({
          "responsive": true,
          "lengthChange": true,
          "autoWidth": false,
          "bProcessing": true,
            "ajax": {
                  "url": "/GetAllUsers"
            },
            "columns": [
                  {
                        data: 'id',
                        render: function (data, type, row, meta) {
                              return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                        }
                  },
                  { "data": "firstName" },
                  { "data": "lastName" },
                  { "data": "role.name"},
                  { "data": "bhcd" },
                  { "data": "emailId"},
                  { "data": "registeredSanitarian" },
                  { "data": "sanitarianInTrain" },
                  { "data": "designatedRepresentative" },
                  { "data": "certifiedPoolOperator" },
                  { "data": "certifiedPoolInspector" },
                  {
                        "data": "encryptedId", "render": function (data, type, row, meta) {
                              if (Role == "SuperAdmin") {
                                    if (row.isActive == true) {
                                          //if (row.roleId != 1) {

                                          //}
                                          //else {
                                          //    return null
                                          //}
                                          return `<div class="m-75 btn-group"  role="group">      
                                          <a class="btn btn-sm btn-custom"  onclick=UpsertModal('/GetUser?id=${data}')><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeUserState?id=${data}')><i class="fa-solid fa-xmark" id="icon"  style="cursor:pointer" title="Inactive"></i> </a>
                                       </div>`
                                    }
                                    else {
                                          return `<div class="m-75 btn-group"  role="group">      
                                          
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeUserState?id=${data}')><i class="fa-solid fa-check" id="icon"  style="cursor:pointer" title="Active"></i> </a>
                                          
                                    </div>`
                                    }
                              }
                              else {
                                    return null;
                              }
                              
                        }
                        
                  }

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



function UpsertModal(url)
{
    $('#userUpsertForm').trigger('reset');
    $.ajax({
        type: "GET",
        url: url,
        beforeSend: function () {
            $('div#loading-wrapper').show();
        },
        success: function (data) {
              console.log(data);
            if (data.success) {
                $('#userId').val(data.user.id);
                $('#firstName').val(data.user.firstName);
                $('#lastName').val(data.user.lastName);
                $('#bhcd').val(data.user.bhcd);
                $('#emailId').val(data.user.emailId);
                $('#rSanitarian').val(data.user.registeredSanitarian);
                $('#sanitarianTrain').val(data.user.sanitarianInTrain);
                $('#designRepresentative').val(data.user.designatedRepresentative);
                $('#poolOperator').val(data.user.certifiedPoolOperator);
                $('#certPoolInspector').val(data.user.certifiedPoolInspector);
                $('#roleId').val(data.user.roleId);
            }
        },
        error: function (data) {
            console.log(data);
        },
        complete: function () {
            $('div#loading-wrapper').hide();
            $('#userUpsert').modal('show');
        }
    });
}

