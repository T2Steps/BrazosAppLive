var dataTable;
//$('#encryptedEstId').val(data.encryptid);
function loadDataTable(choice, isEdit) {
      var id = $('#estId').val();
      var encryptedid = $('#encryptedEstId').val();
      var lang = $('#lang').val();
      if (lang == undefined) {
            lang = 1;
      }
      var url = window.location.href;
      var columns = [];
      var columnDefs = null;
      if (isEdit == "Yes") {
            columns = [
                  {
                        data: 'encryptedId',
                        render: function (data, type, row, meta) {
                              return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                        }
                        , "width": "10%"
                  },
                  {
                        data: 'docFileName',
                        render: function (data, type, row, meta) {
                              if (lang == 1) {
                                    //var href = "/ViewDoc?id=" + encodeURIComponent(id) + "&file=" + ;
                                    //return '<a id="lnk" role="button" href= "' + "../../PermitDocuments/Food/" + id + "/" + data + '"target="_blank" ><i  class="fas fa-file-pdf ml-4"  style="color:#022E5F; cursor:pointer" title="View Doc"></i> </a> '
                                    //return '<a id="lnk" role="button" href='${href}' target="_blank"><i  class="fas fa-file-pdf ml-4"  style="color:#022E5F; cursor:pointer" title="View Doc"></i> </a> '
                                    var href = "/ViewDoc?id=" + encryptedid + "&file=" + row.encryptedId;

                                    return `<a id="lnk_${meta.row}" role="button" href="${href}" target="_blank">
                                            <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="View Doc"></i>
                                        </a>`;
                              }
                              else if (lang == 2) {
                                    //return '<a id="lnk" role="button" href= "' + "../../PermitDocuments/Food/" + id + "/" + data + '"target="_blank" ><i  class="fas fa-file-pdf ml-4"  style="color:#022E5F; cursor:pointer" title="Vista"></i> </a> '
                                    var href = "/ViewDoc?id=" + encryptedid + "&file=" + row.encryptedId;

                                    return `<a id="lnk_${meta.row}" role="button" href="${href}" target="_blank">
                                            <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="Vista"></i>
                                        </a>`;
                              }

                        }, "width": "10%"
                  },
                  { "data": "description", "width": "20%", className: "text-left" },
                  { "data": "associatedNote", "width": "30%", className: "text-left" },
                  { "data": "uploadedBy", "width": "10%" },
                  { "data": "uploadedOn", "width": "10%" },
                  {
                      "data": "encryptedId", "render": function (data, type, row, meta) {
                              if (!url.includes('View')) {
                                    if (lang == 1) {
                                          return ` <div class="m-75 btn-group" id="docIcons${meta.row}"  role="group">      
                                                <a class="btn btn-sm btn-block btn-outline-danger" onclick = DeleteDocs('/DeleteDoc?id=${data}')><i class="fa fa-trash" style="cursor:pointer" title="Delete"></i> </a>
                                          </div>`
                                    }
                                    else if (lang == 2) {
                                          return ` <div class="m-75 btn-group" id="docIcons${meta.row}"  role="group">      
                                                <a class="btn btn-sm btn-block btn-outline-danger" onclick = DeleteDocs('/DeleteDoc?id=${data}')><i class="fa fa-trash" style="cursor:pointer" title="Borrar"></i> </a>
                                          </div>`
                                    }

                              }
                              else {
                                    return null;
                              }

                        },
                        "width": "10%"
                  }
            ];

            columnDefs = [
                  //{
                  //      targets: [4],
                  //      render: function (data, type, row) {
                  //            var uploadDate = data.split("T");
                  //            uploadDate = moment(uploadDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                  //            return uploadDate;
                  //      }
                  //}
            ]
      }
      else {
            columns = [
                  {
                        data: 'encryptedId',
                        render: function (data, type, row, meta) {
                              return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                        }
                        , "width": "10%"
                  },
                  {
                        data: 'docFileName',
                        render: function (data, type, row, meta) {
                              if (lang == 1) {
                                    //return '<a id="lnk" role="button" href= "' + "../../PermitDocuments/Food/" + id + "/" + data + '"target="_blank" ><i  class="fas fa-file-pdf ml-4"  style="color:#022E5F; cursor:pointer" title="View Doc"></i> </a> '
                                    var href = "/ViewDoc?id=" + encryptedid + "&file=" + row.encryptedId;

                                    return `<a id="lnk_${meta.row}" role="button" href="${href}" target="_blank">
                                            <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="View Doc"></i>
                                        </a>`;
                              }
                              else if (lang == 2) {
                                    //return '<a id="lnk" role="button" href= "' + "../../PermitDocuments/Food/" + id + "/" + data + '"target="_blank" ><i  class="fas fa-file-pdf ml-4"  style="color:#022E5F; cursor:pointer" title="Vista"></i> </a> '
                                    var href = "/ViewDoc?id=" + encryptedid + "&file=" + row.encryptedId;

                                    return `<a id="lnk_${meta.row}" role="button" href="${href}" target="_blank">
                                            <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="Vista"></i>
                                        </a>`;
                              }

                        }, "width": "10%"
                  },
                  { "data": "description", "width": "30%", className: "text-left" },
                  { "data": "associatedNote", "width": "40%", className: "text-left" },
                  {
                        "data": "encryptedId", "render": function (data, type, row, meta) {
                              if (!url.includes('View')) {
                                    if (lang == 1) {
                                          return ` <div class="m-75 btn-group" id="docIcons${meta.row}"  role="group">      
                                                <a class="btn btn-sm btn-block btn-outline-danger" onclick = DeleteDocs('/DeleteDoc?id=${data}')><i class="fa fa-trash" style="cursor:pointer" title="Delete"></i> </a>
                                          </div>`
                                    }
                                    else if (lang == 2) {
                                          return ` <div class="m-75 btn-group" id="docIcons${meta.row}"  role="group">      
                                                <a class="btn btn-sm btn-block btn-outline-danger" onclick = DeleteDocs('/DeleteDoc?id=${data}')><i class="fa fa-trash" style="cursor:pointer" title="Borrar"></i> </a>
                                          </div>`
                                    }

                              }
                              else {
                                    return null;
                              }

                        },
                        "width": "10%"
                  }
            ]
      }
      
      dataTable = $('#docstable').DataTable({
            "responsive": true,
            "lengthChange": false,
            "autoWidth": false,
            "searching": choice,
            "ajax": {
                  "url": "/GetAllDocs?id=" + encryptedid,
                  "dataSrc": function (json) {
                        //console.log(json.count)
                        $('#Cnt').val(json.count);
                        if (isEdit != "Yes")
                        {
                              if ($('#Cnt').val() >= 5) {
                                    $('#uploadModalBtn').css('display', 'none');
                              }
                        }
                        
                        return json.data;
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

function Modalert() {
      var lang = $('#lang').val();
      var flg = 0;
      if (lang == undefined) {
            lang = 1;
      }
      var alertTxt = ""
      if (lang == 1) {
            alertTxt = "You have some unsaved Changes. On closing, your progress will be lost";
            if ($('#exampleInputFileEN').val() != "") {
                  flg = 1;
            }
      }
      else if (lang == 2) {
            alertTxt = "Tienes algunos cambios sin guardar.Al cerrar, tu progreso se perderá.";
            if ($('#exampleInputFileSP').val() != "") {
                  flg = 1;
            }
      }
      
      if ($('#description').val() != "") {
            flg = 1;
      }

      if (flg == 1) {
            if (confirm(alertTxt)) {
                  $('#documentUploadModal').modal('hide');
            }
      }
      else {
            $('#documentUploadModal').modal('hide');
      }
}

function clrall() {
    var lang = $('#lang').val();
    if(lang==undefined){
          lang = 1;
    }
    if (lang == 1) {
        $('.sp').css('display', 'none');
        $('.en').css('display', '');
        $('#exampleModalLabel').text("Upload Document")
        $('#description').attr('placeholder', "Description")
        $('#associateNote').attr('placeholder', "Associated Notes...")
    }
    else if (lang == 2) {
        $('.en').css('display', 'none');
        $('.sp').css('display', '');
        $('#exampleModalLabel').text("Cargar Documento")
        $('#description').attr('placeholder', "Descripción")
        $('#associateNote').attr('placeholder', "Notas asociadas...")
        $('#exampleInputFileSP').next('.custom-file-label').text('Ningún archivo elegido');
    }
    $('#documentUploadForm').trigger('reset');
    
}

function UploadDoc() {
      //alert(1);
      //console.log($('#exampleInputFileSP').val());
      
      var lang = $('#lang').val();
      var laluvulu = 0;
      if (lang == undefined) {
            lang = 1;
            laluvulu = 1;
      }
      if (lang == 1) {
            $('#uploadBtnen').prop('disabled', true)
      }
      else {
            $('#uploadBtnsp').prop('disabled', true)
      }
      var reqTxt = "";
      var fileTxt = "";
      var successmsg = "";
      var errormsg = ""
      if (lang == 1) {
            reqTxt = " Required Field "
            fileTxt = " No File Chosen To Upload "
            successmsg = "Uploaded Successfully"
            errormsg = "An Error Occurred While Uploading the File"
      }
      else if (lang == 2) {
            reqTxt = " Campo requerido "
            fileTxt = " No se ha elegido ningún archivo para cargar "
            successmsg = "Subido exitosamente"
            errormsg = "Se produjo un error al cargar el archivo"
      }

      var flg = 0;
      if ($('#description').val() == "") {
            $('#descriptionError').text("\u24d8" + reqTxt);
            setTimeout(() => {
                  $('#descriptionError').html("");
            }, 5000)
            flg = 1;
      }

      if (lang == 1) {
            if ($('#exampleInputFileEN').val() == "") {
                  $('#fileError').text("\u24d8" + fileTxt);
                  setTimeout(() => {
                        $('#fileError').html("");
                  }, 5000)
                  flg = 1;
            }
      }

      else if (lang == 2) {
            if ($('#exampleInputFileSP').val() == "") {
                  $('#fileError').text("\u24d8" + fileTxt);
                  setTimeout(() => {
                        $('#fileError').html("");
                  }, 5000)
                  flg = 1;
            }
      }

      if (flg == 1) {
            if (lang == 1) {
                  $('#uploadBtnen').prop('disabled', false)
            }
            else {
                  $('#uploadBtnsp').prop('disabled', false)
            }
            return false;
      }
      else {
            var formData = new FormData();
            if (lang == 1) {
                  formData.append('DocFile', $('#exampleInputFileEN')[0].files[0]);
            }
            else if (lang == 2) {
                  formData.append('DocFile', $('#exampleInputFileSP')[0].files[0]);
            }
            formData.append('Description', $('#description').val());
            formData.append('AssociatedNote', $('#associateNote').val());
            formData.append('EstId', $('#encryptedEstId').val());


            $('#documentUploadForm').trigger('reset');

            $.ajax({
                  type: "POST",
                  url: '/UploadDoc',
                  data: formData,
                  contentType: false,
                  processData: false,
                  beforeSend: function () {
                        $('div#loading-wrapper').show();
                        $('#spin').show();
                        $('#saveicon').hide();
                        //console.log(lang);
                        //if (lang == 1) {
                        //    $('#uploadBtnen').prop('disabled', true)
                        //}
                        //else {
                        //    $('#uploadBtnsp').prop('disabled', true)
                        //}
                  },
                  success: function (data) {
                        console.log(data);
                        if (data.success) {
                              $('audio#success_sound')[0].play();
                              setTimeout(() => {
                                    toastr.success(successmsg);
                              }, 500)
                              $('#Cnt').val(parseInt($('#Cnt').val()) + 1);
                              if (laluvulu == 0) {
                                    //console.log($('#Cnt').val());
                                    if (parseInt($('#Cnt').val()) > 4) {
                                          //alert(1);
                                          $('#uploadModalBtn').css('display', 'none')
                                    }
                              }
                              
                              //dataTable.ajax.reload();
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
                        $('#spin').hide();
                        $('#saveicon').show();
                        //$('#uploadBtn').prop('disabled', false)
                        if (lang == 1) {
                            $('#uploadBtnen').prop('disabled', false)
                        }
                        else {
                            $('#uploadBtnsp').prop('disabled', false)
                        }
                        $('#documentUploadModal').modal('hide');
                        dataTable.ajax.reload();
                  }
            });
      }
}

function ValidateFile(element) {
      var lang = $('#lang').val();
      if (lang == undefined) {
            lang = 1;
      }
      var requiredfileExtensions = ['jpg', 'png', 'pdf'];
      var fileExtension = element.val().split('.').pop().toLowerCase();
      if ($.inArray(fileExtension, requiredfileExtensions) == -1) {
            $('#fileError').html("");
            if (lang == 1) {
                  $('#fileError').text("\u24d8" + " Invalid File Format");
            }
            else if (lang == 2) {
                  $('#fileError').text("\u24d8" + " Formato de archivo inválido");
                  
            }
            //$('#fileError').text("Invalid File Format");
            setTimeout(() => {
                  $('#fileError').html("");
            }, 5000)
            element.val("");
      }
}

$('#exampleInputFile').bind('change', function () {
      var lang = $('#lang').val();
      if (lang == undefined) {
            lang = 1;
      }
      var maxSizeMB = 1;
      var maxSize = maxSizeMB * 1024 * 1024;
      if (this.files[0].size > maxSize) {
            $('#fileError').html("");
            if (lang == 2) {
                  $('#fileError').text("\u24d8" + " File Size too Large (File size should be less than 10 Mb)");
            }
            else if (lang == 1) {
                  $('#fileError').text("\u24d8" + " El tamaño del archivo es demasiado grande (el tamaño del archivo debe ser inferior a 10 Mb)");
            }
            //$('#fileError').text("File Size too Large (File size should be less than 10 Mb)");
            setTimeout(() => {
                  $('#fileError').html("");
            }, 5000)
            $(this).val("");
      }
});

function DeleteDocs(url) {
      var lang = $('#lang').val();
      var laluvulu = 0;
      if (lang == undefined) {
            lang = 1;
            laluvulu = 1;
      }
      var title = "";
      var text = "";
      var confirmButtonText = "";
      var cancelButtonText = "";
      var successmsg = "";
      var errormsg = "";

      if (lang == 1) {
            title = "Are you sure?";
            text = "You won't be able to revert this!";
            confirmButtonText = "Yes, delete it!";
            cancelButtonText = "Cancel";
            successmsg = "Deleted Successfully";
            errormsg = "Unexpected Error Occurred";
      }
      else if (lang == 2) {
            title = "¿Estas segura?";
            text = "¡No podrás revertir esto!";
            confirmButtonText = "¡Sí, bórralo!";
            cancelButtonText = "Cancelar";
            successmsg = "Borrado Exitosamente";
            errormsg = "Error al eliminar";
      }

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
                                          $('#Cnt').val(parseInt($('#Cnt').val()) - 1);
                                          //console.log($('#Cnt').val());
                                          if (laluvulu == 0) {
                                                if (parseInt($('#Cnt').val()) <= 4) {
                                                      //alert(1);
                                                      $('#uploadModalBtn').show();
                                                }
                                          }
                                          
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