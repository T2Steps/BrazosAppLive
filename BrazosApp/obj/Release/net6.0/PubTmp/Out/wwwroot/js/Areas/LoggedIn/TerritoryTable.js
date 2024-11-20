var dataTable;
$(function () {
      loadDataTable();
});

function loadDataTable() {

      dataTable = $('#territoryIdx').DataTable({
            "responsive": true,
            "lengthChange": false,
            "autoWidth": false,
            "ajax": {
                  "url": "/GetAllTerritories"
            },
            "columns": [
                  {
                        data: 'id',
                        render: function (data, type, row, meta) {
                              return meta.row + meta.settings._iDisplayStart + 1;
                        }, width: "5%"
                  },
                  { "data": "name", width: "20%" },
                  { "data": "colorCode", width: "15%" },
                  { "data": "assignedUsernames", width: "50%" },
                  {
                        "data": "encryptedId", "render": function (data, type, row, meta) {
                              if (row.isActive == true) {
                                    return `<div class="m-75 btn-group"  role="group">      
                                          <a class="btn btn-sm btn-custom"  onclick=UpsertTerritory('/GetTerritory?id=${data}')><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit Territory"></i></a>
                                          <a class="btn btn-sm btn-custom"  onclick=ManageUserModal('/GetAllTerritoryAssignedUsers?id=${data}')><i class="fa-solid fa-user-gear" aria-hidden="true" style="cursor:pointer" title="Manage Assigned Users"></i></a>
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeTerritoryState?id=${data}')><i class="fa-solid fa-xmark" id="icon"  style="cursor:pointer" title="Inactive"></i> </a>
                                    </div>`
                              }
                              else {
                                    return `<div class="m-75 btn-group"  role="group">      
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeTerritoryState?id=${data}')><i class="fa-solid fa-check" id="icon"  style="cursor:pointer" title="Active"></i> </a>
                                    </div>`
                              }
                        }, width: "10%"

                  },

            ],
            columnDefs: [
                  {
                        targets: [2],
                        render: function (data, type, row) {
                              var color = data;
                              return `<div style="background-color:${color};width:100%;height:20px"></div>`
                        }
                  },
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


function UpsertTerritory(url) {
      $('#TerritoryUpsertForm').trigger('reset');
      $.ajax({
            type: "GET",
            url: url,
            beforeSend: function () {
                  $('div#loading-wrapper').show();
            },
            success: function (data) {
                  if (data.success) {
                        $('#territoryId').val(data.territory.territoryId);
                        $('#territoryName').val(data.territory.name);
                        $('#colordata').val(data.territory.colorCode);
                        $('#defaultInspectorId').val(data.territory.defaultInspectorId);
                        $('#colorBox').css('color', data.territory.colorCode);
                        $('.my-colorpicker2').colorpicker('setValue', data.territory.colorCode);
                  }
            },
            error: function (data) {
                  console.log(data);
            },
            complete: function () {
                  $('div#loading-wrapper').hide();
                  $('#territoryModal').modal('show');
            }
      });
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
                        //console.log(data);
                        $('#uterritoryId').val(data.inspectors[0].territory.id);
                        $('#uname').val(data.inspectors[0].territory.name);
                        $('#ucolorcode').val(data.inspectors[0].territory.colorCode);
                        $('#ucolorcode').css('background-color', data.inspectors[0].territory.colorCode);
                        for (let i = 0; i < data.inspectors.length; i++) {
                              if (data.inspectors[i].typeId == 1 || data.inspectors[i].typeId == 2) {
                                    $('#userManageData').append(
                                          `<tr>
                                        <td width="10%">${i + 1}</td>
                                        <td width="80%">${data.inspectors[i].assignedUser.firstName} ${data.inspectors[i].assignedUser.lastName}</td>
                                        
                                  </tr>`)
                              }
                              else if (data.inspectors[i].typeId == 3) {
                                    $('#userManageData').append(
                                          `<tr>
                                        <td width="10%">${i + 1}</td>
                                        <td style="display:none">${data.inspectors[i].encryptedId}</td>
                                        <td width="80%">${data.inspectors[i].assignedUser.firstName} ${data.inspectors[i].assignedUser.lastName}</td>
                                        <td width="10%"><a class="btn btn-sm btn-block btn-outline-danger"  onclick=RemoveInspector('/RemoveInspectorFromTerritory?id=${data.inspectors[i].encryptedId}')><i class="fa-solid fa-trash" aria-hidden="true" style="cursor:pointer" title="Delete"></i></a></td>
                                  </tr>`)
                              }
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
            formData.append('TerritoryWiseInspectors.TerritoryId', $('#uterritoryId').val());
            formData.append('TerritoryWiseInspectors.TypeId', $('#typeId').val());
            formData.append('TerritoryWiseInspectors.AssignedUserId', $('#InspectorId').val());
            //console.log(formData);
            //console.log($('#uterritoryId').val());
            //console.log($('#typeId').val());
            //console.log($('#InspectorId').val());
            $.ajax({
                  type: "POST",
                  url: "/AssignInspectorsToTerritory",
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
                              var cnt = parseInt($('#rcount').val());
                              $('#userManageData').append(
                                    `<tr>
                                          <td width="10%">${cnt + 1}</td>
                                          <td style="display:none">${data.encryptedId}</td>
                                          <td width="80%">${data.name}</td>
                                          <td width="10%"><a class="btn btn-sm btn-block btn-outline-danger"  onclick=RemoveInspector('/RemoveInspectorFromTerritory?id=${data.encryptedId}')><i class="fa-solid fa-trash" aria-hidden="true" style="cursor:pointer" title="Delete"></i></a></td>
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
            type: "DELETE",
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