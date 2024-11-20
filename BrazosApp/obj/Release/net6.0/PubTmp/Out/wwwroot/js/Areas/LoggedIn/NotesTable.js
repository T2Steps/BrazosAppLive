var notesdataTable;

function loadNotesDataTable() {
      var id = $('#estId').val();
      var encryptedid = $('#encryptedEstId').val();
      var role = $('#rl').val();
      var user = $('#username').val();
      var url = window.location.href;
      notesdataTable = $('#notestable').DataTable({
            "responsive": true,
            "lengthChange": false,
            "autoWidth": false,
            "bProcessing": true,
            "ajax": {
                  "url": "/GetAllNotes?id=" + encryptedid,
            },

            "columns": [
                  {
                        data: 'encryptedId',
                        render: function (data, type, row, meta) {
                              return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                        }
                        , "width": "5%", className: "text-left"
                  },
                  { "data": "title", "width": "20%", className: "text-left" },
                  { "data": "description", "width": "40%", className: "text-left" },
                  { "data": "uploadedOn", "width": "15%", className: "text-left" },
                  { "data": "uploadedBy", "width": "15%", className: "text-left" }, 
                  {
                        "data": "encryptedId", "render": function (data, type, row, meta) {
                              if (!url.includes('View')) {
                                  if (user == row.uploadedBy) {
                                      return ` <div class="m-75 btn-group" id="docIcons[0]"  role="group">
                                                <a class="btn btn-sm btn-custom"  onclick=NotesModal('/GetNote?id=${data}')><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
                                                <a class="btn btn-sm btn-block btn-outline-danger" onclick = DeleteNotes('/DeleteNote?id=${data}')><i class="fa fa-trash" style="cursor:pointer" title="Delete"></i> </a>
                                          </div>`
                                  }
                                  else if (role == "SuperAdmin") {
                                      return ` <div class="m-75 btn-group" id="docIcons[0]"  role="group">      
                                                <a class="btn btn-sm btn-block btn-outline-danger" onclick = DeleteNotes('/DeleteNote?id=${data}')><i class="fa fa-trash" style="cursor:pointer" title="Delete"></i> </a>
                                          </div>`
                                  }
                                  else {
                                      return null;
                                  }

                              }
                              else {
                                    return null;
                              }

                        },
                        "width": "5%"
                  }
            ],
            //columnDefs: [
            //      {
            //            targets: [3],
            //            render: function (data, type, row) {
            //                  var uploadDate = data.split("T");
            //                  uploadDate = moment(uploadDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
            //                  return uploadDate;
            //            }
            //      }
            //],
            "language": {
                  "emptyTable": "No Records found"
            },
            "width": "100%",

      });

}


function notesclrall()
{
      $('#notesUploadForm').trigger('reset');
}

function nModalert() {
      var flg = 0;
      var alertTxt = "You have some unsaved Changes. On closing, your progress will be lost";
      if ($('#ntitle').val() != "") {
            flg = 1;
      }

      if ($('#ndescription').val() != "") {
            flg = 1; 
      }

      if (flg == 1) {
            if (confirm(alertTxt)) {
                  $('#notesUploadModal').modal('hide');
            }
      }
      else {
            $('#notesUploadModal').modal('hide');
      }
}


function NotesUpsert() {
      $('#notesFormSubmitBtn').prop('disabled', true)
      var reqTxt = " Required Field ";
      var flg = 0;
      if ($('#ntitle').val() == "") {
            $('#ntitleError').text("\u24d8" + reqTxt);
            setTimeout(() => {
                  $('#ntitleError').html("");
            }, 5000)
            flg = 1;
      }

      if ($('#ndescription').val() == "") {
            $('#ndescriptionError').text("\u24d8" + reqTxt);
            setTimeout(() => {
                  $('#ndescriptionError').html("");
            }, 5000)
            flg = 1;
      }

      if (flg == 1) {
            $('#notesFormSubmitBtn').prop('disabled', false)
            return false;
      }
      else {
            var formData = new FormData();
            formData.append('NoteId', $('#noteId').val());
            formData.append('Title', $('#ntitle').val());
            formData.append('Description', $('#ndescription').val());
            formData.append('EstablishmentId', $('#estId').val());

            $('#notesUploadForm').trigger('reset');

            $.ajax({
                  type: "POST",
                  url: '/NotesUpsert',
                  data: formData,
                  contentType: false,
                  processData: false,
                  beforeSend: function () {
                        $('div#loading-wrapper').show();
                        $('#spin').show();
                        $('#saveicon').hide();
                        /*$('#notesFormSubmitBtn').prop('disabled', true)*/
                  },
                  success: function (data) {
                        console.log(data);
                        if (data.success) {
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
                        $('#spin').hide();
                        $('#saveicon').show();
                        $('#notesFormSubmitBtn').prop('disabled', false)
                        $('#notesUploadModal').modal('hide');
                        notesdataTable.ajax.reload();
                  }
            });
      }
}

function NotesModal(url)
{
      $.ajax({
            type: "GET",
            url: url,
            beforeSend: function () {
                  $('div#loading-wrapper').show();
            },
            success: function (data) {
                  console.log(data.note); 
                  $('#noteId').val(data.note.noteId);
                  $('#ntitle').val(data.note.title);
                  $('#ndescription').val(data.note.description)
                  $('#notesUploadModal').modal('show');
            },
            error: function (data) {
                  console.log(data);
            },
            complete: function () {
                  $('div#loading-wrapper').hide();
            }
      })
}


function DeleteNotes(url) {
      $('audio#warning')[0].play();
      setTimeout(() => {
            Swal.fire({
                  title: "Are you sure?",
                  text: "You won't be able to revert this!",
                  icon: 'warning',
                  showCancelButton: true,
                  confirmButtonColor: '#7aa1db',
                  cancelButtonColor: '#d33',
                  cancelButtonText: "Cancel",
                  confirmButtonText: "Yes, delete it!"
            }).then((result) => {
                  if (result.isConfirmed) {
                        $.ajax({
                              type: "DELETE",
                              url: url,
                              beforeSend: function () {
                                    $('div#loading-wrapper').show();
                              },
                              success: function (data) {
                                  debugger;
                                  console.log(data);
                                    if (data.success) {
                                          $('audio#success_sound')[0].play();
                                          setTimeout(() => {
                                              toastr.success(data.message);
                                          }, 500)
                                    }
                                    else {
                                          $('audio#errorsound')[0].play();
                                          setTimeout(() => {
                                              toastr.error(data.message);
                                          }, 775)
                                    }
                              },
                              error: function (data) {
                                    console.log(data);
                              },
                              complete: function () {
                                    $('div#loading-wrapper').hide();
                                    notesdataTable.ajax.reload();
                              }
                        });

                  }
            })
      }, 100)

}